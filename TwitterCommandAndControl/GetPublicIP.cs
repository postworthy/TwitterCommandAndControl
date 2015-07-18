using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCommandAndControl
{
    public static class GetPublicIP
    {
        private static string ip = null;
        public static string GetIP()
        {
#if DEBUG
            ip = "127.0.0.1";
            return ip;
#else
            if (ip == null)
            {
                using (var req = new WebClient())
                {
                    ip = req.DownloadString("http://checkip.dyndns.org").Split(':').Last().Split('<').First().Trim();
                    if (ip.Where(x => x == '.').Count() != 3)
                        ip = null;
                }
            }

            return ip;
#endif
        }
    }
}

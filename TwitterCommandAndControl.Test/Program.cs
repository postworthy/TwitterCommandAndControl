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
            }, 
            track: hashTag, 
            log: Console.Out);
            
            while (!tracker.IsActive) ;

            Console.WriteLine("Starting Test");

            for (var i = 0; i < 10; i++)
            {
                Messenger.Send(hashTag + " Test " + i);
                Thread.Sleep(1000);
            }

            Console.WriteLine("Test Complete");
            Console.WriteLine("Press Any Key To Exit");

            Console.ReadLine();

            //tracker.Wait();
        }
    }
}

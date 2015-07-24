using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCommandAndControl.Core
{
    public static class Messenger
    {
        public static void Send(string msg)
        {
            TwitterContext context = new TwitterContext(new MvcAuthorizer()
            {
                CredentialStore = new LinqToTwitter.InMemoryCredentialStore()
                {
                    OAuthTokenSecret = ConfigurationManager.AppSettings["OAuthTokenSecret"],
                    ConsumerKey = ConfigurationManager.AppSettings["ConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"],
                    OAuthToken = ConfigurationManager.AppSettings["OAuthToken"]
                }
            });

            var tweetTask = context.TweetAsync(msg);
            tweetTask.Wait();
            if (!tweetTask.IsFaulted)
            {
                var status = tweetTask.Result;
                if (status != null)
                {
                    context.DeleteTweetAsync(status.StatusID).Wait();
                }
            }
        }
    }
}

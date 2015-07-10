using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterCommandAndControl
{
    public class Tracker
    {
        private Task streamTask = null;
        private StreamContent stream = null;
        private TextWriter log = null;
        private Action<string> statusHandler = null;
        private Tracker() { }
        private Tracker(Action<string> statusHandler, TextWriter log) { this.statusHandler = statusHandler; this.log = log; }
        public bool IsActive { get { return stream != null; } }
        public static Tracker New(Action<string> statusHandler, string track = null, TextWriter log = null)
        {
            var t = new Tracker(statusHandler, log);
            t.Start(track);
            return t;
        }
        private void Start(string track = null)
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

            if (log != null)
                context.Log = log;

            track = track ?? ConfigurationManager.AppSettings["Track"];
            var trackList = !string.IsNullOrEmpty(track) ? track.ToLower().Split(',').OrderByDescending(x => x.Length).ToList() : new List<string>();

            try
            {
                streamTask = context.Streaming
                    .Where(s => s.Type == LinqToTwitter.StreamingType.Filter && s.Track == string.Join(",", trackList.Distinct()))
                    .Select(strm => strm)
                    .StartAsync(async strm =>
                    {
                        await Task.Run(() =>
                        {
                            try
                            {
                                stream = strm;
                                if (strm != null)
                                {
                                    if (!string.IsNullOrEmpty(strm.Content))
                                    {
                                        var status = new LinqToTwitter.Status(LitJson.JsonMapper.ToObject(strm.Content));
                                        if (status != null && status.StatusID > 0)
                                        {
                                            string statusText = status.Text.ToLower();
                                            if (trackList.Any(x => statusText.Contains(x)))
                                            {
                                                statusHandler(status.Text);
                                                if (log != null)
                                                    log.WriteLine("{0}: Status Handled: @{1} said [{2}]", DateTime.Now, status.User.ScreenName, status.Text);
                                            }
                                        }
                                        else if (log != null)
                                            log.WriteLine("{0}: Unhandled Item in Stream: {1}", DateTime.Now, strm.Content);
                                    }
                                    else if (log != null)
                                        log.WriteLine("{0}: Twitter Keep Alive", DateTime.Now);
                                }
                                else
                                    throw new ArgumentNullException("strm", "This value should never be null!");
                            }
                            catch (Exception ex)
                            {
                                if (log != null)
                                    log.WriteLine("{0}: Error (TrackerStream): {1}", DateTime.Now, ex.ToString());
                            }
                        });
                    });

            }
            catch (Exception ex)
            {
                if (log != null)
                    log.WriteLine("{0}: Error: {1}", DateTime.Now, ex.ToString());
            }
        }
        public void Wait()
        {
            if (streamTask != null)
                streamTask.Wait();
        }
    }
}

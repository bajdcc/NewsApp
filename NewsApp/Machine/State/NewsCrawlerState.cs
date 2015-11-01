using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace NewsApp.Machine.State
{
    class NewsCrawlerState : BaseState
    {
        private IState prev;
        private Util.StaticTimer _timer;
        private List<RssModel> rss_feed_list;
        private int index = 0;

        public NewsCrawlerState(MachineContext context, IState prev) : base(context)
        {
            this.prev = prev;
            this._timer = new Util.StaticTimer(TimeSpan.FromMinutes(3));
            InitRssFeed();
        }

        /// <summary>
        /// From Baidu News Rss<see cref="http://www.baidu.com/search/rss.html"/>
        /// </summary>
        private void InitRssFeed()
        {
            var rd = App.Current.Resources["RssFeed"] as ResourceDictionary;
            var md = rd.MergedDictionaries[0];
            var models = md["Rss"] as RssModel[];
            rss_feed_list = models.ToList();
        }

        public override void OnReset()
        {
            _timer.SetMinValue();
        }

        public override void OnStart()
        {
            base.Start = true;
            Trace("[Crawler] Started");
        }

        public override void OnMessage(NewsMessage msg)
        {
            if (prev != null)
            {
                prev.OnMessage(msg);
            }
        }

        public override void OnTimer()
        {
            if (base.Start && this._timer.IsTimeoutOnce())
            {
                this._timer.Restart();
                if (!base.Context.HasMessage())
                {
                    Trace("[Crawler] Working...");
                    OnCrawl();
                }
            }
        }

        private void OnCrawl()
        {
            if (prev == null)
                return;
            Trace("[Crawler] Fetching RSS Feed...");
            var model = rss_feed_list[index];
            Trace("[Crawler] Rss: " + model.Description);
            try
            {
                var reader = new Util.MultiDateFormatXmlReader(model.RssUrl);
                var feed = SyndicationFeed.Load(reader);
                reader.Close();
                foreach (var item in feed.Items)
                {
                    prev.OnMessage(new NewsMessage()
                    {
                        Origin = model.Description,
                        Content = item.Title.Text,
                        Uri = item.Links.FirstOrDefault().Uri,
                        Time = item.PublishDate.DateTime
                    });
                }
            }
            catch (Exception ex)
            {
                Trace("[Crawler] Error: " + ex.Message);
            }            
            index++;
        }
    }
}

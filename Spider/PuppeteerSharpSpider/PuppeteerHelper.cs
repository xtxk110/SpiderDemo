using System;
using System.Collections.Generic;
using System.Text;
using PuppeteerSharp;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using SpiderModel.Video;
using System.Linq;
using Newtonsoft.Json;

namespace PuppeteerSharpSpider
{
    public class PuppeteerHelper
    {
        public PuppeteerHelper()
        {
        }
        /// <summary>
        /// 获取页面内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetPageContentAsync(string url)
        {
            string result = string.Empty;
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            string i = Puppeteer.GetExecutablePath();
            using (Browser browser= await Puppeteer.LaunchAsync(new LaunchOptions {Headless=true,IgnoreHTTPSErrors=true }))
            {
                using Page page = await browser.NewPageAsync();
                await page.GoToAsync(url);
                result = await page.GetContentAsync();
            }
            return result;
        }
        /// <summary>
        /// AngleSharp解析网页
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public List<VideoInfo> V360KanJX(string content)
        {
            string prefix = "https://www.360kan.com";
            List<VideoInfo> videos = new List<VideoInfo>();
            string paystr = string.Empty;
            var context= BrowsingContext.New(Configuration.Default);
            IHtmlParser parser = context.GetService<IHtmlParser>();
            //HtmlParser parser=new HtmlParser()
            IHtmlDocument doc = parser.ParseDocument(content);
            IEnumerable<IElement> elements = doc.All.Where(m => m.LocalName == "li" && m.ClassName == "item");
            foreach(IElement item in elements)
            {
                paystr = item.QuerySelector("span.pay")?.Text();
                VideoInfo video = new VideoInfo
                {
                    Name = item.GetElementsByClassName("s1")[0].Text(),
                    Cover = item.GetElementsByTagName("img")[0].GetAttribute("src"),
                    Year = item.GetElementsByClassName("hint")[0].Text(),
                    Description = item.QuerySelector("p.star").Text(),
                    IsPay = string.IsNullOrWhiteSpace(paystr) || !paystr.Contains("付费") ? false : true,
                    Type = 1,
                    ParentUrl = prefix + item.QuerySelector("a.js-tongjic").GetAttribute("href")
                };
                videos.Add(video);
            }
            return videos;
        }
        /// <summary>
        /// 返回json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}

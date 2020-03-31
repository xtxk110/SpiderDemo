using System;
using System.Collections.Generic;
using System.Text;
using JasonSoft.Net.JsHttpClient.Http;
using Microsoft.Extensions.DependencyInjection;
using JasonSoft.Net.JsHttpClient.Extensions;
using HtmlAgilityPack;
using SpiderModel.Video;
using Newtonsoft.Json;

namespace JsHttpClientSpider
{
    /// <summary>
    /// 可以适当抓取简单JS动态内容
    /// </summary>
    public class JsHttpHelper
    {
        ServiceCollection sc;//服务集合
        ServiceProvider sp;//服务提供程序
        public JsHttpHelper()
        {
            sc = new ServiceCollection();
            sc.AddJsHttpClient();
            sp = sc.BuildServiceProvider();
        }
        /// <summary>
        /// IJsHttp/抓取内容
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <returns></returns>
        public string GetPageContent(string url)
        {
            string result = string.Empty;
            IJsHttpClient client = sp.GetService<IJsHttpClient>();
            if (null != client&&!string.IsNullOrWhiteSpace(url))
            {
                JsHttpRequest jsreq = new JsHttpRequest();
                jsreq.Uri = url;
                JsHttpResponse jsrep = client.Send(jsreq);
                result = jsrep.Html;
            }

            return result;
        }
        /// <summary>
        /// xpath解析 
        /// HtmlAgilityPack(可以直接使用此库解析非JS动态内容)
        /// </summary>
        /// <param name="content">获取的HTML内容</param>
        /// <returns></returns>
        public List<VideoInfo> V360KanJX(string content)
        {
            string prefix = "https://www.360kan.com";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//li[@class='item']");
            List<VideoInfo> videos = new List<VideoInfo>();
            string paystr = string.Empty;
            foreach (HtmlNode item in nodes)
            {
                paystr = item.SelectSingleNode("//span[@class='pay']").InnerText;
                VideoInfo video = new VideoInfo
                {
                    Name = item.SelectSingleNode("//span[@class='s1']").InnerText,
                    Cover = item.SelectSingleNode("//img").Attributes["src"].Value,
                    Year = item.SelectSingleNode("//span[@class='hint']").InnerText,
                    Description = item.SelectSingleNode("//p[@class='star']").InnerText,
                    IsPay = string.IsNullOrWhiteSpace(paystr) || !paystr.Contains("付费") ? false : true,
                    Type = 1,
                    ParentUrl = prefix+item.SelectSingleNode("//a[@class='js-tongjic']").GetAttributeValue("href", "")
                };
                videos.Add(video);
            };
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

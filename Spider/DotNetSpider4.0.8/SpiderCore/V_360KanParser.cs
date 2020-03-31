using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DotnetSpider;
using DotnetSpider.DataFlow;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.Selector;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Downloader;
using JsHttpClientSpider;
using SpiderModel.Video;

namespace Spider4_0_8.SpiderCore
{
    /// <summary>
    /// 360影视页面解析
    /// </summary>
    public class V_360KanParser : DataParserBase
    {
        SpiderStart _start;
        public V_360KanParser(SpiderStart start)
        {
            this._start = start;
            Required = DataParserHelper.CheckIfRequiredByRegex("/list");
            FollowRequestQuerier =BuildFollowRequestQuerier(DataParserHelper.QueryFollowRequestsByXPath("//div[@class='ew-page']/a[last()]"));
        }
        protected override Task<DataFlowResult> Parse(DataFlowContext context)
        {
            ISelectable selectable = context.Selectable;
            string next = selectable.XPath("//div[@class='ew-page']/a[last()]").GetValue().TrimEnd("&gt;".ToCharArray());
            // 解析数据
            List<string> data = selectable.XPath("//li[@class='item']").GetValues();
            
            if (null != data && data.Count > 0)
            {
                List<VideoInfo> videos = new List<VideoInfo>();
                Selectable st1 = null;
                List<Request> reqs = new List<Request>();
                foreach (string item in data)
                {
                    st1 = new Selectable(item);
                    //临时视频地址
                    string url = st1.XPath("//a/@href").GetValue();
                    VideoInfo video = new VideoInfo
                    {
                        Name = st1.XPath("//span[@class='s1']").GetValue(),
                        Cover = st1.XPath("//img/@src").GetValue(),
                        Year = st1.XPath("//span[@class='hint']").GetValue(),
                        Description = st1.XPath("//p[@class='star']").GetValue(),
                        IsPay = string.IsNullOrWhiteSpace(st1.XPath("//span[@class='pay']").GetValue()) || !st1.XPath("//span[@class='pay']").GetValue().Contains("付费") ? false : true,
                        Type = 1,
                        ParentUrl = url
                    };
                    
                    videos.Add(video);
                    if (!string.IsNullOrWhiteSpace(video.ParentUrl))
                    {
                        string tm = new JsHttpHelper().GetPageContent(video.ParentUrl);
                        if (!string.IsNullOrWhiteSpace(tm))
                        {
                            Selectable stt = new Selectable(tm);
                            var urls = stt.XPath("//div[@class='top-list-zd g-clear']//a[@data-daochu]").GetValues(ValueOption.OuterHtml);
                            foreach (var i in urls)
                            {
                                stt = new Selectable(i);
                                string u = stt.XPath("//a/@href").GetValue();
                                if (!string.IsNullOrWhiteSpace(u))
                                {
                                    string n = stt.XPath("//a").GetValue();
                                    video.Details.Add(new VideoDetail { PlayUrl = u, Number = "1", IsPay = video.IsPay, PlayName = n });
                                }
                            }
                            stt = null;
                        }
                        Request req = CreateFromRequest(context.Response.Request, url);
                        req.DownloaderType = DownloaderType.WebDriver;
                        reqs.Add(req);
                    }
                    //videos.AsParallel().ForAll(m =>
                    //{
                    //    if (!string.IsNullOrWhiteSpace(m.ParentUrl))
                    //    {
                    //        string tm = new JsHttpHelper().GetPageContent(m.ParentUrl);
                    //        if (!string.IsNullOrWhiteSpace(tm))
                    //        {
                    //            Selectable stt = new Selectable(tm);
                    //            var urls = stt.XPath("//div[@class='top-list-zd g-clear']//a['@data-daochu']").GetValues(ValueOption.OuterHtml);
                    //            foreach (var i in urls)
                    //            {
                    //                stt = new Selectable(i);
                    //                string u = stt.XPath("//a/@href").GetValue();
                    //                if (!string.IsNullOrWhiteSpace(u))
                    //                {
                    //                    string n = stt.XPath("//a").GetValue();
                    //                    video.Details.Add(new VideoDetail { PlayUrl = u, Number = "1", IsPay = video.IsPay, PlayName = n });
                    //                }
                    //            }
                    //            stt = null;
                    //        }
                    //        //reqs.Add(CreateFromRequest(context.Response.Request, url));
                    //    }
                    //});

                }
                st1 = null;
                context.AddData("v", videos);
                //if (reqs.Count > 0)
                //{
                //    context.AddExtraRequests(reqs.ToArray());
                //}
            }
            //如果解析为空，跳过后续步骤(存储 etc)
            if (data == null || data.Count == 0)
            {
                context.ClearData();
                return Task.FromResult(DataFlowResult.Terminated);
            }
            if (next != "下一页")
                FollowRequestQuerier = null;
            return Task.FromResult(DataFlowResult.Success);
        }
    }
    /// <summary>
    /// 360电影详情
    /// </summary>
    public class V_360KanDetailParser : DataParserBase
    {
        public V_360KanDetailParser()
        {
            Required = DataParserHelper.CheckIfRequiredByRegex("/m/");
        }
        protected override Task<DataFlowResult> Parse(DataFlowContext context)
        {
            ISelectable selectable = context.Selectable;
            //context.AddData("v", selectable.GetValue());
            List<string> details = selectable.XPath("//div[@class='top-list-zd g-clear']").GetValues();
            ISelectable s1 = null;
            foreach (var item in details)
            {
                s1 = new Selectable(item);
                string url = s1.XPath("//a/@href").GetValue();
                string name = s1.XPath("//a").GetValue();
            }

            return Task.FromResult(DataFlowResult.Success);
        }
    }
}

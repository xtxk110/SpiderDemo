using System;
using Microsoft.Extensions.DependencyInjection;
using DotnetSpider;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using DotnetSpider.DataFlow.Parser;
using DotnetSpider.DataFlow;
using System.Threading.Tasks;
using DotnetSpider.DataFlow.Storage;
using DotnetSpider.Downloader;
using Spider4_0_8.SpiderCore;
using JsHttpClientSpider;

namespace Spider4_0_8
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("PRINT_DOTNET_SPIDER_INFO", true);
            string url1 = "https://www.360kan.com/dianying/list.php?cat=all&year=all&area=all&act=all&rank=createtime";
            string url2 = "https://www.360kan.com/m/gqfnahH5Rnr5Th.html";
            SpiderStart start = new SpiderStart();
            start.Start(url1, new List<IDataFlow> { new V_360KanParser(start)});

            //string result = new JsHttpHelper().GetPageContent("https://www.360kan.com/m/gqfnahH5Rnr5Th.html");
            //Console.WriteLine(result);

            Console.ReadKey();
        }
        
    }
}

using System;

namespace PuppeteerSharpSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            string url1 = "https://www.360kan.com/dianying/list.php?cat=all&year=all&area=all&act=all&rank=createtime";
            PuppeteerHelper helper = new PuppeteerHelper();
            var result =  helper.GetPageContentAsync(url1);
            
            Console.WriteLine(helper.GetJson(helper.V360KanJX(result.Result)));
            Console.ReadKey();
        }
    }
}

using System;

namespace JsHttpClientSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            string url1 = "https://www.360kan.com/dianying/list.php?cat=all&year=all&area=all&act=all&rank=createtime";
            JsHttpHelper helper = new JsHttpHelper();
            string result = helper.GetPageContent(url1);
            var c = helper.V360KanJX(result);
            Console.WriteLine(helper.GetJson(c));
            Console.ReadKey();
        }
    }
}

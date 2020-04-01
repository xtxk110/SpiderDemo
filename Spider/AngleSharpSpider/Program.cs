using System;

namespace AngleSharpSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            string url1 = "https://www.360kan.com/dianying/list.php?cat=all&year=all&area=all&act=all&rank=createtime";
            AngleSharpHelper helper = new AngleSharpHelper();
            var task = helper.GetPageContent(url1);
            task.Wait();
            Console.WriteLine(task.Result);

            Console.Read();
        }
    }
}

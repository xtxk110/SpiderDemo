using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;

namespace AngleSharpSpider
{
    public class AngleSharpHelper
    {
        public async Task<string> GetPageContent(string url)
        {
            string result = string.Empty;
            IBrowsingContext context = BrowsingContext.New(Configuration.Default.WithDefaultLoader().WithJs());
            IDocument doc=await context.OpenAsync(url);
            result = doc.DocumentElement.OuterHtml;
            return result;
        } 
    }
}

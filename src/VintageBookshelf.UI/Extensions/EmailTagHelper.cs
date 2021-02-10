using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VintageBookshelf.UI.Extensions
{
    public class EmailTagHelper : TagHelper
    {
        public string Domain { get; set; } = "vintagebookshelf.com";
        
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await output.GetChildContentAsync();
            var target = $"{content.GetContent()}@{Domain}";
            output.Attributes.SetAttribute("href","mailto:" + target);
            output.Content.SetContent(target);
        }
    }
}
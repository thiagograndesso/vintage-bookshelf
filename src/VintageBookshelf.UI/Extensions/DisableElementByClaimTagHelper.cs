using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VintageBookshelf.UI.Extensions
{
    [HtmlTargetElement("a", Attributes = "disable-by-claim-name")]
    [HtmlTargetElement("a", Attributes = "disable-by-claim-value")]
    public class DisableElementByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public DisableElementByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        
        [HtmlAttributeName("disable-by-claim-name")]
        public string ClaimName { get; set; }
        
        [HtmlAttributeName("disable-by-claim-value")]
        public string ClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (output is null) throw new ArgumentNullException(nameof(output));

            var hasAccess = CustomAuthorization.ValidateUserClaims(_contextAccessor.HttpContext, ClaimName, ClaimValue);
            if (hasAccess)
            {
                return;
            }

            output.Attributes.RemoveAll("href");
            output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed"));
            output.Attributes.Add(new TagHelperAttribute("title", "Permission not allowed"));
        }
    }
}
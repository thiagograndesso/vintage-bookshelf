using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace VintageBookshelf.UI.Extensions
{
    [HtmlTargetElement("*", Attributes = "suppress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "suppress-by-claim-value")]
    public class HideElementByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HideElementByClaimTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        
        [HtmlAttributeName("suppress-by-claim-name")]
        public string ClaimName { get; set; }
        
        [HtmlAttributeName("suppress-by-claim-value")]
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
            
            output.SuppressOutput();
        }
    }
}
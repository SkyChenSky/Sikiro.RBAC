using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{

    [HtmlTargetElement("LayuiButton")]
    public class LayuiButtonTag : TagHelper
    {
        #region 初始化
        private const string PermCodeAttributeName = "PermCode";
        private const string ClasstAttributeName = "class";
        private const string LayEventAttributeName = "lay-event";
        private const string LaySubmitAttributeName = "LaySubmit";
        private const string LayIdAttributeName = "id";
        private const string StyleAttributeName = "style";

        [HtmlAttributeName(StyleAttributeName)]
        public string Style { get; set; }

        [HtmlAttributeName(LayIdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(LaySubmitAttributeName)]
        public string LaySubmit { get; set; }

        [HtmlAttributeName(LayEventAttributeName)]
        public string LayEvent { get; set; }

        [HtmlAttributeName(ClasstAttributeName)]
        public string Class { get; set; }

        [HtmlAttributeName(PermCodeAttributeName)]
        public int PermCode { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiButtonTag(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        #endregion
        public override async void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            var administrator = ViewContext.HttpContext.GetCurrentUser();
            if (administrator == null)
                return;

            var childContent = await output.GetChildContentAsync();

            if (((List<int>)ViewContext.ViewData["PermCodes"]).Contains(PermCode) || administrator.IsSuper)
            {
                foreach (var item in context.AllAttributes)
                {
                    output.Attributes.Add(item.Name, item.Value);
                }

                output.TagName = "a";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(childContent.GetContent());
            }
            else
            {
                output.TagName = "";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent("");
            }
        }
    }
}

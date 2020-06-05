using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{
    [HtmlTargetElement("layuiForm")]
    public class LayuiFormTag : TagHelper
    {
        #region 初始化
        private const string IdAttributeName = "id";
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiFormTag(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;

            var htmlAttributes = new Dictionary<string, object>
            {
                { "class", "layui-form layuiadmin-card-header-auto layui-form-pane" },
                { "id",Id},
                { "lay-filter",Id}
            };

            var tagBuilder = _generator.GenerateForm(ViewContext, Action, Controller, null, "post", htmlAttributes);

            output.MergeAttributes(tagBuilder);
            output.PostContent.SetHtmlContent(
                "<div class='layui-form-item layui-hide'><input type='button' lay-submit lay-filter='btn-submit' id='btn-submit' value='确认'> </div>");

            output.PostElement.SetHtmlContent($@"<script>
                $(function () {{
                layui.form.on('submit(btn-submit)', function () {{
                    $('#{Id}').formPost(function () {{
                        var index = parent.layer.getFrameIndex(window.name);
                        parent.layer.close(index);
                    }});
                }});
            }});
    </script>");
        }
    }
}

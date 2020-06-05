using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{
    /// <summary>
    /// 下拉框
    /// </summary>
    [HtmlTargetElement("layuiCheckBox", TagStructure = TagStructure.WithoutEndTag)]
    public class LayuiCheckBoxTag : TagHelper
    {
        #region 初始化
        private const string ForAttributeName = "asp-for";
        private const string TextAttributeName = "text";
        private const string DefaultTextAttributeName = "defaultText";
        private const string DispalyAttributeName = "input-display";
        private const string UrlAttributeName = "url";
        private const string DataAttributeName = "data";

        [HtmlAttributeName(DispalyAttributeName)]
        public ELayuiShow InputDisplay { get; set; }

        [HtmlAttributeName(UrlAttributeName)]
        public string Url { get; set; }

        [HtmlAttributeName(TextAttributeName)]
        public string Text { get; set; }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(DataAttributeName)]
        public object Data { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiCheckBoxTag(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            var selectListItems = new List<SelectListItem>();
            var kvList = For?.Metadata.EnumGroupedDisplayNamesAndValues;
            if (kvList != null)
                selectListItems = For.Metadata.EnumGroupedDisplayNamesAndValues.Select(a => new SelectListItem { Text = a.Key.Name.ToString(), Value = a.Value.ToString(), Selected = a.Value == ((int)((For.Model) ?? -1)).ToString() }).ToList();

            if (kvList == null && Data != null)
                selectListItems = (List<SelectListItem>)Data;

            var idStr = NameAndIdProvider.CreateSanitizedId(ViewContext, For.Name, _generator.IdAttributeDotReplacement);

            var tagBuilder = new StringBuilder(32);
            selectListItems.ForEach(item =>
            {
                var attributes = new Dictionary<string, object>
                {
                    {"title", item.Text},
                    { "lay-skin","primary" }
                };
                var inputTagBuilder = _generator.GenerateCheckBox(ViewContext,
                   For.ModelExplorer,
                   For.Name,
                   item.Value == (string)For.ModelExplorer.Model,
                   attributes);
                tagBuilder.Append(inputTagBuilder.GetString());
            });

            var outpuHtml = $@"
              <label class='layui-form-label'>{Text ?? For.Metadata.DisplayName}</label>
            <div id='{idStr}_checkGroup' class='layui-input-block'>
              {tagBuilder}
            </div>";

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", InputDisplay.GetDisplayName());
            output.Attributes.Add("id", $"{idStr}_div");
            output.Content.SetHtmlContent(outpuHtml);
            if (kvList == null && !Url.IsNullOrEmpty())
            {
                output.PostElement.SetHtmlContent($@"<script type='text/javascript'>
                 $(function () {{
                    $('#{idStr}_checkGroup').bindCheckBoxData('{Url}','{For.Model}','{For.Name}');
                    }});
                </script>");
            }

        }
    }
}

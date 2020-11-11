using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{
    /// <summary>
    /// 下拉框
    /// </summary>
    [HtmlTargetElement("layuiMultiSelect", TagStructure = TagStructure.WithoutEndTag)]
    public class LayuiMultiSelectTag : TagHelper
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

        [HtmlAttributeName(DefaultTextAttributeName)]
        public string DefaultText { get; set; }

        [HtmlAttributeName(DataAttributeName)]
        public object Data { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiMultiSelectTag(IHtmlGenerator generator)
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
                selectListItems = For.Metadata.EnumGroupedDisplayNamesAndValues.Select(a => new SelectListItem
                {
                    Text = a.Key.Name.ToString(),
                    Value = a.Value.ToString()
                }).ToList();

            if (kvList == null && Data != null)
                selectListItems = ((IEnumerable<SelectListItem>)Data).ToList();

            if (!DefaultText.IsNullOrWhiteSpace())
                selectListItems.Insert(0, new SelectListItem { Text = DefaultText, Value = "" });

            var idStr = TagBuilder.CreateSanitizedId(For.Name, _generator.IdAttributeDotReplacement);
            var attributes = new Dictionary<string, object>
            {
                {"xm-select",idStr},
                { "xm-select-skin","default"}
            };

            IList l = new ArrayList();
            if (For.Model != null)
                l = (IList)For.Model;

            selectListItems.ForEach(item =>
                {
                    item.Selected = For.Model != null && l.Contains(item.Value);
                });

            var tagBuilder = _generator.GenerateSelect(ViewContext, For.ModelExplorer, null, For.Name, selectListItems, null, false, attributes);

            var outpuHtml = $@"
              <label class='layui-form-label'>{Text ?? For.Metadata.DisplayName}</label>
            <div class='layui-input-block'>
              {tagBuilder.GetString().Replace("&amp;nbsp;", "&nbsp;")}
            </div>";

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("id", $"{idStr}_div");
            output.Attributes.Add("class", InputDisplay.GetDisplayName());
            output.Content.SetHtmlContent(outpuHtml);

            var jsValue = l.Cast<object>().Aggregate("", (current, t) => current + (t + ",")).TrimEnd(',');

            if (kvList == null && !Url.IsNullOrEmpty())
            {
                output.PostElement.SetHtmlContent($@"<script type='text/javascript'>
                 $(function () {{
                    $('#{idStr}').bindMultiSelectData('{Url}','{jsValue}','{idStr}');
                    }});
                </script>");
            }

        }
    }
}

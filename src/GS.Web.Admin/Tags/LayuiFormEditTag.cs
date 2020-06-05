using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{
    [HtmlTargetElement("layuiFormEdit", TagStructure = TagStructure.WithoutEndTag)]
    public class LayuiFormEditTag : TagHelper
    {
        #region 初始化
        private const string ValueAttributeName = "value";
        private const string PlaceholderAttributeName = "placeholder";
        private const string TextAttributeName = "text";
        private const string FormatAttributeName = "asp-format";
        private const string DisabledAttributeName = "disabled";
        private const string ForAttributeName = "asp-for";
        private const string TypeAttributeName = "type";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        private string _type;
        [HtmlAttributeName(TypeAttributeName)]
        public string Type
        {
            set => _type = value;
            get => _type.IsNullOrWhiteSpace() ? "text" : _type;
        }

        [HtmlAttributeName(ValueAttributeName)]
        public object Value { get; set; }

        [HtmlAttributeName(PlaceholderAttributeName)]
        public string Placeholder { get; set; }

        [HtmlAttributeName(TextAttributeName)]
        public string Text { get; set; }

        [HtmlAttributeName(FormatAttributeName)]
        public string Format { get; set; }

        [HtmlAttributeName(DisabledAttributeName)]
        public bool Disabled { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiFormEditTag(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            var modelExplorer = For.ModelExplorer;

            var attributes = new Dictionary<string, object>
            {
                {PlaceholderAttributeName, Placeholder},
                {TextAttributeName, Text??modelExplorer.Metadata.Description??modelExplorer.Metadata.DisplayName},
                {"class", "layui-input"},
                {"Type",Type}
            };

            var idStr = NameAndIdProvider.CreateSanitizedId(ViewContext, For.Name, _generator.IdAttributeDotReplacement);

            if (modelExplorer.Metadata.IsRequired)
                attributes["lay-verify"] = "required";
            if (Disabled)
                attributes["Disabled"] = "Disabled";

            var value = modelExplorer.Model ?? Value;
            var inputTagBuilder = _generator.GenerateTextBox(ViewContext, For.ModelExplorer, For.Name, value, Format, attributes);

            var htmlText = !attributes[TextAttributeName].ToStr().IsNullOrWhiteSpace() ? $"<label class='layui-form-label'>{attributes[TextAttributeName]}</label>" : "";
            var outpuHtml = $@"
        {htmlText}
        <div class='layui-input-block'>
            {inputTagBuilder.GetString()}
        </div>";

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("id", $"{idStr}_div");
            output.Attributes.Add("class", "layui-form-item");
            output.Content.SetHtmlContent(outpuHtml);
        }
    }
}

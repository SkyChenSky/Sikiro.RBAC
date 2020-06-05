using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{
    /// <inheritdoc />
    /// <summary>
    /// 输入文本框
    /// </summary>
    [HtmlTargetElement("layuiImg", TagStructure = TagStructure.WithoutEndTag)]
    public class LayuiImgTag : TagHelper
    {
        #region 初始化
        private const string ValueAttributeName = "value";
        private const string PlaceholderAttributeName = "placeholder";
        private const string TextAttributeName = "text";
        private const string FormatAttributeName = "asp-format";
        private const string DisabledAttributeName = "disabled";
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

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
        public LayuiImgTag(IHtmlGenerator generator)
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
                {"class", "layui-input"}
            };

            if (Disabled)
                attributes["Disabled"] = "Disabled";

            var value = modelExplorer.Model ?? Value;

            var htmlText = !attributes[TextAttributeName].ToStr().IsNullOrWhiteSpace() ? $"<label class='layui-form-label'>{attributes[TextAttributeName]}</label>" : "";
            var outpuHtml = $@"
                    {htmlText}
                    <div class='layui-input-inline'>
                        <button id='btn_{For.Name}_up' type='button' class='layui-btn' >上传图片</button>
                <img class='layui-upload-img' id='{For.Name}_imgs1' src='{value}' style='max-width:200px' />
                <p id='{For.Name}_retry'></p>
                <input  id='{For.Name}_imgs2' type = 'hidden' value='{value}' name='{For.Name}'>
                <div>请上传100px*100px的图片</div>
                    </div>";

            var idStr = NameAndIdProvider.CreateSanitizedId(ViewContext, For.Name, _generator.IdAttributeDotReplacement);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "layui-form-item");
            output.Attributes.Add("id", $"{idStr}_div");
            output.Content.SetHtmlContent(outpuHtml);

            output.PostElement.SetHtmlContent($@" <script type='text/javascript'>
$(function () {{
                var imageUrl;
            var $ = layui.jquery;
            var upload = layui.upload;

            var uploadInst = upload.render({{
                elem: '#btn_{For.Name}_up',
                url: '/Upload/Img',
                before: function (obj) {{
                    obj.preview(function (index, file, result) {{
                        if (file.size / 1024 / 1024 > 2) {{
                            layer.msg('上传的图片不能超过1M',
                            {{
                                icon: 2,
                                time: 2000
                            }});
                            return false;
                        }}
                        $('#{For.Name}_imgs1').attr('src', result);
                    }});
                }},
                done: function (res) {{
                    if (res.success) {{
                        if (res.data) {{
                            imageUrl = res.data;
                                $('#{For.Name}_imgs2').val(imageUrl);
                        }}
                    }} else {{
                        var demoText = $('#{For.Name}_retry');
                        demoText.html(""<span style='color: #FF5722;'>上传失败</span> <a class='layui-btn layui-btn-xs demo-reload'>重试</a>"");
                        demoText.find('.demo-reload').on('click', function () {{
                            uploadInst.upload();
                        }});
                    }}
                }}
            }});
            }});
            </script>");
        }
    }
}

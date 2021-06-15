using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Tags
{
    [HtmlTargetElement("layuiTableFreshButton", TagStructure = TagStructure.WithoutEndTag)]
    public class LayuiTableFreshButtonTag : TagHelper
    {
        #region 初始化
        private const string IdAttributeName = "id";
        private const string TargetFormIdName = "targetFormId";

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(TargetFormIdName)]
        public string TargetFormId { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiTableFreshButtonTag(IHtmlGenerator generator)
        {
            _generator = generator;
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            var buttonHtml = string.Format(@"<button id='{0}' lay-filter='{0}' type='button' class='layui-btn layuiadmin-btn-admin' lay-submit>
                        <i class='layui-icon layui-icon-search layuiadmin-button-btn'></i>
               </button>", Id);

            var scriptHtml = $@"<script>
           var form = layui.form;
           var table = layui.table;
           form.on('submit({Id})', function(data) {{
               var field = data.field;

               //执行重载
               table.reload('{TargetFormId}', {{
                   where: field,
                   page: {{
                          curr: $('.layui-laypage-em').next().html() 
                     }}
               }});
           }});
           </script> ";
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "layui-inline");
            output.Content.SetHtmlContent(buttonHtml);
            output.PostElement.SetHtmlContent(scriptHtml);
        }
    }
}

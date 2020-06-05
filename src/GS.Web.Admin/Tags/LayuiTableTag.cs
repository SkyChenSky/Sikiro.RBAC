using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Extension;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Tags
{
    /// <inheritdoc />
    /// <summary>
    /// 表单控件
    /// </summary>
    [HtmlTargetElement("layuiTable", TagStructure = TagStructure.WithoutEndTag)]
    public class LayuiTableTag : TagHelper
    {
        #region 初始化
        private const string IdAttributeName = "id";
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string PageAttributeName = "page";
        private const string LimitAttributeName = "limit";
        private const string ToolBarIdAttributeName = "toolbar";
        private const string ToolBarWeightAttributeName = "toolbarweight";
        private const string MultipleAttributeName = "multiple";
        private const string ColsModelAttributeName = "colsModel";
        private const string HeightAttributeName = "height";

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(IdAttributeName)]
        public string Id { get; set; }

        [HtmlAttributeName(PageAttributeName)]
        public bool? Page { get; set; }

        [HtmlAttributeName(LimitAttributeName)]
        public int? Limit { get; set; }

        [HtmlAttributeName(ToolBarIdAttributeName)]
        public string ToolBarId { get; set; }

        [HtmlAttributeName(HeightAttributeName)]
        public int? Height { get; set; }

        private int _toolBarWeight = 180;

        [HtmlAttributeName(ToolBarWeightAttributeName)]
        public int ToolBarWeight
        {
            set => _toolBarWeight = value;
            get => _toolBarWeight;
        }

        [HtmlAttributeName(MultipleAttributeName)]
        public ETableSelectType Multiple { get; set; }

        [HtmlAttributeName(ColsModelAttributeName)]
        public Type ColsModel { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private readonly IHtmlGenerator _generator;
        public LayuiTableTag(IHtmlGenerator generator)
        {
            _generator = generator;
            if (!Page.HasValue)
                Page = true;

            if (!Limit.HasValue)
                Limit = 10;
        }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.ThrowIfNull();
            output.ThrowIfNull();

            output.TagName = "table";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add("id", Id);
            output.Attributes.Add("lay-filter", Id);

            var url = $"url:'/{Controller}/{Action}',";
            var page = $"page: {Page.ToString().ToLower()},";
            var limit = $"limit: {Limit},";
            var elem = $"elem: '#{Id}',";
            var id = $"id: '{Id}',";
            var multiple = "";

            switch (Multiple)
            {
                case ETableSelectType.Checkbox:
                    multiple = "{ type:'checkbox', fixed: true },";
                    break;
                case ETableSelectType.Radio:
                    multiple = "{ type:'radio', fixed: true },";
                    break;
            }
            var toolBar = ToolBarId.IsNullOrEmpty() ? "" : $"{{title: '操作', fixed: true, width: {ToolBarWeight}, align: 'center', toolbar: '#{ToolBarId}' }},";
            var style = "";
            var propertiyDic = ColsModel.GetProperties().ToDictionary(k => k.Name, v => v.GetCustomAttribute<TableColsAttribute>());

            var cols = new StringBuilder(64);
            foreach (var dic in propertiyDic)
            {
                if (dic.Value == null)
                    continue;

                var field = dic.Value.Field ?? dic.Key.ToCamelCase();
                var title = dic.Value.Tile;
                var width = dic.Value.Width;
                var align = dic.Value.Align.GetDisplayName();
                var isImage = dic.Value.IsImage;

                string html = "";
                if (isImage)
                {
                    style = @" 
    <style>
       .layui-table-body .layui-table-cell {
            height: 65px;
            line-height: 65px;
        }
    </style>";
                    html = $@",templet: ""<div><div class='div-img'><img src='{{{{d.{field}}}}}' layer-src='{{{{d.{field}}}}}' style='height:60px' /></div></div>""";
                }
                var widthHtml = width != 0 ? "width:" + width + "," : "";

                cols.AppendLine($"{{ field: '{field}', title: '{title}', {widthHtml} align: '{align}'{html} }},");
            }

            var heightHtml = Height.HasValue ? $"height: {Height}," : "";

            output.PreElement.AppendHtml(style);
            output.PostElement.SetHtmlContent(string.Format(@"<script type='text/javascript'>
        
                layui.use('table', function() {{
                var table = layui.table;
                table.render({{
                done: function (res, curr, count) {{
                layer.photos({{
                    photos: '.div-img'
                    , anim: 5
                }});
                if(res.error && res.data && res.data.status == 403){{
                    window.location.href = '/home/error?value=' + res.message;
                }}
                            }},
                    method:'post', 
                    {0}
                    {1}
                    {2}
                    {8}
                    cellMinWidth: 160,
                    cols: [[
                        {6}
                        {5}
                        {7}
                    ]],
                     {3}
                     {4}

                }});
            }});
            </script>", elem, id, url, page, limit, toolBar, multiple, cols, heightHtml));
        }
    }

    public enum ETableSelectType
    {
        None,
        Radio,
        Checkbox
    }
}

using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Tags
{
    public enum ELayuiShow
    {
        [Display(Name = "layui-inline")]
        LayuiInline = 0,
        [Display(Name = "layui-form-item")]
        LayuiFormItem = 1
    }
}

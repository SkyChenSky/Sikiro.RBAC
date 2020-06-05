using System.ComponentModel.DataAnnotations;
using Sikiro.Common.Utils;

namespace Sikiro.Web.Admin.Models.Department
{
    public class AddEditModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "请填写部门名称")]
        [StringLength(10, ErrorMessage = "岗位名称不能超过10字符")]
        [RegularExpression(RegularExpression.ZhongYing, ErrorMessage = "岗位名称只允许输入中文、英文字符")]
        public string Name { get; set; }

        public int Order { get; set; }

        public string ParentId { get; set; }
    }
}

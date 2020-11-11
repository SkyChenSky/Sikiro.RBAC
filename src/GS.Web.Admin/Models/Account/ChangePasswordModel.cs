using System.ComponentModel.DataAnnotations;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Account
{
    public class ChangePasswordModel
    {
        [Password(false, ""), Required(ErrorMessage = "请输入原密码", AllowEmptyStrings = true)]
        public string OldPassword { get; set; }

        [Password(false, ""), Required(ErrorMessage = "请输入新密码", AllowEmptyStrings = true)]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordByUserId
    {
        [Required(ErrorMessage = "请输入原密码", AllowEmptyStrings = true)]
        public string OldPassword { get; set; }

        [Password(true, "新密码:请输入6-18位字母与数字组合的密码"), Required(ErrorMessage = "请输入新密码", AllowEmptyStrings = true)]
        public string NewPassword { get; set; }
    }

}

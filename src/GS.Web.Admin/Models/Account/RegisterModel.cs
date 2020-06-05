using System.ComponentModel.DataAnnotations;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Account
{
    public class RegisterByUserNameModel
    {
        [Required(ErrorMessage = "请输入用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [Password(false,"")]
        public string Password { get; set; }
    }

    public class RegisterByMobileModel
    {
        [Required(ErrorMessage = "请输入手机号")]
        [Mobile]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [Password(false, "")]
        public string Password { get; set; }
    }
}

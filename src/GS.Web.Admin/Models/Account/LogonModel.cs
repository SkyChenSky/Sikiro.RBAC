using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.Account
{
    public class LogonModel
    {
        [Required(ErrorMessage = "请输入用户名或者手机号吗")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }
    }
}

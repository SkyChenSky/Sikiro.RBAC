using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.Administrator
{
    public class ResetPasswordModel
    {
        public string UserId { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "请填写重置密码")]
        public string Password { get; set; }
    }
}

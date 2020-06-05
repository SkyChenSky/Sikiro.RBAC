using System;
using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.Company
{
    public class AddorEditCompany
    {
        public string Id { get; set; }

        [Display(Name = "商户名称")]
        [Required(ErrorMessage = "请输入商户名称")]

        public string Name { get; set; }


        [Display(Name = "与客户聊天显示的名称")]
        [Required(ErrorMessage = "请输入与客户聊天显示的名称")]
        public string ChatName { get; set; }

        [Display(Name = "与客户聊天显示的头像")]
        [Required(ErrorMessage = "请输入与客户聊天显示的头像")]

        public string ChatLogo { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        public DateTime CreateDateTime
        {
            get; set;
        }


        public string CompanyNo { get; set; }
    }
}

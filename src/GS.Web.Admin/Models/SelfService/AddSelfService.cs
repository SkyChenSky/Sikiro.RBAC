using System;
using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.SelfService
{
    public class AddSelfService
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "请输入产品名称")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 产品 logo
        /// </summary>
        [Display(Name = "产品logo")]
        public string ServiceLogo { get; set; }

        /// <summary>
        /// 排序，值越大越排
        /// </summary>
        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入排序")]
        public int Order { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Display(Name = "创建日期")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 产品详情
        /// </summary>
        [Display(Name = "产品详情")]
        public string Detail { get; set; }
    }
}

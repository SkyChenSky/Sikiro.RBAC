using System;
using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.Shopping
{
    public class AddShopping
    {

        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "请输入产品名称")]
        /// <summary>

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 产品 logo
        /// </summary>
        [Display(Name = "产品logo")]
        public string ProductLogo { get; set; }

        [Display(Name = "属性1")]
        [Required(ErrorMessage = "请输入属性1")]
        /// <summary>
        /// 属性 1，例如：时效
        /// </summary>
        public string Item1 { get; set; }
        [Display(Name = "属性1的值")]
        [Required(ErrorMessage = "请输入属性1的值")]
        /// <summary>
        /// 属性 1 字段的值 
        /// </summary>
        public string Item1Val { get; set; }

        [Display(Name = "属性2")]
        [Required(ErrorMessage = "请输入属性2")]
        /// <summary>
        /// 属性 2，例如：收费标准 
        /// </summary>

        public string Item2 { get; set; }
        [Display(Name = "属性2的值")]
        [Required(ErrorMessage = "请输入属性2的值")]
        /// <summary>
        /// 属性 2 字段的值 
        /// </summary>

        public string Item2Val { get; set; }

        /// <summary>
        /// 排序，值越大越排
        /// </summary>


        [Display(Name = "排序")]
        [Required(ErrorMessage = "请输入排序")]
        public int Order { get; set; }


        public DateTime CreateDateTime { get; set; }

        [Display(Name = "点击跳转的网址")]
        [Required(ErrorMessage = "请输入点击跳转的网址")]
        /// <summary>
        /// 点击跳转的网址 
        /// </summary>
        public string GoUrl { get; set; }
    }
}

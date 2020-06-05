using System;
using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.ProductAging
{
    public class EditProduct
    {
        public string Id { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        [Required(ErrorMessage = "请输入产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品 logo
        /// </summary>
        [Display(Name = "产品logo")]
        public string ProductLogo { get; set; }

        /// <summary>
        /// 属性 1，例如：时效
        /// </summary>
        [Display(Name = "属性1")]
        [Required(ErrorMessage = "请输入属性1")]
        public string Item1 { get; set; }

        /// <summary>
        /// 属性 1 字段的值 
        /// </summary>
        [Display(Name = "属性1值")]
        [Required(ErrorMessage = "请输入属性1值")]
        public string Item1Val { get; set; }

        /// <summary>
        /// 属性 2，例如：收费标准 
        /// </summary>
        [Display(Name = "属性2")]
        [Required(ErrorMessage = "请输入属性2")]
        public string Item2 { get; set; }

        /// <summary>
        /// 属性 2 字段的值 
        /// </summary>
        [Display(Name = "属性2值")]
        [Required(ErrorMessage = "请输入属性2值")]
        public string Item2Val { get; set; }
        /// <summary>
        /// 产品详情
        /// </summary>

        public string Detail { get; set; }
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
    }
}

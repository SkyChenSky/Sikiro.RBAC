using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Shopping
{
    public class ShoppingModel
    {
        public string Id { get; set; }

        [TableCols(Tile = "产品名称")]
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        [TableCols(Tile = "产品 logo", IsImage = true)]
        /// <summary>
        /// 产品 logo
        /// </summary>
        public string ProductLogo { get; set; }
        [TableCols(Tile = "属性1")]
        /// <summary>
        /// 属性 1，例如：时效
        /// </summary>
        public string Item1 { get; set; }

        /// <summary>
        /// 属性 1 字段的值 
        /// </summary>
        public string Item1Val { get; set; }
        [TableCols(Tile = "属性2")]
        /// <summary>
        /// 属性 2，例如：收费标准 
        /// </summary>

        public string Item2 { get; set; }
   
        /// <summary>
        /// 属性 2 字段的值 
        /// </summary>

        public string Item2Val { get; set; }

        /// <summary>
        /// 排序，值越大越排
        /// </summary>


        [TableCols(Tile = "排序 ")]
        public int Order { get; set; }
        [TableCols(Tile = "创建日期 ")]
        /// <summary>
        /// 创建日期
        /// </summary>

        public DateTime CreateDateTime { get; set; }
        [TableCols(Tile = "点击跳转的网址")]
        /// <summary>
        /// 点击跳转的网址 
        /// </summary>
        public string GoUrl { get; set; }
    }
}

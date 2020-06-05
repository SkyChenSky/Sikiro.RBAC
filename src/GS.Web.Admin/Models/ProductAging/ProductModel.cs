using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.ProductAging
{
    public class ProductModel
    {

        public string Id { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        /// 
        [TableCols(Tile = "产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品 logo
        /// </summary>
        [TableCols(Tile = "产品 logo", IsImage=true)]
        public string ProductLogo { get; set; }

        /// <summary>
        /// 属性 1，例如：时效
        /// </summary>
        [TableCols(Tile = "属性1")]
        public string Item1 { get; set; }

        /// <summary>
        /// 属性 1 字段的值 
        /// </summary>
        //[TableCols(Tile = "属性1的值")]
        public string Item1Val { get; set; }

        /// <summary>
        /// 属性 2，例如：收费标准 
        /// </summary>
        [TableCols(Tile = "属性2")]
        public string Item2 { get; set; }

        /// <summary>
        /// 属性 2 字段的值 
        /// </summary>
        //[TableCols(Tile = "属性2的值")]
        public string Item2Val { get; set; }
        /// <summary>
        /// 产品详情
        /// </summary>
        //[TableCols(Tile = "产品详情")]
        //public string Detail { get; set; }
        /// <summary>
        /// 排序，值越大越排
        /// </summary>


        [TableCols(Tile = "排序")]
        public int Order { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>

        [TableCols(Tile = "创建日期")]
        public DateTime CreateDateTime { get; set; }
    }
}

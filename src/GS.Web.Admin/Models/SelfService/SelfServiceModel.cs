using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.SelfService
{
    public class SelfServiceModel
    {

        public string Id { get; set; }



        /// <summary>
        /// 产品名称
        /// </summary>
        /// 

        [TableCols(Tile = "产品名称")]
        public string ServiceName { get; set; }


        /// <summary>
        /// 产品 logo
        /// </summary>
        /// 
        [TableCols(Tile = "产品 logo", IsImage = true)]
        public string ServiceLogo { get; set; }


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

        /// <summary>
        /// 产品详情
        /// </summary>

        public string Detail { get; set; }
    }
}

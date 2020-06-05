using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Company
{
    public class CompanyModel
    {
        public string Id { get; set; }

        [TableCols(Tile = "商户名称")]

        public string Name { get; set; }



        //[TableCols(Tile = "商户号")]
        //public string CompanyNo { get; set; }


        [TableCols(Tile = "与客户聊天显示的名称")]
        public string ChatName { get; set; }

        [TableCols(Tile = "与客户聊天显示的头像", IsImage = true)]


        public string ChatLogo { get; set; }


        [TableCols(Tile = "备注")]

        public string Remark { get; set; }


        [TableCols(Tile = "创建时间")]

        public DateTime CreateDateTime
        {
            get; set;
        }


        [TableCols(Tile = "Pc端路径")]
        public string PcUrl { get; set; }
        [TableCols(Tile = "手机端路径")]
        public string MobileUrl { get; set; }
    }
}

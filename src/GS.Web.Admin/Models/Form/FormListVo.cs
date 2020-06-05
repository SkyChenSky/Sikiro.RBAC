using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Form
{
    public class FormListVo
    {
        public string Id { get; set; }

        [TableCols(Tile = "表单类型")]
        public string Type { get; set; }



        [TableCols(Tile = "表单号")]
        public string FormNo { get; set; }

        [TableCols(Tile = "上一环节操作")]
        public string UserName { get; set; }


        /// <summary>
        /// 表单当前状态
        /// </summary>
        [TableCols(Tile = "表单当前状态")]
        public string Status { get; set; }

      
    }


    public class HomeFormVo
    {
        public string Id { get; set; }

        public string TypeName { get; set; }

        public string FormNo { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }


        public string StatusName { get; set; }

        public DateTime CreateTime { get; set; }


        public string OrderNo { get; set; }




    }
}

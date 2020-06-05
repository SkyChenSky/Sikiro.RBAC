using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Position
{
    public class PositionModel
    {

        public string Id { get; set; }

        [TableCols(Tile = "岗位名称")]

        public string Name { get; set; }


        [TableCols(Tile = "排序值")]

        public int Order { get; set; }

        /// <summary>
        /// 编辑者
        /// </summary>
        /// 
        [TableCols(Tile = "编辑者")]
        public string AdministratorName { get; set; }

        [TableCols(Tile = "备注")]
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        [TableCols(Tile ="操作时间")]
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
    }
}

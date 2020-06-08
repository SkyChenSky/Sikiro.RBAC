using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Position
{
    public class PositionModel
    {

        public string Id { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [TableCols(Tile = "岗位名称")]
        public string Name { get; set; }


        /// <summary>
        /// 排序值
        /// </summary>
        [TableCols(Tile = "排序值")]
        public int Order { get; set; }

        /// <summary>
        /// 编辑者
        /// </summary>
        [TableCols(Tile = "编辑者")]
        public string AdministratorName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [TableCols(Tile = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [TableCols(Tile ="操作时间")]
        public DateTime UpdateDateTime { get; set; }
    }
}

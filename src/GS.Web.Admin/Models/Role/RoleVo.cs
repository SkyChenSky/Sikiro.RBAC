using System;
using MongoDB.Bson;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Role
{
    public class RoleVo
    {
        public string Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [TableCols(Tile = "角色名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [TableCols(Tile = "创建时间")]
        public DateTime CreateDateTime { get; set; }
    }
}

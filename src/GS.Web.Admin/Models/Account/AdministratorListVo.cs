using System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Account
{
    public class AdministratorListVo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        [TableCols(Tile = "姓名")]
        public string RealName { get; set; }

        [TableCols(Tile = "登录名")]
        public string UserName { get; set; }

        [TableCols(Tile = "手机")]
        public string Phone { get; set; }

        [TableCols(Tile = "所属部门")]
        public string Department { get; set; }

        [TableCols(Tile = "岗位")]
        public string Position { get; set; }

        [TableCols(Tile = "角色")]
        public string Role { get; set; }

        [TableCols(Tile = "账号状态")]
        public string Status { get; set; }

        public int StatusForInt { get; set; }

        [TableCols(Tile = "管理员类型")]
        public string Type { get; set; }

        [TableCols(Tile = "创建时间")]
        public DateTime CreateDateTime { get; set; }
    }
}

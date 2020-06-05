using Sikiro.Entity.System;

namespace Sikiro.Web.Admin.Models.Administrator
{
    public class ListSearchParams
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        public EAdministratorStatus? Status { get; set; }

        /// <summary>
        /// 管理员类型
        /// </summary>
        public EAdminType? Type { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 岗位Id
        /// </summary>
        public string PositionId { get; set; }
    }
}

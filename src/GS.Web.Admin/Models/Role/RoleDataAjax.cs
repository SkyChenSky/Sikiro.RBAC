using System.Collections.Generic;

namespace Sikiro.Web.Admin.Models.Role
{
    public class RoleDataAjax
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public bool IsSelect { get; set; }
        public bool IsDisabled { get; set; }
        public List<MAction> MActionlList { get; set; }
    }

    public class MAction
    {
        public string MActionId { get; set; }
        public string MActionName { get; set; }

        public bool IsSelect { get; set; }

        #region 管理员列表的分配权限才用到

        public bool IsDisabled { get; set; }


        #endregion
    }

    public class RoleRequest
    {
        public string roleId { get; set; }
        public string roleName { get; set; }
        public List<string> operations { get; set; }
        public string ShowCheckDataAjax { get; set; }

        public  List<string> MenuIds { get; set; }
    }
}

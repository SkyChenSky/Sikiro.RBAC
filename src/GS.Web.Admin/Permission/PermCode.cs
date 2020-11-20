using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Permission
{
    /// <summary>
    /// 权限值枚举
    /// </summary>
    public enum PermCode : int
    {
        #region 管理员
        [Display(Name = "管理员列表")]
        Administrator_List = 10001,
        [Display(Name = "管理员添加")]
        Administrator_Add = 10002,
        [Display(Name = "管理员编辑")]
        Administrator_Edit = 10003,
        [Display(Name = "管理员启用")]
        Administrator_Open = 10004,
        [Display(Name = "管理员停用")]
        Administrator_Stop = 10005,
        [Display(Name = "管理员重置密码")]
        Administrator_ResetPassword = 10006,
        [Display(Name = "管理员批量设置状态")]
        Administrator_BatchSetStatus = 10007,
        #endregion

        #region 岗位
        [Display(Name = "岗位管理列表")]
        Position_List = 10009,
        [Display(Name = "岗位管理添加")]
        Position_Add = 10010,
        [Display(Name = "岗位管理编辑")]
        Position_Edit = 10008,
        [Display(Name = "岗位管理删除")]
        Position_Delete = 10011,
        #endregion

        #region 角色管理
        [Display(Name = "角色管理列表")]
        Role_List = 10012,
        [Display(Name = "角色管理添加")]
        Role_Add = 10013,
        [Display(Name = "角色管理编辑")]
        Role_Edit = 10014,
        [Display(Name = "角色管理删除")]
        Role_Delete = 10015,
        [Display(Name = "角色管理权限")]
        Role_Jurisdiction = 10016,
        #endregion

        #region 组织管理
        [Display(Name = "组织管理列表")]
        Department_List = 10020,
        [Display(Name = "组织管理添加编辑")]
        Department_AddEdit = 10021,
        [Display(Name = "组织管理删除")]
        Department_Delete = 10022,
        #endregion

        #region 菜单管理
        [Display(Name = "菜单管理列表")]
        Menu_List = 10030,
        [Display(Name = "菜单管理添加编辑")]
        Menu_AddEdit = 10031,
        [Display(Name = "菜单管理关联")]
        Menu_Relate = 10032,
        [Display(Name = "菜单管理删除")]
        Menu_Delete = 10033,
        #endregion
    }
}
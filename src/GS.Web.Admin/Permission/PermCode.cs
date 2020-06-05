using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Permission
{
    /// <summary>
    /// 权限值枚举
    /// </summary>
    public enum PermCode : int
    {
        #region 管理员-100
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
        [Display(Name = "管理员分配权限")]
        Administrator_Jurisdiction = 10008,
        #endregion

        #region 岗位
        [Display(Name = "岗位管理列表")]
        Position_List = 10009,
        [Display(Name = "岗位管理添加")]
        Position_Add = 10010,
        [Display(Name = "岗位管理编辑")]
        Position_Edit = 10016,
        [Display(Name = "岗位管理删除")]
        Position_Dete = 10011,
        #endregion

        #region 角色管理
        [Display(Name = "角色管理列表")]
        Role_List = 10012,
        [Display(Name = "角色管理添加")]
        Role_Add = 10013,
        [Display(Name = "角色管理编辑")]
        Role_Edit = 10014,
        [Display(Name = "角色管理删除")]
        Role_Dete = 10015,
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

        #region 前台系统以2开头
        [Display(Name = "企业列表")]
        Company_list = 20001,
        [Display(Name = "企业添加和编辑")]
        Company_AddorEdit = 20002,
        [Display(Name = "企业删除")]
        Company_Dele = 20003,
        [Display(Name = "微商城列表")]
        Shopping_list = 20004,
        [Display(Name = "微商城添加")]
        Shopping_Add = 20005,
        [Display(Name = "微商城编辑")]
        Shopping_Edit = 20006,
        [Display(Name = "微商城删除")]
        Shopping_Dele = 20007,
        [Display(Name = "产品列表")]
        ProductAging_list = 20008,
        [Display(Name = "产品添加")]
        ProductAging_Add = 20009,
        [Display(Name = "产品编辑")]
        ProductAging_Edit = 20010,
        [Display(Name = "产品删除")]
        ProductAging_Dele = 20011,
        [Display(Name = "自助服务列表")]
        SelfService_list = 20012,
        [Display(Name = "自助服务添加")]
        SelfService_Add = 20013,
        [Display(Name = "自助服务编辑")]
        SelfService_Edit = 20014,
        [Display(Name = "自助服务删除")]
        SelfService_Dele = 20015,
        [Display(Name = "用户反馈列表")]
        FeedBack_list = 20016,
        [Display(Name = "用户反馈编辑")]
        FeedBack_Edit = 20017,
        [Display(Name = "用户信息列表")]
        Info_list = 20018,


        [Display(Name = "最近联系人列表")]
        Relation_list = 20060,

        [Display(Name = "预设回复列表")]
        ChatReply_List = 20019,
        [Display(Name = "添加预设类型")]
        ChatReply_TypeAdd = 20020,
        [Display(Name = "预设添加")]
        ChatReply_Add = 20021,
        [Display(Name = "预设批量删除")]
        ChatReply_batchDele = 20022,
        [Display(Name = "预设删除")]
        ChatReply_Dele = 20023,
        [Display(Name = "预设编辑")]
        ChatReply_Edit = 20024,
        [Display(Name = "客服表单审批列表")]
        Form_clientList = 20025,
        [Display(Name = "客服表单审操作")]
        Form_clientOperation = 20026,

        [Display(Name = "仓库表单审批列表")]
        Form_warehouseList = 20027,
        [Display(Name = "仓库表单操作")]
        Form_warehouseOperation = 20028,
        [Display(Name = "站内信息列表")]
        Info_news = 20029,
        [Display(Name = "站内信息回复")]
        Info_Add = 20030,

        [Display(Name = "用户信息的启用")]
        Info_Open = 20031,
        [Display(Name = "用户信息的停用")]
        Info_Stop = 20032,

        [Display(Name = "客服聊天")]
        CustomService_List = 20040,

        [Display(Name = "聊天记录列表")]
        Chat_List = 20050
        #endregion
    }
}
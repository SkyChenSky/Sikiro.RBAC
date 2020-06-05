using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.System
{
    /// <summary>
    /// 后台管理员
    /// </summary>
    [Mongo(DbConfig.Name)]
    public class Administrator : MongoEntity
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public ObjectId CompanyId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public ObjectId DepartmentId { get; set; }

        /// <summary>
        /// 岗位id数组
        /// </summary>
        private ObjectId[] _positionIds;

        public ObjectId[] PositionIds
        {
            set => _positionIds = value;
            get => _positionIds ?? new ObjectId[] { };
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        private ObjectId[] _roleIds;

        public ObjectId[] RoleIds
        {
            set => _roleIds = value;
            get => _roleIds ?? new ObjectId[] { };
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        public EAdministratorStatus Status { get; set; }

        /// <summary>
        /// 管理员类型
        /// </summary>
        public EAdminType Type { get; set; }

        /// <summary>
        /// 操作权限数组
        /// </summary>
        private ObjectId[] _menuActionIds;

        public ObjectId[] MenuActionIds
        {
            set => _menuActionIds = value;
            get => _menuActionIds ?? new ObjectId[] { };
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        private ObjectId[] _dataAccessIds;

        public ObjectId[] DataAccessIds
        {
            set => _dataAccessIds = value;
            get => _dataAccessIds ?? new ObjectId[] { };
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        private ObjectId[] _menuIds;

        public ObjectId[] MenuId
        {
            set => _menuIds = value;
            get => _menuIds ?? new ObjectId[] { };
        }

        /// <summary>
        /// 是否本系统超级管理员
        /// </summary>
        public bool IsSuper { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
    }

    public enum EAdministratorStatus
    {
        [Display(Name = "禁止登录")]
        Stop = -20,
        [Display(Name = "已注销")]
        Cancel = -10,
        [Display(Name = "已删除")]
        Deleted = -1,
        [Display(Name = "未激活")]
        NotActive = 0,
        [Display(Name = "正常")]
        Normal = 1,
    }

    public enum EAdminType
    {
        [Display(Name = "系统管理账号")]
        Admin = 0,
        [Display(Name = "企业客户账号")]
        Company = 1,
        [Display(Name = "个人用户账号")]
        Person = 1
    }
}

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

    /// <summary>
    /// 管理员状态
    /// </summary>
    public enum EAdministratorStatus
    {
        [Display(Name = "已停用")]
        Stop = -1,
        [Display(Name = "已删除")]
        Deleted = -1,
        [Display(Name = "未激活")]
        NotActive = 0,
        [Display(Name = "正常")]
        Normal = 1,
    }
}

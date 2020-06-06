using System.ComponentModel.DataAnnotations;
using Mapster;
using Sikiro.Entity.System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Administrator
{
    /// <summary>
    /// 编辑模型
    /// </summary>
    public class AddModel
    {
        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        [Required]
        [Display(Name = "登录名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        [Required]
        [Display(Name = "部门")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 岗位id数组
        /// </summary>
        [Required]
        [Display(Name = "岗位")]
        [AdaptIgnore]
        public string PositionIds { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        [Display(Name = "角色")]
        [AdaptIgnore]
        public string RoleIds { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        [Display(Name = "手机")]
        [Mobile]
        public string Phone { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        [Display(Name = "状态")]
        public EAdministratorStatus Status { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "企业")]
        [Required]
        public string CompanyId { get; set; }
    }
}

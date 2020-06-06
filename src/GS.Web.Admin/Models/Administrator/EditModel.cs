using System.ComponentModel.DataAnnotations;
using Sikiro.Entity.System;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Administrator
{
    /// <summary>
    /// 编辑模型
    /// </summary>
    public class EditModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [Display(Name = "姓名")]

        public string RealName { get; set; }


        /// <summary>
        /// 登录名称
        /// </summary>
        [Required(ErrorMessage = "登录名称不能为空")]
        [Display(Name = "登录名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        [Required(ErrorMessage = "部门不能为空")]
        [Display(Name = "部门")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 岗位id数组
        /// </summary>
        [Required(ErrorMessage = "岗位不能为空")]
        [Display(Name = "岗位")]

        public string PositionIds { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required(ErrorMessage = "角色不能为空")]
        [Display(Name = "角色")]
        public string RoleIds { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "手机不能为空")]
        [Display(Name = "手机")]
        [Mobile]
        public string Phone { get; set; }

        /// <summary>
        /// 账号状态
        /// </summary>
        [Display(Name = "状态")]
        public EAdministratorStatus Status { get; set; }
    }
}

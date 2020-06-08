using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Sikiro.Web.Admin.Models.Role
{
    public class AddRoleModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "角色名称不能为空")]
        public string Name { get; set; }
    }
}

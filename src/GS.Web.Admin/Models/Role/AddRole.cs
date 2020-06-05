using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace Sikiro.Web.Admin.Models.Role
{
    public class AddRole
    {

        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "角色名称不能为空")]
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据权限
        /// </summary>
        public ObjectId[] DataAccessIds { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public ObjectId[] MenuId { get; set; }

        /// <summary>
        /// 功能
        /// </summary>
        public ObjectId[] MenuActionIds { get; set; }
    }
}

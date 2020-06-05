using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.Menu
{
    public class AddEditModel
    {
        public string Id { get; set; }

        [Display(Name = "菜单名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "菜单图标")]
        [Required]
        public string Icon { get; set; }

        [Display(Name = "菜单路径")]
        public string Url { get; set; }

        [Display(Name = "排序")]
        [Required]
        public int Order { get; set; }

        [Display(Name = "父菜单")]
        public string ParentId { get; set; }
    }
}

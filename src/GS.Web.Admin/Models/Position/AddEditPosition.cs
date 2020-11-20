using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Sikiro.Common.Utils;

namespace Sikiro.Web.Admin.Models.Position
{
    public class AddEditPosition
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// 
        [Display(Name = "岗位名称")]
        [Required(ErrorMessage = "请输入岗位名称")]
        [StringLength(10, ErrorMessage = "岗位名称不能超过10字符")]
        [RegularExpression(RegularExpression.ZhongYing, ErrorMessage = "岗位名称只允许输入中文、英文字符")]
        public string Name { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        [Display(Name = "排序值")]
        [Required(ErrorMessage = "请输入排序值")]
        public int Order { get; set; }

        /// <summary>
        /// 编辑者
        /// </summary>
        public ObjectId AdministratorId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(10, ErrorMessage = "备注长度不能超过50个中文字符")]
        public string Remark { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
    }
}

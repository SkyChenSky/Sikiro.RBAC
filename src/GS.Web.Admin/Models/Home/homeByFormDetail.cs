using System;
using MongoDB.Bson;
using Sikiro.Entity.Admin;

namespace Sikiro.Web.Admin.Models.Home
{
    public class HomeByFormDetail
    {
        /// <summary>
        /// 表单ID
        /// </summary>
        public ObjectId FormId { get; set; }

        /// <summary>
        /// 处理管理员ID
        /// </summary>
        public ObjectId AdministratorId { get; set; }

        /// <summary>
        /// 处理管理员名称
        /// </summary>
        public string AdministratorName { get; set; }

        /// <summary>
        /// 处理情况
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 表单当前状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }


        public FormStatus StatusInt { get; set; }

        public string FormName { get; set; }
    }
}

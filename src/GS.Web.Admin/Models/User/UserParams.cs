using System;

namespace Sikiro.Web.Admin.Models.User
{
    public class UserParams
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime? CreateDateTime { get; set; }
    }
}

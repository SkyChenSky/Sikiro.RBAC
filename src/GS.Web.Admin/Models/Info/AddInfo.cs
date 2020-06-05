using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.Info
{
    public class AddInfo
    {
        [Display(Name = "消息标题")]
        [Required(ErrorMessage = "请输入产品名称")]
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }


        [Display(Name = "消息内容")]
        [Required(ErrorMessage = "消息内容")]
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Desc { get; set; }


        /// <summary>
        /// 00000000000000 代表所有人，否则发送给某一个用
        /// </summary>
        public string ToUser { get; set; }


        /// <summary>
        /// 发送类型
        /// </summary>
        [Display(Name = "发送类型")]
        public string sendlist { get; set; }


        public string userList { get; set; }


        /// <summary>
        /// 特殊用
        /// </summary>

        public string UserName { get; set; }

    }

}

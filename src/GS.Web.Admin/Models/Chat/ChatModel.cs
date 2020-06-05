using System;
using MongoDB.Bson;
using Sikiro.Web.Admin.Attribute;

namespace Sikiro.Web.Admin.Models.Chat
{
    public class ChatModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public ObjectId UserId { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public ObjectId CompanyId { get; set; }

        /// <summary>
        /// 表单类型
        /// </summary>
        [TableCols(Tile = "用户名称")]
        public string FormUser { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [TableCols(Tile = "内容")]
        public string News { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [TableCols(Tile = "图片", IsImage = true)]
        public string Picture { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [TableCols(Tile = "时间")]
        public DateTime CreateDateTime { get; set; }
    }
}

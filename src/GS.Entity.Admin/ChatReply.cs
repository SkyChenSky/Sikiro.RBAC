using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    [Mongo(DbConfig.Name)]
    public class ChatReply : MongoEntity, ITenant
    {
        /// <summary>
        /// 企业 id
        /// </summary>
        public ObjectId? CompanyId { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
       public ObjectId ParentId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string News { get; set; }

       /// <summary>
       /// 排序
       /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 状态，0:启用，-1 停用
        /// </summary>
        public ChatReplyStatus Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 简码
        /// </summary>
        public string Code { get; set; }

    }

    public enum ChatReplyStatus
    {
 
        [Display(Name = "启用")]
        Open,
        [Display(Name = "停用")]
        Stop
    }
}

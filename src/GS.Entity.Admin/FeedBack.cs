using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    /// <summary>
    /// 后台用户反馈表
    /// </summary>
    [Mongo(DbConfig.Name)]
   public  class FeedBack : MongoEntity, ITenant
    {


        public ObjectId UserId { get; set; }
        /// <summary>
        /// 企业商户 id
        /// </summary>
        public ObjectId? CompanyId { get; set; }

        /// <summary>
        /// 反馈内容 
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        public string Reply { get; set; }

        /// <summary>
        /// 反馈日期
        /// </summary>
     
        public  DateTime? FeedBackTime { get; set; }

        /// <summary>
        /// 回复日期
        /// </summary>
        public DateTime? ReplyTime { get; set; }
    }
}

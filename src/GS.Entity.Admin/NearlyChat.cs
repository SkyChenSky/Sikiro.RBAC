using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    [Mongo(DbConfig.Name)]
    public class NearlyChat : MongoEntity, ITenant
    {
        /// <summary>
        /// 客服Id
        /// </summary>
        public List<ObjectId> CustomServiceId { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public ObjectId? CompanyId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public ObjectId CustomId { get; set; }

        /// <summary>
        /// 匿名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDateTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }


        /// <summary>
        /// 条数
        /// </summary>
        public int Count { get; set; }

        public string ChatRemark { get; set; }

    }
}

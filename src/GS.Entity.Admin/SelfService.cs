using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    /// <summary>
    /// 自助服务表
    /// </summary>
    [Mongo(DbConfig.Name)]
    public class SelfService : MongoEntity, ITenant
    {

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ServiceName { get; set; }


        /// <summary>
        /// 产品 logo
        /// </summary>
        public string ServiceLogo { get; set; }


        /// <summary>
        /// 排序，值越大越排
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>

        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 产品详情
        /// </summary>

        public string Detail { get; set; }

        public ObjectId? CompanyId { get; set; }
    }
}

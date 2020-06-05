using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    [Mongo(DbConfig.Name)]
    public class Info : MongoEntity, ITenant
    {
        public EInfoType Type { get; set; }

        public string Title { get; set; }

        public string Desc { get; set; }

        public ObjectId ToUser { get; set; }

        public DateTime CreateDateTime { get; set; }

        public ObjectId? CompanyId { get; set; }
    }

    public enum EInfoType
    {
        /// <summary>
        /// 站内信
        /// </summary>
        Local
    }
}

using System;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.System
{
    /// <summary>
    /// 后台企业商户表 
    /// </summary>

    [Mongo(DbConfig.Name)]
    public class Company : MongoEntity
    {
        public string Name { get; set; }

        public string ChatName { get; set; }

        public string ChatLogo { get; set; }

        public string Remark { get; set; }

        public DateTime CreateDateTime
        {
            get; set;
        }


        public string CompanyNo { get; set; }
    }
}

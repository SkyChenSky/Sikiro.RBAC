using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    /// <summary>
    /// 微商城表
    /// </summary>

    [Mongo(DbConfig.Name)]
    public  class Shopping: MongoEntity, ITenant
    {
        /// <summary>
        /// 企业商户 id
        /// </summary>
        public ObjectId? CompanyId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 产品 logo
        /// </summary>
        public string ProductLogo { get; set; }

        /// <summary>
        /// 属性 1，例如：时效
        /// </summary>
        public string Item1 { get; set; }

        /// <summary>
        /// 属性 1 字段的值 
        /// </summary>
        public string Item1Val { get; set; }

        /// <summary>
        /// 属性 2，例如：收费标准 
        /// </summary>

        public string Item2 { get; set; }

        /// <summary>
        /// 属性 2 字段的值 
        /// </summary>

        public string Item2Val { get; set; }

        /// <summary>
        /// 排序，值越大越排
        /// </summary>



        public int Order { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>

        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 点击跳转的网址 
        /// </summary>
        public string GoUrl { get; set; }

    }
}

using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    [Mongo(DbConfig.Name)]
    public class Chat : MongoEntity, ITenant
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public ObjectId UserId { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public ObjectId? CompanyId { get; set; }

        /// <summary>
        /// 表单类型
        /// </summary>
        public ObjectId FormId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string News { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public EChatStatus Status { get; set; }

        /// <summary>
        /// 发送源类型
        /// </summary>
        public ESendFrom SendFrom { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public EChatType Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

    }

    public enum EChatStatus
    {
        NoRead,
        Read
    }

    public enum ESendFrom
    {
        /// <summary>
        /// 客服
        /// </summary>
        CustomService,
        /// <summary>
        /// 客户
        /// </summary>
        Custom
    }

    public enum EChatType
    {
        /// <summary>
        /// 文字
        /// </summary>
        Word = 1,

        /// <summary>
        /// 图片
        /// </summary>
        Picture,

        /// <summary>
        /// 表单
        /// </summary>
        Form
    }
}

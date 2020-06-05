using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    /// <summary>
    ///  用户表
    /// </summary>
    [Mongo(DbConfig.Name)]
    public class User : MongoEntity, ITenant
    {
        public string UserNo { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string OpenId { get; set; }

        public string WxName { get; set; }

        public string UserLogo { get; set; }

        public ObjectId? CompanyId { get; set; }

        public UserStatus Status { get; set; }

        public bool IsBlack { get; set; }

        public DateTime? CreateDateTime { get; set; }

        /// <summary>
        /// 是否修改过用户名
        /// </summary>
        public bool IsChangedUserName { get; set; }

        public string ChatRemark { get; set; }
    }
    public enum UserStatus
    {
        Stop,
        Open
    }
}

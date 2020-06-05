using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.System
{
    [Mongo(DbConfig.Name)]
    public class Role : MongoEntity
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据权限
        /// </summary>
        public ObjectId[] DataAccessIds { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public ObjectId[] MenuId { get; set; }

        /// <summary>
        /// 功能
        /// </summary>
        public ObjectId[] MenuActionIds { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人的UerID
        /// </summary>
        public ObjectId UserId { get; set; }
    }
}

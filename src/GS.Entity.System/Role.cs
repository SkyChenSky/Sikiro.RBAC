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
        public DateTime CreateDateTime { get; set; }
    }
}

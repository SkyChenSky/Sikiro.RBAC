using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.System
{
    [Mongo(DbConfig.Name)]
    public class Department : MongoEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public ObjectId? ParentId { get; set; }
    }
}

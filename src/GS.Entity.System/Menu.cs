using System;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.System
{
    [Mongo(DbConfig.Name)]
    public class Menu : MongoEntity
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public int Order { get; set; }

        public ObjectId? ParentId { get; set; }

        private ObjectId[] _menuActionIds;

        public ObjectId[] MenuActionIds
        {
            set => _menuActionIds = value;
            get => _menuActionIds ?? new ObjectId[] { };
        }

        public DateTime CreateDateTime { get; set; }
    }
}

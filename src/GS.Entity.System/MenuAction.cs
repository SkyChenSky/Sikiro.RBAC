using System;
using System.Collections.Generic;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.System
{
    [Mongo(DbConfig.Name)]
    public class MenuAction : MongoEntity
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public int Code { get; set; }

        public List<string> Url { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}

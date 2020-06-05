using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.System
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleService : IDepend
    {
        private readonly MongoRepository _mongoRepository;

        public RoleService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public List<Role> GetByIds(IEnumerable<ObjectId> ids)
        {
            if (ids == null || !ids.Any())
                return new List<Role>();

            return _mongoRepository.ToList<Role>(a => ids.Contains(a.Id));
        }

        public ServiceResult Add(Role model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess("操作成功");
        }

        public List<Role> GetTopList(int? top)
        {

            return _mongoRepository.ToList<Role>(a => true, a => a.Desc(b => b.CreateTime), top);
        }

        public ServiceResult Update(string id, Expression<Func<Role, Role>> updateExpression)
        {
            _mongoRepository.Update<Role>(c => c.Id == id.ToObjectId(), updateExpression);
            return ServiceResult.IsSuccess("操作成功");
        }
        public bool Exists(Expression<Func<Role, bool>> expression)
        {
            return _mongoRepository.Exists(expression);
        }

        public Role Get(Expression<Func<Role, bool>> updateExpression)
        {

            return _mongoRepository.Get<Role>(updateExpression);
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetSelectList()
        {
            return _mongoRepository.ToList<Role>(a => true, a => a.Asc(b => b.CreateTime), null);
        }

        public ServiceResult Delete(Expression<Func<Role, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0 ? ServiceResult.IsSuccess("删除成功") : ServiceResult.IsFailed("删除失败");
        }
    }
}

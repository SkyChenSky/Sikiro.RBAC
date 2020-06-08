using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System.Const;
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

        /// <summary>
        /// 获取列表页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public PageList<Role> GetPageList(int pageIndex, int pageSize, Expression<Func<Role, bool>> expression)
        {
            return _mongoRepository.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        /// <summary>
        /// 根据Id组获取
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Role> GetByIds(IEnumerable<ObjectId> ids)
        {
            if (ids == null || !ids.Any())
                return new List<Role>();

            return _mongoRepository.ToList<Role>(a => ids.Contains(a.Id));
        }

        /// <summary>
        /// 根据Id组获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Role GetById(string id)
        {
            var idObjectId = id.ToObjectId();
            return _mongoRepository.Get<Role>(a => a.Id == idObjectId);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult Add(Role entity)
        {
            entity.MenuActionIds = new ObjectId[] { };
            entity.MenuId = new ObjectId[] { };
            entity.CreateDateTime = DateTime.Now;

            var result = _mongoRepository.AddIfNotExist(a => a.Name == entity.Name, entity);
            return result
                ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess)
                : ServiceResult.IsFailed("已存在相同名称的角色");
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult Edit(Role entity)
        {
            entity.MenuActionIds = new ObjectId[] { };
            entity.MenuId = new ObjectId[] { };
            entity.CreateDateTime = DateTime.Now;

            var result = _mongoRepository.Exists<Role>(a => a.Id != entity.Id && a.Name == entity.Name);

            if (result)
                return ServiceResult.IsFailed("已存在相同名称的角色");

            return
                _mongoRepository.Update(entity)
                    ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess)
                    : ServiceResult.IsFailed(AccountConstString.OperateFailed);
        }


        public ServiceResult Update(string id, Expression<Func<Role, Role>> updateExpression)
        {
            _mongoRepository.Update<Role>(c => c.Id == id.ToObjectId(), updateExpression);
            return ServiceResult.IsSuccess("操作成功");
        }

        public Role Get(Expression<Func<Role, bool>> updateExpression)
        {
            return _mongoRepository.Get(updateExpression);
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetSelectList()
        {
            return _mongoRepository.ToList<Role>(a => true, a => a.Asc(b => b.CreateDateTime), null);
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ServiceResult DeleteById(string roleId)
        {
            var roleIdObject = roleId.ToObjectId();
            var isExists = _mongoRepository.Exists<Administrator>(a => a.RoleIds.Contains(roleIdObject));
            if (isExists)
                return ServiceResult.IsFailed("无法删除已分配关联用户的数据");

            return _mongoRepository.Delete<Role>(a => a.Id == roleIdObject) > 0
                ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess)
                : ServiceResult.IsFailed(AccountConstString.OperateFailed);
        }
    }
}

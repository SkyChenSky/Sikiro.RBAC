using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Bson;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.System
{
    /// <summary>
    /// 部门
    /// </summary>
    public class DepartmentService : IDepend
    {
        private readonly MongoRepository _mongoRepository;

        public DepartmentService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public PageList<Department> GetList()
        {
            var list = _mongoRepository.ToList<Department>(a => true, a => a.Desc(b => b.Order), null);
            return new PageList<Department>(1, 10, list.Count, list);
        }

        public Department GetById(string id)
        {
            return _mongoRepository.Get<Department>(a => a.Id == id.ToObjectId());
        }

        public List<Department> GetByIds(IEnumerable<ObjectId> ids)
        {
            if (!ids.Any())
                return new List<Department>();

            return _mongoRepository.ToList<Department>(a => ids.Contains(a.Id));
        }

        public ServiceResult Add(Department entity)
        {
            var result = _mongoRepository.AddIfNotExist(a => a.Name == entity.Name, entity);

            return result
                ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess)
                : ServiceResult.IsFailed("已存在相同名称的部门");
        }

        public ServiceResult Update(Department entity)
        {
            var isExists = _mongoRepository.Exists<Department>(a => a.Id != entity.Id && a.Name == entity.Name);
            if (isExists)
                return ServiceResult.IsSuccess("已存在相同名称的部门");

            _mongoRepository.Update(entity);

            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }


        public Department Get(Expression<Func<Department, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Department> GetSelectList(string parentId = null)
        {
            var list = _mongoRepository.ToList<Department>(
                a => a.Id != parentId.ToObjectId() && a.ParentId != parentId.ToObjectId(), a => a.Desc(b => b.Order),
                null);

            return RecursionDepartmentList(list, null);
        }

        private IEnumerable<Department> RecursionDepartmentList(List<Department> list, ObjectId? parentId, int deepCount = 0)
        {
            var newList = new List<Department>();
            var oldList = list.Where(a => a.ParentId == parentId).ToList();

            var sb = new StringBuilder();
            for (var i = 0; i < deepCount; i++)
                sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            if (sb.Length > 0)
                sb.Append("|--");

            ++deepCount;
            oldList.ForEach(item =>
            {
                item.Name = sb + item.Name;
                newList.Add(item);
                var recursionResult = RecursionDepartmentList(list, item.Id, deepCount);
                newList.AddRange(recursionResult);
            });

            return newList;
        }

        public ServiceResult DeleteById(string id)
        {
            return _mongoRepository.Delete<Department>(a => a.Id == id.ToObjectId()) > 0 ? ServiceResult.IsSuccess("删除成功") : ServiceResult.IsFailed("删除失败");
        }

        public ServiceResult IsCanDelete(string id)
        {
            var isHasDepartment = _mongoRepository.Exists<Department>(a => a.ParentId == id.ToObjectId());
            if (isHasDepartment)
                return ServiceResult.IsFailed("此部门拥有多个子部门，删除失败");

            var isHasAdmin = _mongoRepository.Exists<Administrator>(a => a.DepartmentId == id.ToObjectId());
            if (isHasAdmin)
                return ServiceResult.IsFailed("此部门拥有多个部门人员，删除失败");

            return ServiceResult.IsSuccess("");
        }
    }
}

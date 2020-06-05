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
    /// 岗位
    /// </summary>
    public class PositionService : IDepend
    {
        private readonly MongoRepository _mongoRepository;

        public PositionService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public List<Position> GetByIds(IEnumerable<ObjectId> ids)
        {
            if (ids == null || !ids.Any())
                return new List<Position>();

            return _mongoRepository.ToList<Position>(a => ids.Contains(a.Id));
        }

        public PageList<Position> GetPageList(int pageIndex, int pageSize, Expression<Func<Position, bool>> expression)
        {
            return _mongoRepository.PageList(expression, a => a.Desc(b => b.UpdateDateTime), pageIndex, pageSize);
        }

        public ServiceResult Add(Position model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess("添加成功");
        }

        public ServiceResult Update(string id, Position model)
        {
            _mongoRepository.Update<Position>(c => c.Id == new ObjectId(id), c => new Position
            {
                Name = model.Name,
                Remark = string.IsNullOrEmpty(model.Remark) ? "" : model.Remark,
                Order = model.Order,
                AdministratorId = model.AdministratorId,
                UpdateDateTime = model.UpdateDateTime
            });
            return ServiceResult.IsSuccess("更新成功");
        }
        public Position Get(Expression<Func<Position, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public bool Exists(Expression<Func<Position, bool>> expression)
        {
            return _mongoRepository.Exists(expression);
        }
        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Position> GetSelectList()
        {
            return _mongoRepository.ToList<Position>(a => true, a => a.Desc(b => b.Order), null);
        }

        public ServiceResult Delete(Expression<Func<Position, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0 ? ServiceResult.IsSuccess("删除成功") : ServiceResult.IsFailed("删除失败");
        }

    }
}

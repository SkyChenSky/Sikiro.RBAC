using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using Sikiro.Common.Utils;
using Sikiro.Entity.Admin;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Nosql.Mongo.Base;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.Repository.Admin
{
    public class MongoRep
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly MongoRepository _mongoRepository;

        public MongoRep(IHttpContextAccessor accessor, MongoRepository mongoRepository)
        {
            _accessor = accessor;
            _mongoRepository = mongoRepository;
        }

        private AdministratorData GetUser()
        {
            return _accessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value
                 .FromJson<AdministratorData>();
        }

        private ObjectId? GetUserId()
        {
            return GetUser()?.UserId.ToObjectId();
        }

        private bool IsSuper()
        {
            return GetUser()?.IsSuper ?? false;
        }

        private Expression<Func<T, bool>> GetWhere<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity, ITenant
        {
            //todo 删除
            if (IsSuper())
                return predicate;

            var userId = GetUserId();
            ObjectId? companyId = null;

            if (userId.HasValue)
                companyId = _mongoRepository.Get<Administrator, ObjectId?>(a => a.Id == userId.Value, a => a.DepartmentId);

            if (predicate == null)
                predicate = ExpressionBuilder.Init<T>();

            return predicate.And(a => a.CompanyId == companyId);
        }

        public bool Add<T>(T entity) where T : MongoEntity, ITenant
        {
            entity.ThrowIfNull();

            var userId = GetUserId();
            ObjectId? companyId = null;

            if (userId.HasValue)
                companyId = _mongoRepository.Get<Administrator, ObjectId?>(a => a.Id == userId.Value, a => a.DepartmentId);

            if (entity.CompanyId == companyId)
            {
                _mongoRepository.Add(entity);
                return true;
            }

            return false;
        }

        public long Delete<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity, ITenant
        {
            var where = GetWhere(predicate);

            return _mongoRepository.Delete(where);
        }

        public long Update<T>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> updateExpression) where T : MongoEntity, ITenant
        {
            var where = GetWhere(whereExpression);

            return _mongoRepository.Update(where, updateExpression);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity, ITenant
        {
            var where = GetWhere(predicate);

            return _mongoRepository.Get(where);
        }

        public List<T> ToList<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity, ITenant
        {
            var where = GetWhere(predicate);

            return _mongoRepository.ToList(where);
        }

        public PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, Func<Sort<T>, Sort<T>> sort, int pageIndex, int pageSize) where T : MongoEntity, ITenant
        {
            var where = GetWhere(predicate);

            return _mongoRepository.PageList(where, sort, pageIndex, pageSize);
        }
    }
}

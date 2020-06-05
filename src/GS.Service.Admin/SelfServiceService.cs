using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Service.Admin.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    public class SelfServiceService : IDepend
    {

        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;

        public SelfServiceService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep = mongoRep;
        }

        public PageList<SelfService> GetPageList(int pageIndex, int pageSize, Expression<Func<SelfService, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public SelfService Get(Expression<Func<SelfService, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult Delete(Expression<Func<SelfService, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0 ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed); ;
        }

        public ServiceResult Add(SelfService model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Update(string id, SelfService model)
        {
            _mongoRepository.Update<SelfService>(c => c.Id == new ObjectId(id), c => new SelfService
            {
                ServiceName = model.ServiceName,
                ServiceLogo = string.IsNullOrEmpty(model.ServiceLogo) ? "" : model.ServiceLogo,
                Detail = string.IsNullOrEmpty(model.Detail) ? "" : model.Detail,
                Order = model.Order,
                CompanyId = model.CompanyId
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
    }
}

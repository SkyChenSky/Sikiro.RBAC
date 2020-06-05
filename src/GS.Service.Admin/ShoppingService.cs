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
   public class ShoppingService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;
        public ShoppingService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep = mongoRep;
        }

        public PageList<Shopping> GetPageList(int pageIndex, int pageSize, Expression<Func<Shopping, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }


        public Shopping Get(Expression<Func<Shopping, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult Delete(Expression<Func<Shopping, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0 ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed); ;
        }

        public ServiceResult Add(Shopping model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Update(string id, Shopping model)
        {
            _mongoRepository.Update<Shopping>(c => c.Id == new ObjectId(id), c => new Shopping
            {
                ProductName = model.ProductName,
                ProductLogo = string.IsNullOrEmpty(model.ProductLogo) ? "" : model.ProductLogo,
                Item1 = model.Item1,
                Item1Val = model.Item1Val,
                Item2 = model.Item2,
                Item2Val = model.Item2Val,
                Order = model.Order,
                CompanyId = model.CompanyId,
                GoUrl=model.GoUrl
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
    }
}

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
    /// <summary>
    /// 产品时效服务类
    /// </summary>
    public class ProductService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;

        public ProductService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep =mongoRep;
        }

        public PageList<Product> GetPageList(int pageIndex, int pageSize, Expression<Func<Product, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }


        public Product Get(Expression<Func<Product, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult Delete(Expression<Func<Product, bool>> expression)
        {
            return _mongoRepository.Delete(expression)>0? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed); ;
        }

        public ServiceResult Add(Product model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Update(string id, Product model)
        {
           _mongoRepository.Update<Product>(c=>c.Id==new ObjectId(id),c=>new Product {
               ProductName=model.ProductName,
               ProductLogo=string.IsNullOrEmpty(model.ProductLogo)?"": model.ProductLogo,
               Detail= string.IsNullOrEmpty(model.Detail) ? "" : model.Detail,
               Item1=model.Item1,
               Item1Val=model.Item1Val,
               Item2=model.Item2,
               Item2Val=model.Item2Val,
               Order=model.Order,
               CompanyId=model.CompanyId
           });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
    }
}


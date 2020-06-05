using System;
using System.Linq.Expressions;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Service.Admin.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    /// <summary>
    /// 站内信
    /// </summary>
    public class InfoService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;

        public InfoService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep =mongoRep;
        }

        public PageList<Info> GetPageList(int pageIndex, int pageSize, Expression<Func<Info, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public Info Get(Expression<Func<Info, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult Add(Info model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
    }

}

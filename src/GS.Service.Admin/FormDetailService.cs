using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    public class FormDetailService : IDepend
    {
        private readonly MongoRepository _mongoRepository;

        public FormDetailService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        public ServiceResult Add(FormDetail model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }


        public List<FormDetail> GetList(Expression<Func<FormDetail, bool>> expression)
        {
            return _mongoRepository.ToList(expression);
        }


        /// <summary>
        /// 表单4列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<FormDetail> GetTopList()
        {
            return _mongoRepository.ToList<FormDetail>(a => true, a => a.Desc(b => b.CreateDateTime),null);
        }


        public ServiceResult Update(string id, Expression<Func<FormDetail, FormDetail>> updateExpression)
        {
            _mongoRepository.Update<FormDetail>(c => c.FormId == id.ToObjectId(), updateExpression);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public FormDetail Get(Expression<Func<FormDetail, bool>> expression)
        {
            return _mongoRepository.Get<FormDetail>(expression);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Common.Utils;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    public class FormService : IDepend
    {
        private readonly MongoRepository _mongoRepository;

        public FormService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public PageList<Form> GetPageList(int pageIndex, int pageSize, Expression<Func<Form, bool>> expression)
        {
            return _mongoRepository.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public Form Get(Expression<Func<Form, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }


        public ServiceResult Update(string id, Expression<Func<Form, Form>> updateExpression)
        {
            _mongoRepository.Update<Form>(c => c.Id == id.ToObjectId(), updateExpression);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }


        public ServiceResult UpdateByStatus(string id, FormStatus status, Expression<Func<Form, Form>> updateExpression)
        {
            _mongoRepository.Update<Form>(c => c.Id == id.ToObjectId() && c.Status == status, updateExpression);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }


        public void Add(Form model)
        {
            _mongoRepository.Add(model);
        }



        /// <summary>
        /// 表单4列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public List<Form> GetTopList(int? top)
        {

            return _mongoRepository.ToList<Form>(a =>a.Status != FormStatus.Cancel && a.Status != FormStatus.Wait,a => a.Desc(b => b.CreateDateTime), top);
        }


        public long Count()
        {

            return _mongoRepository.Count<Form>(c=>c.Status != FormStatus.Cancel &&c.Status != FormStatus.Wait&&c.CreateDateTime>= AccountHelper.GetMonthStart()&& c.CreateDateTime<AccountHelper.GetMonthEnd());
        }


    }
}

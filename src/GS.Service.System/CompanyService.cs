using System;
using System.Collections.Generic;
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
    /// 公司
    /// </summary>
    public class CompanyService : IDepend
    {

        private readonly MongoRepository _mongoRepository;

        public CompanyService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public List<Company> ToList(Expression<Func<Company, bool>> expression)
        {
            return _mongoRepository.ToList(expression);
        }

        public Company GetById(string id)
        {
            return _mongoRepository.Get<Company>(a => a.Id == new ObjectId(id));
        }




        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>

        public string GetCompanyNo()
        {
            var model = _mongoRepository.Get<Company>(c => true, a => a.Desc(b => b.CreateDateTime));
            if (model != null&& model.CompanyNo!=null)
            {
                return GetRecursionNo(decimal.Parse(model.CompanyNo));
            }
            else
            {
                return "7123";
            }
        }

        public string GetRecursionNo(decimal no)
        {
            var companyNo = no.ToString();
            var model = _mongoRepository.Get<Company>(c => c.CompanyNo == companyNo);
            if (model != null)
            {
                no = no + 1m;
                return GetRecursionNo(no);
            }
            else
            {
                return companyNo;
            }
        }



        public PageList<Company> GetPageList(int pageIndex, int pageSize, Expression<Func<Company, bool>> expression)
        {
            return _mongoRepository.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public ServiceResult Add(Company model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }


        public Company Get(Expression<Func<Company, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult Delete(Expression<Func<Company, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0 ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed); ;
        }

        public ServiceResult Update(string id, Company model)
        {
            _mongoRepository.Update<Company>(c => c.Id == new ObjectId(id), c => new Company
            {
                Name = model.Name,
                ChatName = string.IsNullOrEmpty(model.ChatName) ? "" : model.ChatName,
                ChatLogo = string.IsNullOrEmpty(model.ChatLogo) ? "" : model.ChatLogo,
                Remark = string.IsNullOrEmpty(model.Remark) ? "" : model.Remark,
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public bool Exists(Expression<Func<Company, bool>> expression)
        {
            return _mongoRepository.Exists(expression);
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Company> GetSelectList()
        {
            var list = _mongoRepository.ToList<Company>(a => true, a => a.Desc(b => b.CreateDateTime),
                null);

            return list;
        }

  
    }
}

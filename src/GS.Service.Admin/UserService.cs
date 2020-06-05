using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Service.Admin.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    public class UserService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;

        public UserService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep =mongoRep;
        }
        public List<User> list(Expression<Func<User, bool>> expression)
        {
            return _mongoRepository.ToList(expression);
        }

        public User Get(Expression<Func<User, bool>> expression)
        {
            return _mongoRep.Get(expression);
        }

        public PageList<User> GetPageList(int pageIndex, int pageSize, Expression<Func<User, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public ServiceResult Stop(Expression<Func<User, bool>> expression)
        {
            _mongoRepository.Update<User>(expression, c => new User
            {
              Status=UserStatus.Stop
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
        public ServiceResult Open(Expression<Func<User, bool>> expression)
        {
            _mongoRepository.Update<User>(expression, c => new User
            {
                Status = UserStatus.Open
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public List<User> GetList(Expression<Func<User, bool>> expression)
        {
            return _mongoRepository.ToList(expression);
        }

    }
}

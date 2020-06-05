using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    public class ChatService: IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;

        public ChatService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep = mongoRep;
        }


        public List<Chat> ToList(Expression<Func<Chat, bool>> expression)
        {
            return _mongoRepository.ToList(expression);
        }
        public PageList<Chat> GetPageList(int pageIndex, int pageSize, Expression<Func<Chat, bool>> expression)
        {
         
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }


        public PageList<NearlyChat> GetNearlyChatList(int pageIndex, int pageSize, Expression<Func<NearlyChat, bool>> expression)
        {

            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public long GetNearlyChatCount(Expression<Func<NearlyChat, bool>> expression)
        {

            return _mongoRepository.Count<NearlyChat>(expression);
        }
    }
}

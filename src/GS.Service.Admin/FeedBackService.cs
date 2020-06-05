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
    public class FeedBackService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;
        public FeedBackService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep = mongoRep;
        }

        public PageList<FeedBack> GetPageList(int pageIndex, int pageSize, Expression<Func<FeedBack, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.FeedBackTime), pageIndex, pageSize);
        }


        public FeedBack Get(Expression<Func<FeedBack, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult Update(string id, string replyVules)
        {
            _mongoRepository.Update<FeedBack>(c => c.Id == new ObjectId(id), c => new FeedBack
            {
                 Reply=replyVules,
                 ReplyTime=DateTime.Now
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Update(string id, FeedBack model)
        {
            _mongoRepository.Update<FeedBack>(c => c.Id == new ObjectId(id), c => new FeedBack
            {
                Reply = model.Reply,
                ReplyTime = DateTime.Now,
                CompanyId= model.CompanyId
            });
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
    }
}

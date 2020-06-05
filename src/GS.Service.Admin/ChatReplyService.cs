using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Service.Admin.Bo;
using Sikiro.Service.Admin.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    /// <summary>
    /// 客服预设消息回复
    /// </summary>
    public class ChatReplyService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;
        public ChatReplyService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep = mongoRep;
        }

        public PageList<ChatReply> GetPageList(int pageIndex, int pageSize,
            Expression<Func<ChatReply, bool>> expression)
        {
            return _mongoRep.PageList(expression, a => a.Desc(b => b.CreateDateTime), pageIndex, pageSize);
        }

        public ChatReply GetById(string id)
        {
            return _mongoRepository.Get<ChatReply>(a => a.Id == new ObjectId(id));
        }

        public ServiceResult UpdateById(ObjectId id, ChatReply update)
        {
            _mongoRepository.Update<ChatReply>(a => a.Id == id, c => new ChatReply
            {
                CompanyId = update.CompanyId,
                ParentId = update.ParentId,
                Status = update.Status,
                News = update.News,
                Order = update.Order,
                Code = update.Code
            });

            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult UpdateTypeById(ObjectId id, ChatReply update)
        {
            _mongoRepository.Update<ChatReply>(a => a.Id == id, c => new ChatReply
            {
                CompanyId = update.CompanyId,
                ParentId = update.ParentId,
            });

            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Add(ChatReply administrator)
        {
            _mongoRepository.Add(administrator);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public List<ChatReply> GetList(Expression<Func<ChatReply, bool>> expression)
        {
            return _mongoRep.ToList(expression);
        }

        public ServiceResult Delete(Expression<Func<ChatReply, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0
                ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess)
                : ServiceResult.IsFailed(AccountConstString.OperateFailed);
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IEnumerable<ChatReplyBo> GetAllList(string companyId)
        {
            var list = _mongoRepository.ToList<ChatReply>(a => a.CompanyId == companyId.ToObjectId() && a.Status == ChatReplyStatus.Open, a => a.Desc(b => b.Order), null);

            return RecursionChatReplyList(list, ObjectId.Empty);
        }

        /// <summary>
        /// 递归操作
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static IEnumerable<ChatReplyBo> RecursionChatReplyList(IEnumerable<ChatReply> list, ObjectId parentId)
        {
            return list.Where(a => a.ParentId == parentId).Select(a => new ChatReplyBo
            {
                Id = a.Id.ToString(),
                Code = a.Code,
                Name = a.News,
                Child = RecursionChatReplyList(list, a.Id)
            });
        }
    }
}

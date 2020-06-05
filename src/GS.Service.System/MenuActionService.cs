using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System.Bo;
using Sikiro.Service.System.Const;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.System
{

    /// <summary>
    /// 后台菜单权限
    /// </summary>
    public class MenuActionService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        public MenuActionService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public ServiceResult Add(MenuAction model)
        {
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Update(string id, Expression<Func<MenuAction, MenuAction>> updateExpression)
        {
            _mongoRepository.Update(c => c.Id == id.ToObjectId(), updateExpression);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public List<MenuAction> GetMenuActionList(Expression<Func<MenuAction, bool>> updateExpression)
        {
            return _mongoRepository.ToList(updateExpression);
        }

        public MenuAction Get(Expression<Func<MenuAction, bool>> updateExpression)
        {
            return _mongoRepository.Get(updateExpression);
        }

        public ServiceResult Delete(Expression<Func<MenuAction, bool>> expression)
        {
            return _mongoRepository.Delete(expression) > 0 ? ServiceResult.IsSuccess(AccountConstString.OperateSuccess) : ServiceResult.IsFailed(AccountConstString.OperateFailed);
        }

        public IEnumerable<MenuActionBo> GetAllActionById(string menuId)
        {
            var menu = _mongoRepository.Get<Menu>(a => a.Id == menuId.ToObjectId());

            var hasSelectMenu = menu?.MenuActionIds ?? new ObjectId[] { };
            var menuAction = GetAllActionByUrl(menu?.Url ?? "", menu?.MenuActionIds ?? new ObjectId[] { }, menuId.ToObjectId());

            return menuAction.Select(a => new MenuActionBo
            { Id = a.Id.ToString(), Name = a.Name, Selected = hasSelectMenu.Contains(a.Id) });
        }

        private IEnumerable<MenuAction> GetAllActionByUrl(string url, ObjectId[] menuActionIds, ObjectId menuId)
        {
            var urlSplit = url.Trim('/').Split("/")[0];

            var notCanSelectMenuActionIds = _mongoRepository
                .ToList<Menu, ObjectId[]>(a => a.Id != menuId, a => a.MenuActionIds).Where(a => a != null).SelectMany(a => a);

            return _mongoRepository.ToList<MenuAction>(a =>
                (a.Url.Any(b => b.Contains(urlSplit)) || menuActionIds.Contains(a.Id)) && !notCanSelectMenuActionIds.Contains(a.Id));
        }

        public ServiceResult SetMenuAction(string menuId, string[] menuActionIds)
        {
            var ids = menuActionIds.MapTo<ObjectId[]>();
            _mongoRepository.Update<Menu>(a => a.Id == menuId.ToObjectId(), a => new Menu
            {
                MenuActionIds = ids
            });

            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
    /// 后台菜单
    /// </summary>
    public class MenuService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private List<Menu> newList = new List<Menu>();
        private List<ObjectId> DataIds = new List<ObjectId>();

        public MenuService(MongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        public ServiceResult Add(Menu model)
        {
            model.Url = model.Url?.ToLower();
            _mongoRepository.Add(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public ServiceResult Update(Menu model)
        {
            model.Url = model.Url?.ToLower();
            _mongoRepository.Update(model);
            return ServiceResult.IsSuccess(AccountConstString.OperateSuccess);
        }

        public Menu Get(Expression<Func<Menu, bool>> expression)
        {
            return _mongoRepository.Get(expression);
        }

        public ServiceResult IsNameExists(string id, string name)
        {
            var isExists = _mongoRepository.Exists<Menu>(a => a.Id != id.ToObjectId() && a.Name == name);
            return isExists ? ServiceResult.IsFailed("已存在该菜单名称") : ServiceResult.IsSuccess("");
        }

        /// <summary>
        /// 获取树形列表数据
        /// </summary>
        /// <returns></returns>
        public PageList<Menu> GetTableList()
        {
            var list = _mongoRepository.ToList<Menu>(a => true, a => a.Desc(b => b.Order), null);
            return new PageList<Menu>(1, 10, list.Count, list);
        }

        /// <summary>
        /// 获取下拉框列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Menu> GetSelectList(string parentId = null)
        {
            var list = _mongoRepository.ToList<Menu>(
                a => a.Id != parentId.ToObjectId() && a.ParentId != parentId.ToObjectId(), a => a.Desc(b => b.Order),
                null);

            return RecursionMenuList(list, null);
        }

        /// <summary>
        /// 递归列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <param name="deepCount"></param>
        /// <returns></returns>
        private IEnumerable<Menu> RecursionMenuList(IEnumerable<Menu> list, ObjectId? parentId, int deepCount = 0)
        {
            var newList = new List<Menu>();
            var oldList = list.Where(a => a.ParentId == parentId).ToList();

            var sb = new StringBuilder();
            for (var i = 0; i < deepCount; i++)
                sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            if (sb.Length > 0)
                sb.Append("|--");

            ++deepCount;
            oldList.ForEach(item =>
            {
                item.Name = sb + item.Name;
                newList.Add(item);
                var recursionResult = RecursionMenuList(list, item.Id, deepCount);
                newList.AddRange(recursionResult);
            });

            return newList;
        }

        /// <summary>
        /// 根据ID获取菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Menu GetById(string id)
        {
            return _mongoRepository.Get<Menu>(a => a.Id == id.ToObjectId());
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult DeleteById(string id)
        {
            return _mongoRepository.Delete<Menu>(a => a.Id == id.ToObjectId()) > 0 ? ServiceResult.IsSuccess("删除成功") : ServiceResult.IsFailed("删除失败");
        }

        /// <summary>
        /// 是否可以删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult IsCanDelete(string id)
        {
            var isHasMenu = _mongoRepository.Exists<Menu>(a => a.ParentId == id.ToObjectId());
            if (isHasMenu)
                return ServiceResult.IsFailed("请先删除子菜单");

            return ServiceResult.IsSuccess("");
        }

        /// <summary>
        /// 递归操作
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<ObjectId> GetObjIdList(List<ShowCheck> list)
        {

            foreach (var item in list)
            {
                DataIds.Add(item.Id.ToObjectId());
                GetObjIdList(item.children);
            }
            return DataIds;
        }

        /// <summary>
        /// 获取全部列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<MenuBo> GetMenuInAdmin(string userId)
        {
            var admin = _mongoRepository.Get<Administrator>(a => a.Id == userId.ToObjectId());
            if (admin == null)
                return new List<MenuBo>();

            var list = _mongoRepository.ToList<Menu>(a => true, a => a.Desc(b => b.Order), null);
            if (!admin.IsSuper)
            {
                list = GetAdminMenu(list, admin.RoleIds).ToList().DistinctBy(a => a.Id).ToList();
            }

            return RecursionChatReplyList(list, null);
        }

        private IEnumerable<Menu> GetAdminMenu(List<Menu> menuList, ObjectId[] roleIds)
        {
            var menuIdsInRole = _mongoRepository.ToList<Role>(a => roleIds.Contains(a.Id)).SelectMany(a => a.MenuId);

            return RecursionMenuListOfParentId(menuList, null, menuIdsInRole).OrderByDescending(b => b.Order);
        }

        private IEnumerable<Menu> RecursionMenuListOfParentId(IEnumerable<Menu> list, ObjectId? id,
            IEnumerable<ObjectId> menuIds = null)
        {
            var newList = new List<Menu>();
            var oldList = list.ToList();

            oldList = menuIds != null
                ? oldList.Where(a => menuIds.Contains(a.Id) && a.ParentId != null).ToList()
                : oldList.Where(a => a.Id == id).ToList();

            oldList.ForEach(item =>
            {
                newList.Add(item);
                var recursionResult = RecursionMenuListOfParentId(list, item.ParentId);
                newList.AddRange(recursionResult);
            });

            return newList;
        }

        /// <summary>
        /// 递归操作
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static IEnumerable<MenuBo> RecursionChatReplyList(IEnumerable<Menu> list, ObjectId? parentId)
        {
            return list.Where(a => a.ParentId == parentId).Select(a => new MenuBo
            {
                Icon = a.Icon,
                Name = a.Name,
                Url = a.Url,
                Children = RecursionChatReplyList(list, a.Id)
            });
        }

        public IEnumerable<string> GetAllMenuUrl()
        {
            return _mongoRepository.ToList<Menu, string>(a => a.Url != null, a => a.Url.ToLower().Trim('/'));
        }
    }

    /// <summary>
    ///显示layuitreeCheck的格式
    /// </summary>
    public class ShowCheck
    {
        public string Title { get; set; }
        public string Id { get; set; }

        public bool Checked { get; set; }

        public bool Spread { get; set; }

        public bool Disabled { get; set; }

        public List<ShowCheck> children { get; set; }
    }

}

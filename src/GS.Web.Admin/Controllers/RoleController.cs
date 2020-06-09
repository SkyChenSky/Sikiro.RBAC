using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models.Position;
using Sikiro.Web.Admin.Models.Role;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 角色管理 
    /// </summary>
    public class RoleController : BaseController
    {
        #region 初始化
        private readonly RoleService _roleService;
        private readonly MenuService _menuService;
        private readonly MenuActionService _menuActionService;
        public RoleController(RoleService roleService, MenuService menuService, MenuActionService menuActionService)
        {
            _roleService = roleService;
            _menuService = menuService;
            _menuActionService = menuActionService;
        }
        #endregion

        #region 列表页
        public IActionResult Index()
        {
            return View();
        }

        [Permission(PermCode.Role_List)]
        public IActionResult List(PageListParams<RoleParams> model)
        {
            var where = ExpressionBuilder.Init<Role>();
            var param = model.Params;

            if (!param.RoleName.IsNullOrEmpty())
                where = where.And(a => a.Name.Contains(param.RoleName));

            var result = _roleService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => a.MapTo<RoleVo>());
            return PageList(result);
        }
        #endregion

        #region 创建角色
        /// <summary>
        /// 添加角色页面
        /// </summary>
        [Permission(PermCode.Role_Add)]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        [HttpPost]
        [Permission(PermCode.Role_Add)]
        public IActionResult Add(AddRoleModel model)
        {
            var entity = model.MapTo<Role>();

            var reuslt = _roleService.Add(entity);

            return Json(reuslt);
        }
        #endregion

        #region 编辑角色
        /// <summary>
        /// 编辑角色页面
        /// </summary>
        [Permission(PermCode.Role_Edit)]
        public IActionResult Edit(string id)
        {
            var roleVo = _roleService.GetById(id).MapTo<EditRoleVo>();
            return View(roleVo);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        [HttpPost]
        [Permission(PermCode.Role_Edit)]
        public IActionResult Edit(EditRoleModel model)
        {
            var entity = model.MapTo<Role>();

            var reuslt = _roleService.Edit(entity);

            return Json(reuslt);
        }
        #endregion

        #region 删除角色

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Role_Delete)]
        public IActionResult Delete(string id)
        {
            var json = _roleService.DeleteById(id);
            return Json(json);
        }

        #endregion

        #region 权限

        /// <summary>
        /// 角色权限页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Jurisdiction(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Role_Jurisdiction)]
        public IActionResult Jurisdiction(string id,List<JurisdictionModel> model)
        {
            var resultList = new List<JurisdictionModel>();
            FormatJurisdictionTree(resultList, model);

            var menuActionIds = resultList.Where(a => a.Field == "action").Select(a => a.Id.ToObjectId()).ToArray();
            var menuIds = resultList.Where(a => a.Field == "menu").Select(a => a.Id.ToObjectId()).ToArray();

            var result =_roleService.SetMenuAndMenuAction(id, menuActionIds, menuIds);

            return Json(result);
        }

        /// <summary>
        /// 嵌套转一维
        /// </summary>
        /// <param name="newList"></param>
        /// <param name="oldList"></param>
        private void FormatJurisdictionTree(List<JurisdictionModel> newList, List<JurisdictionModel> oldList)
        {
            oldList.ForEach(a =>
            {
                newList.Add(new JurisdictionModel
                {
                    Title = a.Title,
                    Field = a.Field,
                    Id = a.Id
                });

                if (a.Children != null && a.Children.Any())
                {
                    FormatJurisdictionTree(newList, a.Children);
                }
            });
        }

        /// <summary>
        /// 权限树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Role_Jurisdiction)]
        public IActionResult JurisdictionTree(string id)
        {
            var haveCheckedMenuActionIds = _roleService.GetMenuActionIds(id);
            var allMenuActionList = _roleService.GetAllMenuAction();

            var treeList =  _menuService.GetTreeList(haveCheckedMenuActionIds, allMenuActionList).MapTo<List<JurisdictionVo>>();

            return Json(treeList);
        }

        #endregion

        #region 岗位下拉框

        /// <summary>
        /// 岗位下拉框
        /// </summary>
        /// <returns></returns>
        public IActionResult RoleSelect(List<string> value)
        {
            var selectList = _roleService.GetSelectList().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = value.Contains(a.Id.ToString())
            });

            return Json(selectList);
        }

        #endregion
    }
}

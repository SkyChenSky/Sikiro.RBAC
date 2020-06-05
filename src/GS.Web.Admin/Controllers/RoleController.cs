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
using Sikiro.Web.Admin.Models.Role;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 角色管理 
    /// </summary>
    public class RoleController : BaseController
    {
        private readonly RoleService _roleService;
        private readonly MenuService _menuService;
        private readonly MenuActionService _menuActionService;
        private readonly AdministratorService _administratorService;
        public RoleController(RoleService roleService, MenuService menuService, MenuActionService menuActionService, AdministratorService administratorService)
        {
            _roleService = roleService;
            _menuService = menuService;
            _menuActionService = menuActionService;
            _administratorService = administratorService;
        }
        public IActionResult Index(string r)
        {
            ViewBag.roleList = _roleService.GetTopList(null);
            ViewBag.roleId = string.IsNullOrEmpty(r) ? "" : r;  //用户编辑好定位某个角色
            return View();
        }
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
        public IActionResult AddAJax(AddRole model)
        {

            var isExist = _roleService.Exists(c => c.Name == model.Name);
            if (isExist)
            {
                return Json(ServiceResult.IsFailed("角色名已存在"));
            }
            var entity = model.MapTo<Role>();
            entity.DataAccessIds = new ObjectId[] { };
            entity.MenuActionIds = new ObjectId[] { };
            entity.MenuId = new ObjectId[] { };
            entity.CreateTime = DateTime.Now;
            entity.UserId = CurrentUserData.UserId.ToObjectId();
            var json = _roleService.Add(entity);
            return Json(json);
        }
        #endregion

        #region 获取角色权限数据，设置角色权限数据，删除角色

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Role_Edit)]
        public IActionResult RoleAJax(RoleRequest requestModel)
        {
            List<ObjectId> mActionIds = new List<ObjectId>(); //存菜单操作id
            List<ObjectId> dataIds = new List<ObjectId>();//存数据id
            var menuList = _menuService.GetMenuList(c => true);
            if (string.IsNullOrEmpty(requestModel.roleId))
            {
                return Json(ServiceResult.IsFailed("请选择角色"));
            }
            if (string.IsNullOrEmpty(requestModel.roleName))
            {
                return Json(ServiceResult.IsFailed("请角色名称"));
            }

            var roleModel = _roleService.Get(c => c.Name == requestModel.roleName);
            if (roleModel != null)
            {
                if (roleModel.Id != requestModel.roleId.ToObjectId())
                {
                    return Json(ServiceResult.IsFailed("角色名已存在"));
                }
            }
            if (requestModel.operations != null)   //通过mActionId 去反查menuId
            {
                if (requestModel.operations.Any())
                {
                    foreach (var opeId in requestModel.operations)
                    {
                        var toOpeId = opeId.ToObjectId();//菜单操作id
                        var menuModel = menuList.FirstOrDefault(c => c.MenuActionIds.Contains(toOpeId));
                        if (menuModel != null)
                        {
                            if (!mActionIds.Contains(menuModel.Id))
                            {
                                mActionIds.Add(menuModel.Id);
                            }
                        }
                    }

                }

            }
            if (requestModel.ShowCheckDataAjax != null)  //权限数据格式
            {
                var ShowCheck = requestModel.ShowCheckDataAjax.FromJson<List<ShowCheck>>();
                if (ShowCheck.Any())
                {
                    dataIds = _menuService.GetObjIdList(ShowCheck);
                }
            }
            var role = _roleService.Get(c => c.Id == requestModel.roleId.ToObjectId());
            //var menuId = mActionIds.ToArray();
            var menuActionIds = requestModel.operations == null ? new ObjectId[] { } : requestModel.operations.Select(a => a.ToObjectId()).ToArray();
            var userId = CurrentUserData.UserId.ToObjectId();
            var dataByIds = dataIds.ToArray();

            #region 合并菜单id
            var memuIds = mActionIds.Select(c => c.ToString()).ToList();
            var mergeMemuId = requestModel.MenuIds != null ? requestModel.MenuIds.Union(memuIds) : memuIds; 
            var menuIdToArray= mergeMemuId.Select(c=>c.ToObjectId()).ToArray();
            #endregion
            _roleService.Update(role.Id.ToString(), c => new Role
            {

                MenuId = menuIdToArray,
                MenuActionIds = menuActionIds,
                UserId = userId,
                Name = requestModel.roleName,
                DataAccessIds = dataByIds

            });

            return Json(ServiceResult.IsSuccess("操作成功", new { roleId = requestModel.roleId }));
        }


        /// <summary>
        /// 根据角色获取权限数据
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Role_List)]
        public IActionResult GetRoleJurisdicData(string roleId)
        {
            List<RoleDataAjax> roleDataAjaxList = new List<RoleDataAjax>(); //layui tree 格式
            var role = _roleService.Get(c => c.Id == roleId.ToObjectId());
            var menuList = _menuService.ParentMenuListList();
            var menuActionList = _menuActionService.GetMenuActionList(a => true);
            if (menuList.Any())
            {
                foreach (var memu in menuList)  //查询所有的菜单
                {
                    var roleDataAjax = new RoleDataAjax
                    {
                        MenuId = memu.Id.ToString(),
                        MenuName = memu.Name,
                        MActionlList = new List<MAction>(),
                        IsSelect = role?.MenuId != null && role.MenuId.Contains(memu.Id)
                    };

                    if (memu.MenuActionIds != null && memu.MenuActionIds.Any())
                    {
                        foreach (var mAcId in memu.MenuActionIds)
                        {
                            var mAcModel = menuActionList.FirstOrDefault(c => c.Id == mAcId);
                            if (mAcModel != null)
                            {
                                MAction mActionModel = new MAction()
                                {
                                    MActionId = mAcModel.Id.ToString(),
                                    MActionName = mAcModel.Name,
                                    IsSelect = false
                                };
                                if (role != null)
                                {
                                    if (role.MenuActionIds != null && role.MenuActionIds.Contains(mAcModel.Id))
                                    {
                                        mActionModel.IsSelect = true;

                                    }

                                }
                                roleDataAjax.MActionlList.Add(mActionModel);
                            }

                        }
                    }
                   // if (isSon)  //菜单有操作才显示,注释掉就会显示所有菜单
                        roleDataAjaxList.Add(roleDataAjax);

                }
            }
            var data = _menuService.GetAllList(role.DataAccessIds);

            return Json(new { list = roleDataAjaxList, roleDataAccess = data });
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Role_Dete)]
        public IActionResult RoleRemove(string roleId)
        {


            var model = _administratorService.GetByRoleId(roleId.ToObjectId());
            if (model != null)
            {
                return Json(ServiceResult.IsFailed("已有其他人员分配该角色,不能删除"));
            }
            var json = _roleService.Delete(c => c.Id == roleId.ToObjectId());
            return Json(json);

        }

        #endregion

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

    }
}

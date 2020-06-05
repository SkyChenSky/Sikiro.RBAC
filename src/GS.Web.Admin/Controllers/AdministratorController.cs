using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models.Account;
using Sikiro.Web.Admin.Models.Administrator;
using Sikiro.Web.Admin.Models.Role;
using Sikiro.Web.Admin.Permission;
using ObjectExtension = Sikiro.Common.Utils.ObjectExtension;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class AdministratorController : BaseController
    {
        private readonly AdministratorService _administratorService;
        private readonly DepartmentService _departmentService;
        private readonly RoleService _roleService;
        private readonly PositionService _positionService;
        private readonly MenuService _menuService;
        private readonly MenuActionService _menuActionService;

        public AdministratorController(AdministratorService administratorService,
            DepartmentService departmentService,
            RoleService roleService,
            PositionService positionService,
            MenuService menuService,
            MenuActionService menuActionService)
        {
            _administratorService = administratorService;
            _departmentService = departmentService;
            _roleService = roleService;
            _positionService = positionService;
            _menuService = menuService;
            _menuActionService = menuActionService;

        }

        #region 列表
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Permission(PermCode.Administrator_List)]
        public IActionResult List(Models.PageListParams<ListSearchParams> model)
        {
            var where = ExpressionBuilder.Init<Administrator>();
            var param = model.Params;

            if (!param.RealName.IsNullOrEmpty())
                where = where.And(a => a.RealName.Contains(param.RealName));

            if (!param.UserName.IsNullOrEmpty())
                where = where.And(a => a.UserName.Contains(param.UserName));

            if (param.Status.HasValue)
                where = where.And(a => a.Status == param.Status.Value);

            if (param.Type.HasValue)
                where = where.And(a => a.Type == param.Type.Value);

            if (!param.DepartmentId.IsNullOrEmpty())
                where = where.And(a => a.DepartmentId == param.DepartmentId.ToObjectId());

            if (!param.PositionId.IsNullOrEmpty())
                where = where.And(a => a.PositionIds.Contains(param.DepartmentId.ToObjectId()));

            if (!param.RoleId.IsNullOrEmpty())
                where = where.And(a => a.RoleIds.Contains(param.RoleId.ToObjectId()));

            var administratorList = _administratorService.GetPageList(model.Page, model.Limit, where);
            var departmentList = _departmentService.GetByIds(administratorList.Items.Select(a => a.DepartmentId).ToList());
            var roleList = _roleService.GetByIds(administratorList.Items.SelectMany(a => a.RoleIds));
            var positionList = _positionService.GetByIds(administratorList.Items.SelectMany(a => a.PositionIds));

            var result = _administratorService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a =>
            {
                var department = departmentList.FirstOrDefault(b => b.Id == a.DepartmentId);

                var role = roleList.Where(b => Enumerable.Contains(a.RoleIds, b.Id)).Aggregate("", (current, item) => current + (item.Name + ",")).Trim(',');

                var position = positionList.Where(b => Enumerable.Contains(a.PositionIds, b.Id)).Aggregate("", (current, item) => current + (item.Name + ",")).Trim(',');

                return new AdministratorListVo
                {
                    CreateDateTime = a.CreateDateTime,
                    RealName = a.RealName,
                    Status = ObjectExtension.GetDisplayName(a.Status),
                    StatusForInt = a.Status.GetHashCode(),
                    Type = ObjectExtension.GetDisplayName(a.Type),
                    UserName = a.UserName,
                    Id = a.Id.ToString(),
                    Phone = a.Phone,
                    Position = position,
                    Role = role,
                    Department = department?.Name,
                };
            });
            return PageList(result);
        }
        #endregion

        #region 添加
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Permission(PermCode.Administrator_Add)]
        public IActionResult Add(AddModel model)
        {
            var entity = model.MapTo<Administrator>();

            entity.PositionIds = model.PositionIds.Split(",").Select(a => a.ToObjectId()).ToArray();
            entity.RoleIds = model.RoleIds.Split(",").Select(a => a.ToObjectId()).ToArray();

            var result = _administratorService.Add(entity);

            return Json(result);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var admin = _administratorService.GetById(id);

            var result = admin.MapTo<EditVo>();

            return View(result);
        }

        [HttpPost]
        [Permission(PermCode.Administrator_Edit)]
        public IActionResult Edit(EditModel edit)
        {
            var checkResult = _administratorService.IsExists(edit.UserName, edit.Id);
            if (checkResult.Failed)
                return Json(checkResult);

            var departmentId = edit.DepartmentId.ToObjectId();
            var positionIds = edit.PositionIds.Split(",").Select(a => a.ToObjectId()).ToArray();
            var roleIds = edit.RoleIds.Split(",").Select(a => a.ToObjectId()).ToArray();
            var companyId = edit.CompanyId.ToObjectId();

            var result = _administratorService.UpdateById(edit.Id, a => new Administrator
            {
                UpdateDateTime = DateTime.Now,
                RealName = edit.RealName,
                Type = edit.Type,
                Status = edit.Status,
                DepartmentId = departmentId,
                PositionIds = positionIds,
                Phone = edit.Phone,
                RoleIds = roleIds,
                UserName = edit.UserName,
                CompanyId = companyId
            });

            return Json(result);
        }
        #endregion

        #region 行事件
        [HttpPost]
        [Permission(PermCode.Administrator_Stop)]
        public IActionResult Stop(string userId)
        {
            var result = _administratorService.UpdateStatus(userId, EAdministratorStatus.Stop);

            return Json(result);
        }

        [HttpPost]
        [Permission(PermCode.Administrator_Open)]
        public IActionResult Open(string userId)
        {
            var result = _administratorService.UpdateStatus(userId, EAdministratorStatus.Normal);

            return Json(result);
        }
        #endregion

        #region 重置密码
        [HttpGet]
        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordModel
            {
                UserId = id
            });
        }

        [HttpPost]
        [Permission(PermCode.Administrator_ResetPassword)]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            var result = _administratorService.ResetPassword(model.UserId, model.Password);
            return Json(result);
        }
        #endregion

        #region 批量设置状态
        [HttpGet]
        public IActionResult BatchSetStatus(List<string> id = null)
        {
            return View(new BatchSetStatusModel
            {
                UserIds = id ?? new List<string>()
            });
        }

        [HttpPost]
        [Permission(PermCode.Administrator_BatchSetStatus)]
        public IActionResult BatchSetStatus(BatchSetStatusModel model)
        {
            var result = _administratorService.BatchSetStatus(model.UserIds.Select(a => a.ToObjectId()).ToArray(), model.Status);
            return Json(result);
        }
        #endregion

        #region 分配权限
        [Permission(PermCode.Administrator_Jurisdiction)]
        public IActionResult Jurisdiction(string id)
        {
            ViewBag.AdminId = id;
            return View();
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public IActionResult AdminSetJurisdiction(AdminSetJurisdiction requestModel)
        {
            List<ObjectId> menuIdList = new List<ObjectId>(); //存菜单操作id
            ObjectId[] dataIds = { };//存数据id
            var menuList = _menuService.GetMenuList(c => true);
            var adminModel = _administratorService.GetById(requestModel.AdminId);
            if (string.IsNullOrEmpty(requestModel.AdminId))
            {
                return Json(ServiceResult.IsFailed("AdminId不能为空"));
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
                            if (!menuIdList.Contains(menuModel.Id))
                            {
                                menuIdList.Add(menuModel.Id);
                            }
                        }
                    }

                }

            }

            //var menuId = menuIdList.ToArray();
            var menuActionIds = requestModel.operations == null ? new ObjectId[] { } : requestModel.operations.Select(a => a.ToObjectId()).ToArray();

            dataIds = requestModel.layuiTreeCheck == null ? new ObjectId[] { } : requestModel.layuiTreeCheck.Select(c => c.ToObjectId()).ToArray();
            #region 合并菜单id
            var memuIds = menuIdList.Select(c => c.ToString()).ToList();
            var mergeMemuId = requestModel.MenuIds != null ? requestModel.MenuIds.Union(memuIds) : memuIds;
            var menuIdToArray = mergeMemuId.Select(c => c.ToObjectId()).ToArray();
            #endregion

            _administratorService.UpdateById(adminModel.Id.ToString(), c => new Administrator
            {
                MenuId = menuIdToArray,
                MenuActionIds = menuActionIds,
                DataAccessIds = dataIds
            });

            return Json(ServiceResult.IsSuccess("操作成功"));
        }

        /// <summary>
        /// 根据获取权限数据
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetRoleJurisdicData(string adminId)
        {
            var roleDataAjaxList = new List<RoleDataAjax>();
            var adminModel = _administratorService.GetById(adminId);
            var menuList = _menuService.ParentMenuListList();
            var menuActionList = _menuActionService.GetMenuActionList(a => true);//获取所有菜单的操作
            var mActionByRole = GetmActionList(adminModel.RoleIds);//获取所有指定角色的菜单的操作
            var menuidsByRole = GetMenuList(adminModel.RoleIds);//获取所有指定角色的菜单

            if (menuList.Any())
            {
                foreach (var memu in menuList)  //查询所有的菜单
                {
                    RoleDataAjax roleDataAjax = new RoleDataAjax
                    {
                        MenuId = memu.Id.ToString(),
                        MenuName = memu.Name,
                        MActionlList = new List<MAction>(),
                        IsSelect = false
                    };
                    if (adminModel.MenuId != null && adminModel.MenuId.Contains(memu.Id))
                    {
                        roleDataAjax.IsSelect = true;
                        roleDataAjax.IsDisabled = false;
                    }
                    if (menuidsByRole.Any() && menuidsByRole.Contains(memu.Id))
                    {
                        roleDataAjax.IsDisabled = true;
                        roleDataAjax.IsSelect = true;
                    }

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
                                    IsSelect = false,
                                    IsDisabled = false,
                                };

                                if (adminModel.MenuActionIds != null && adminModel.MenuActionIds.Contains(mAcModel.Id)) //有自己的就不会存在角色
                                {
                                    mActionModel.IsSelect = true;
                                    mActionModel.IsDisabled = false;
                                }
                                if (mActionByRole.Any() && mActionByRole.Contains(mAcModel.Id)) //有角色的就不会存在自己
                                {
                                    mActionModel.IsDisabled = true;
                                    mActionModel.IsSelect = true;
                                }

                                roleDataAjax.MActionlList.Add(mActionModel);
                            }
                        }
                    }
                    //if (isSon)  //菜单有操作才显示，注释掉就显示全部菜单
                        roleDataAjaxList.Add(roleDataAjax);
                }
            }
            var dataListByRole = GetDataList(adminModel.RoleIds);//获取角色的数据权限值
            var data = _menuService.GetAllList(adminModel.DataAccessIds, dataListByRole.ToArray());

            return Json(new { list = roleDataAjaxList, roleDataAccess = data });
        }


        /// <summary>
        /// 通过角色获取菜单里的权限值
        /// </summary>
        /// <returns></returns>
        public List<ObjectId> GetmActionList(ObjectId[] roleIds)
        {
            List<ObjectId> newmActionId = new List<ObjectId>();
            var menuActionList = _menuActionService.GetMenuActionList(a => true);
            var roleList = _roleService.GetByIds(roleIds);
            if (roleList != null && roleList.Any())
            {
                foreach (var roleId in roleList)
                {
                    foreach (var mAction in menuActionList)
                    {
                        var isExist = roleId.MenuActionIds.Contains(mAction.Id);
                        if (isExist && !newmActionId.Contains(mAction.Id))
                        {
                            newmActionId.Add(mAction.Id);
                        }
                    }
                }
            }

            return newmActionId;


        }




        /// <summary>
        /// 通过角色获取菜单
        /// </summary>
        /// <returns></returns>
        public List<ObjectId> GetMenuList(ObjectId[] roleIds)
        {
            List<ObjectId> newMenuId = new List<ObjectId>();
            var menuActionList = _menuActionService.GetMenuActionList(a => true);
            var roleList = _roleService.GetByIds(roleIds);
            if (roleList != null && roleList.Any())
            {
                foreach (var roleId in roleList)
                {
                    foreach (var dataId in roleId.MenuId)
                    {
                        if (!newMenuId.Contains(dataId))
                        {
                            newMenuId.Add(dataId);
                        }

                    }
                }
            }

            return newMenuId;


        }

        /// <summary>
        /// 通过角色权限数据
        /// </summary>
        /// <returns></returns>
        public List<ObjectId> GetDataList(ObjectId[] roleIds)
        {
            List<ObjectId> newDataId = new List<ObjectId>();
            var roleList = _roleService.GetByIds(roleIds);
            if (roleList != null && roleList.Any())
            {
                foreach (var roleId in roleList)
                {
                    foreach (var dataId in roleId.DataAccessIds)
                    {
                        if (!newDataId.Contains(dataId))
                        {
                            newDataId.Add(dataId);
                        }

                    }
                }
            }
            return newDataId;

        }

        #endregion
    }


}
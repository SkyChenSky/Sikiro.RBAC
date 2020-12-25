using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Common.Utils;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models.Account;
using Sikiro.Web.Admin.Models.Administrator;
using Sikiro.Web.Admin.Permission;

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

        public AdministratorController(AdministratorService administratorService,
            DepartmentService departmentService,
            RoleService roleService,
            PositionService positionService)
        {
            _administratorService = administratorService;
            _departmentService = departmentService;
            _roleService = roleService;
            _positionService = positionService;
        }

        #region 列表
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Permission(PermCode.Administrator_List)]
        public IActionResult List(PageListParams<ListSearchParams> model)
        {
            var where = ExpressionBuilder.Init<Administrator>();
            var param = model.Params;

            if (!param.RealName.IsNullOrEmpty())
                where = where.And(a => a.RealName.Contains(param.RealName));

            if (!param.UserName.IsNullOrEmpty())
                where = where.And(a => a.UserName.Contains(param.UserName));

            if (param.Status.HasValue)
                where = where.And(a => a.Status == param.Status.Value);

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
                    Status = a.Status.GetDisplayName(),
                    StatusForInt = a.Status.GetHashCode(),
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

            var result = _administratorService.UpdateById(edit.Id, a => new Administrator
            {
                UpdateDateTime = DateTime.Now,
                RealName = edit.RealName,
                Status = edit.Status,
                DepartmentId = departmentId,
                PositionIds = positionIds,
                Phone = edit.Phone,
                RoleIds = roleIds,
                UserName = edit.UserName,
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
    }


}
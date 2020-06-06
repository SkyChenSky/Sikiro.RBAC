using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sikiro.Common.Utils;
using Sikiro.Entity.System;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Web.Admin.Models.Menu;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuController : BaseController
    {
        private readonly MenuActionService _menuActionService;
        private readonly MenuService _menuService;
        public MenuController(MenuService menuService, MenuActionService menuActionService)
        {
            _menuActionService = menuActionService;
            _menuService = menuService;
        }

        #region 菜单页
        /// <summary>
        /// 页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [Permission(PermCode.Menu_List)]
        public IActionResult List()
        {
            var list = _menuService.GetTableList().UpdateForPageListResult(a => new ListVo
            {
                Id = a.Id.ToString(),
                Name = a.Name,
                PId = a.ParentId?.ToString() ?? "",
                Icon = a.Icon,
                Order = a.Order,
                Url = a.Url
            });
            return PageList(list);
        }
        #endregion

        #region 添加编辑
        /// <summary>
        /// 添加编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(PermCode.Menu_AddEdit)]
        public IActionResult AddEdit(string id)
        {
            var selectList = _menuService.GetSelectList(id).Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            ViewBag.MenuList = selectList;

            ViewBag.IconList = LableHelper.GetLableList();

            var menu = _menuService.GetById(id);
            if (menu == null)
                return View();

            var model = menu.MapTo<AddEditModel>();
            return View(model);
        }

        /// <summary>
        /// 添加编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEdit(AddEditModel model)
        {
            var isNameExists = _menuService.IsNameExists(model.Id, model.Name);
            if (isNameExists.Failed)
                return Json(isNameExists);

            var entity = model.MapTo<Menu>();
            if (model.Id.IsNullOrWhiteSpace())
                _menuService.Add(entity);
            else
                _menuService.Update(entity);

            return Json(ServiceResult.IsSuccess("操作成功"));

        }
        #endregion

        #region 下拉框
        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IActionResult MenuSelect(string value)
        {
            var list = _menuService.GetSelectList().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id.ToString() == value
            }); ;
            return Json(list);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Menu_Delete)]
        public IActionResult Delete(string id)
        {
            var isCanDelResult = _menuService.IsCanDelete(id);

            if (!isCanDelResult.Success)
                return Json(isCanDelResult);

            var delResult = _menuService.DeleteById(id);
            return Json(delResult);
        }
        #endregion

        #region 关联
        [HttpGet]
        public IActionResult Relate(string id)
        {
            ViewBag.Menu = _menuActionService.GetAllActionById(id);
            return View(new RelateModel
            {
                MenuId = id,
                MenuActionId = new string[] { }
            });
        }

        [HttpPost]
        [Permission(PermCode.Menu_Relate)]
        public IActionResult Relate(RelateModel param)
        {
            var result = _menuActionService.SetMenuAction(param.MenuId, param.MenuActionId);
            return Json(result);
        }
        #endregion
    }
}

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sikiro.Entity.System;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Web.Admin.Models.Department;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public class DepartmentController : BaseController
    {
        private readonly DepartmentService _departmentService;

        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        #region 列表页
        public IActionResult Index()
        {
            return View();
        }

        [Permission(PermCode.Department_List)]
        public IActionResult List()
        {
            var list = _departmentService.GetList().UpdateForPageListResult(a => new ListVo
            {
                Id = a.Id.ToString(),
                Name = a.Name,
                PId = a.ParentId?.ToString() ?? "",
                IsDepartment = true,
            });
            return PageList(list);
        }
        #endregion

        #region 添加编辑
        /// <summary>
        /// 添加或者编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Permission(PermCode.Department_AddEdit)]
        public IActionResult AddEdit(string id)
        {
            //部门下拉框
            var departmentSelet = _departmentService.GetSelectList(id).Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });

            ViewBag.DepartmentSelet = departmentSelet;

            if (id.IsNullOrWhiteSpace())
                return View();

            var department = _departmentService.GetById(id);
            if (department == null)
                return View();

            var model = department.MapTo<AddEditModel>();
            return View(model);
        }

        /// <summary>
        /// 添加或者编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddEdit(AddEditModel model)
        {
            var departmentName = _departmentService.Get(c => c.Name == model.Name);
            var entity = model.MapTo<Department>();
            if (model.Id.IsNullOrWhiteSpace())
            {

                if (departmentName != null)
                {
                    return Json(ServiceResult.IsFailed("部门名已存在"));
                }
                _departmentService.Add(entity);
            }
            else
            {
                if (departmentName != null)
                {
                    if (entity.Id != departmentName.Id)
                    {
                        return Json(ServiceResult.IsFailed("部门名已存在"));
                    }

                }
                _departmentService.Update(entity);
            }


            return Json(ServiceResult.IsSuccess("操作成功"));
        }
        #endregion

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Department_Delete)]
        public IActionResult Delete(string id)
        {
            var isCanDelResult = _departmentService.IsCanDelete(id);

            if (!isCanDelResult.Success)
                return Json(isCanDelResult);

            var delResult = _departmentService.DeleteById(id);
            return Json(delResult);
        }

        /// <summary>
        /// 部门下拉框
        /// </summary>
        /// <returns></returns>
        public IActionResult DepartmentSelect(string value)
        {
            var selectList = _departmentService.GetSelectList().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id.ToString() == value
            });

            return Json(selectList);
        }
    }
}
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Sikiro.Entity.System;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models.Company;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 企业列表
    /// </summary>
    public class CompanyController : BaseController
    {

        private readonly CompanyService _companyService;
        private readonly IConfiguration _iConfiguration;
        public CompanyController(CompanyService companyService,IConfiguration iConfiguration)
        {
            _companyService = companyService;
            _iConfiguration = iConfiguration;
        }


        public IActionResult Index()
        {
            return View();
        }
        [Permission(PermCode.Company_list)]
        public IActionResult List(Models.PageListParams<CompanyParams> model)
        {
            var where = ExpressionBuilder.Init<Company>();
            var param = model.Params;

            if (!param.CompanyName.IsNullOrEmpty())
                where = where.And(a => a.Name.Contains(param.CompanyName));


            var result = _companyService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => new CompanyModel
            {
                Id = a.Id.ToString(),
                Name = a.Name ?? "",
                ChatName = a.ChatName ?? "",
                ChatLogo = a.ChatLogo ?? "",
                Remark = a.Remark ?? "",
                CreateDateTime = a.CreateDateTime,
                PcUrl = _iConfiguration["CompanyPcUrl"] + "?CompanyId="+a.Id,
                MobileUrl = _iConfiguration["CompanyMobileUrl"] + "?CompanyId=" + a.Id
            });
            return PageList(result);
        }

        public IActionResult AddorEdit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var model = _companyService.Get(c => c.Id == id.ToObjectId());
                var result = model.MapTo<AddorEditCompany>();
                result.Id = model.Id.ToString();
                return View(result);
            }

        }
        [Permission(PermCode.Company_AddorEdit)]
        [HttpPost]
        public IActionResult AddorEditAJax(AddorEditCompany model)
        {
            var result = new ServiceResult();
            if (!string.IsNullOrEmpty(model.Id))
            {
                var comModel = _companyService.Get(c => c.Name == model.Name);
                if (comModel != null)
                {
                    if (comModel.Id != model.Id.ToObjectId())
                    {
                        return Json(ServiceResult.IsFailed("商户名已存在"));
                    }
                }

                var entity = model.MapTo<Company>();
                result = _companyService.Update(entity.Id.ToString(), entity);
            }
            else
            {
                var isExist = _companyService.Exists(c => c.Name == model.Name);
                if (isExist)
                {
                    return Json(ServiceResult.IsFailed("商户名已存在"));
                }

                var entity = model.MapTo<Company>();
                entity.CreateDateTime = DateTime.Now;
                entity.CompanyNo = _companyService.GetCompanyNo();
                result = _companyService.Add(entity);

            }

            return Json(result);
        }

        [Permission(PermCode.Company_Dele)]
        [HttpPost]
        public IActionResult DeleAJax(string id)
        {

            var json = _companyService.Delete(c => c.Id == id.ToObjectId());
            return Json(json);
        }


        /// <summary>
        /// 部门下拉框
        /// </summary>
        /// <returns></returns>
        public IActionResult CompanySelect(string value)
        {
            var selectList = _companyService.GetSelectList().Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name,
                Selected = a.Id.ToString() == value
            });

            return Json(selectList);
        }
    }
}
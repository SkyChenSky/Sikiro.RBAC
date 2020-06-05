using System;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models;
using Sikiro.Web.Admin.Models.SelfService;
using Sikiro.Web.Admin.Permission;
using PageListResultExtension = Sikiro.Tookits.Base.PageListResultExtension;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 自住服务
    /// </summary>
    public class SelfServiceController : BaseController
    {
        private readonly SelfServiceService _selfService;
        private readonly AdministratorService _administratorService;

        public SelfServiceController(SelfServiceService selfService, AdministratorService administratorService)
        {
            _selfService = selfService;
            _administratorService = administratorService;
        }



        public IActionResult Index()
        {
            return View();
        }
        [Permission(PermCode.SelfService_list)]
        public IActionResult List(PageListParams<SelfServicParams> model)
        {
            var where = ExpressionBuilder.Init<SelfService>();
            var param = model.Params;

            if (!param.ProductName.IsNullOrEmpty())
                where = where.And(a => a.ServiceName.Contains(param.ProductName));

            var result = PageListResultExtension.UpdateForPageListResult(_selfService.GetPageList(model.Page, model.Limit, @where), a => new SelfServiceModel
            {
                Id = a.Id.ToString(),
                CreateDateTime = a.CreateDateTime,
                ServiceName = a.ServiceName,
                ServiceLogo = a.ServiceLogo,
                Order = a.Order
            });
            return PageList(result);
        }


        public IActionResult Add()
        {
            return View();
        }
        [Permission(PermCode.SelfService_Add)]
        [HttpPost]
        public IActionResult AddAJax(AddSelfService model, string editorVulue)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<SelfService>();
            entity.CompanyId = companyId;
            entity.CreateDateTime = DateTime.Now;
            entity.Detail = editorVulue;
            var json = _selfService.Add(entity);
            return Json(json);
        }


        public IActionResult Edit(string id)
        {

            var model = _selfService.Get(c => c.Id == id.ToObjectId());
            var result = model.MapTo<EditSelfService>();
            result.Id = model.Id.ToString();
            return View(result);
        }
        [Permission(PermCode.SelfService_Edit)]
        [HttpPost]
        public IActionResult EditAJax(EditSelfService model,string editorVulue)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<SelfService>();
            entity.CompanyId = companyId; 
            entity.Detail = editorVulue;
            var json = _selfService.Update(entity.Id.ToString(), entity);
            return Json(json);
        }

        [Permission(PermCode.SelfService_Dele)]
        [HttpPost]
        public IActionResult DeleAJax(string id)
        {

            var json = _selfService.Delete(c => c.Id == id.ToObjectId() );
            return Json(json);
        }
    }
}
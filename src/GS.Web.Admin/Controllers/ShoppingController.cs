using System;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models;
using Sikiro.Web.Admin.Models.Shopping;
using Sikiro.Web.Admin.Permission;
using PageListResultExtension = Sikiro.Tookits.Base.PageListResultExtension;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 微商城
    /// </summary>
    public class ShoppingController : BaseController
    {
        private readonly ShoppingService _shoppingService;
        private readonly AdministratorService _administratorService;
        public ShoppingController(ShoppingService shoppingService, AdministratorService administratorService)
        {
            _shoppingService = shoppingService;
            _administratorService =administratorService;
        }


        public IActionResult Index()
        {
            return View();
        }
        [Permission(PermCode.Shopping_list)]
        public IActionResult List(PageListParams<ShoppingParams> model)
        {
            var where = ExpressionBuilder.Init<Shopping>();
            var param = model.Params;

            if (!param.ProductName.IsNullOrEmpty())
                where = where.And(a => a.ProductName.Contains(param.ProductName));


            var result = PageListResultExtension.UpdateForPageListResult(_shoppingService.GetPageList(model.Page, model.Limit, @where), a => new ShoppingModel
            {
                Id = a.Id.ToString(),
                CreateDateTime = a.CreateDateTime,
                ProductName = a.ProductName,
                ProductLogo = a.ProductLogo,
                Item1 = a.Item1 + "：" + a.Item1Val,
                Item1Val = a.Item1Val,
                Item2 = a.Item2 + "：" + a.Item2Val,
                Item2Val = a.Item2Val,
                Order = a.Order,
                GoUrl = a.GoUrl

            });
            return PageList(result);
        }


        public IActionResult Add()
        {

            return View();
        }
        [Permission(PermCode.Shopping_Add)]
        [HttpPost]
        public IActionResult AddAJax(AddShopping model, string editorVulue)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<Shopping>();
            entity.CompanyId = companyId;
            entity.CreateDateTime = DateTime.Now;
            var json = _shoppingService.Add(entity);
            return Json(json);
        }


        public IActionResult Edit(string id)
        {

            var model = _shoppingService.Get(c => c.Id == id.ToObjectId());
            var result = model.MapTo<EditShopping>();
            result.Id = model.Id.ToString();
            return View(result);
        }

        [Permission(PermCode.Shopping_Edit)]
        [HttpPost]
        public IActionResult EditAJax(EditShopping model, string imgLogo, string id)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<Shopping>();
            entity.CompanyId = companyId;
            entity.Id = id.ToObjectId();
            var json = _shoppingService.Update(id, entity);
            return Json(json);
        }

        [Permission(PermCode.Shopping_Dele)]
        [HttpPost]
        public IActionResult DeleAJax(string id)
        {

            var json = _shoppingService.Delete(c => c.Id == id.ToObjectId());
            return Json(json);
        }



 
    }
}
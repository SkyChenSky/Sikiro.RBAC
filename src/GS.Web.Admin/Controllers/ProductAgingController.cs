using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models;
using Sikiro.Web.Admin.Models.ProductAging;
using Sikiro.Web.Admin.Permission;
using PageListResultExtension = Sikiro.Tookits.Base.PageListResultExtension;

namespace Sikiro.Web.Admin.Controllers
{

    /// <summary>
    /// 产品时效
    /// </summary>
    public class ProductAgingController : BaseController
    {
        private readonly ProductService _productService;
        private readonly AdministratorService _administratorService;

        public ProductAgingController(ProductService productService, AdministratorService administratorService)
        {
            _productService = productService;
            _administratorService =administratorService;
        }


        public IActionResult Index()
        {
            return View();
        }
        [Permission(PermCode.ProductAging_list)]
        public IActionResult List(PageListParams<ProductParams> model)
        {
            var where = ExpressionBuilder.Init<Product>();
            var param = model.Params;

            if (!param.ProductName.IsNullOrEmpty())
                where = where.And(a => a.ProductName.Contains(param.ProductName));


            var result = PageListResultExtension.UpdateForPageListResult(_productService.GetPageList(model.Page, model.Limit, @where), a => new ProductModel
            {
                Id = a.Id.ToString(),
                CreateDateTime = a.CreateDateTime,
                ProductName = a.ProductName,
                ProductLogo = a.ProductLogo,
                Item1 = a.Item1 + "：" + a.Item1Val,
                Item1Val = a.Item1Val,
                Item2 = a.Item2 + "：" + a.Item2Val,
                Item2Val = a.Item2Val,
                //Detail=a.Detail,
                Order = a.Order
            });
            return PageList(result);
        }


        public IActionResult Add()
        {
            return View();
        }
        [Permission(PermCode.ProductAging_Add)]
        [HttpPost]
        public IActionResult AddAJax(AddProduct model, string editorVulue)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<Product>();
            entity.CompanyId = companyId;
            entity.CreateDateTime = DateTime.Now;
            entity.Detail = editorVulue;
            var json = _productService.Add(entity);
            return Json(json);
        }


        public IActionResult Edit(string id)
        {
            var model = _productService.Get(c => c.Id == new ObjectId(id));
            var result = model.MapTo<EditProduct>();
            result.Id = model.Id.ToString();
            return View(result);
        }
        [Permission(PermCode.ProductAging_Edit)]
        [HttpPost]
        public IActionResult EditAJax(EditProduct model,string editorVulue)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<Product>();
            entity.CompanyId = companyId;
            entity.Detail = editorVulue;
            var json = _productService.Update(entity.Id.ToString(), entity);
            return Json(json);
        }
        [Permission(PermCode.ProductAging_Dele)]
        [HttpPost]
        public IActionResult DeleAJax(string id)
        {

            var json = _productService.Delete(c => c.Id == id.ToObjectId());
            return Json(json);
        }

    }
}
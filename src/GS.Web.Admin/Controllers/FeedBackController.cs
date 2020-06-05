using Microsoft.AspNetCore.Mvc;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models;
using Sikiro.Web.Admin.Models.FeedBack;
using Sikiro.Web.Admin.Permission;
using PageListResultExtension = Sikiro.Tookits.Base.PageListResultExtension;

namespace Sikiro.Web.Admin.Controllers
{
    public class FeedBackController : BaseController
    {

        private readonly FeedBackService _feedBackService;
        private readonly UserService _userService;
        private readonly AdministratorService _administratorService;
        public FeedBackController(FeedBackService feedBackService, UserService userService,AdministratorService administratorService)
        {
            _feedBackService = feedBackService;
            _userService = userService;
            _administratorService =administratorService;
        }
        [Permission(PermCode.FeedBack_list)]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(PageListParams<FeedBackParams> model)
        {
            var where = ExpressionBuilder.Init<FeedBack>();
            var param = model.Params;

            if (!param.Desc.IsNullOrEmpty())
                where = where.And(a => a.Desc.Contains(param.Desc));



            var result = PageListResultExtension.UpdateForPageListResult(_feedBackService.GetPageList(model.Page, model.Limit, @where), a => new FeedBackModel
            {
                UserName = _userService.Get(c => c.Id == a.UserId).UserName,
                Id = a.Id.ToString(),
                Desc = a.Desc,
                Contact = a.Contact,
                Reply = a.Reply,
                FeedBackTime = a.FeedBackTime,
                ReplyTime = a.ReplyTime
            });
            return PageList(result);

        }


        public IActionResult Edit(string id)
        {


            var model = _feedBackService.Get(c => c.Id ==id.ToObjectId());
            var result = model.MapTo<EditFeedBack>();
            return View(result);
        }


        [Permission(PermCode.FeedBack_Edit)]
        [HttpPost]
        public IActionResult EditAJax(string id, EditFeedBack model)
        {

            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<FeedBack>();
            entity.CompanyId = companyId;

            var json = _feedBackService.Update(id, entity);
            return Json(json);
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sikiro.Common.Utils;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models;
using Sikiro.Web.Admin.Models.ChatReply;
using Sikiro.Web.Admin.Permission;
using ObjectExtension = Sikiro.Common.Utils.ObjectExtension;
using PageListResultExtension = Sikiro.Tookits.Base.PageListResultExtension;

namespace Sikiro.Web.Admin.Controllers
{

    /// <summary>
    /// 预设计回复
    /// </summary>
    public class ChatReplyController : BaseController
    {
        private readonly ChatReplyService _chatReplyService;
        private readonly AdministratorService _administratorService;

        public ChatReplyController(ChatReplyService chatReplyService, AdministratorService administratorService)
        {
            _chatReplyService = chatReplyService;
            _administratorService = administratorService;
        }
        #region 列表
        [HttpGet]
       
        public IActionResult Index()
        {

            return View();
        }
        [Permission(PermCode.ChatReply_List)]
        public IActionResult List(PageListParams<object> model)
        {
            var where = ExpressionBuilder.Init<ChatReply>();
            var param = model.Params;


            where = where.And(a => a.ParentId != AccountHelper.AllPerson.ToObjectId());
            var result = PageListResultExtension.UpdateForPageListResult(_chatReplyService.GetPageList(model.Page, model.Limit, @where), a => new ChatReplyModel()
            {
                Code = a.Code ?? "",
                CompanyId = a.CompanyId,
                Id = a.Id.ToString(),
                News = a.News,
                ChatReplyType = _chatReplyService.GetById(a.ParentId.ToString()) != null ? _chatReplyService.GetById(a.ParentId.ToString()).News : "",
                Status = ObjectExtension.GetDisplayName(a.Status),
                CreateDateTime = a.CreateDateTime,
                Order = a.Order


            });
            return PageList(result);
        }
        #endregion
        #region 添加预设回复
        [HttpGet]
      
        public IActionResult Add()
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var typeList = _chatReplyService.GetList(c => c.ParentId == AccountHelper.AllPerson.ToObjectId() && c.CompanyId == companyId);
            List<SelectListItem> lisItem = new List<SelectListItem>();
            if (typeList != null && typeList.Count > 0)
            {
                lisItem = typeList.Select(c => new SelectListItem { Text = c.News, Value = c.Id.ToString() }).ToList();
            }

            ViewBag.lisItem = lisItem;
            return View();
        }


        [HttpPost]
        [Permission(PermCode.ChatReply_Add)]
        public IActionResult Add(ChatReplyEditModel model)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<ChatReply>();
            entity.CompanyId = companyId;
            entity.CreateDateTime = DateTime.Now;
            var result = _chatReplyService.Add(entity);
            return Json(result);
        }

        #endregion
        #region 添加类型

        public IActionResult AddType()
        {
            return View();
        }
        [Permission(PermCode.ChatReply_TypeAdd)]
        public IActionResult AddTypeAjax(AddChatReplyType model)
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var entity = model.MapTo<ChatReply>();
            entity.Order = 0;
            entity.CompanyId = companyId;
            entity.CreateDateTime = DateTime.Now;
            entity.ParentId = "".ToObjectId();
            entity.Status = ChatReplyStatus.Open;
            var result = _chatReplyService.Add(entity);
            return Json(result);
        }

        #endregion
        #region 编辑预设回复
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var admin = _chatReplyService.GetById(id);
            var result = admin.MapTo<ChatReplyEditModel>();
            result.Id = admin.Id.ToString();
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var typeList = _chatReplyService.GetList(c => c.ParentId == AccountHelper.AllPerson.ToObjectId() && c.CompanyId == companyId);

            ViewBag.lisItem = typeList.Select(c => new SelectListItem { Text = c.News, Value = c.Id.ToString(), Selected = c.ParentId == admin.ParentId }).ToList();

            return View(result);
        }

        [HttpPost]
        [Permission(PermCode.ChatReply_Edit)]
        public IActionResult Edit(ChatReplyEditModel edit)
        {
            var entity = edit.MapTo<ChatReply>();
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            entity.CompanyId = companyId;
            entity.Id = edit.Id.ToObjectId();
            entity.ParentId = edit.ParentId.ToObjectId();

            var result = _chatReplyService.UpdateById(entity.Id, entity);

            return Json(result);
        }
        #endregion
        #region 删除和批量删除

        [HttpPost]

        [Permission(PermCode.ChatReply_Dele)]
        public IActionResult DeleteAJax(string id)
        {


            var json = _chatReplyService.Delete(c => c.Id == id.ToObjectId());
            return Json(json);
        }

        [HttpPost]
        [Permission(PermCode.ChatReply_batchDele)]
        public IActionResult BatchDeleteAJax(List<string> ids)
        {

            var objIds = ids.Select(c => c.ToObjectId()).ToList();
            var json = _chatReplyService.Delete(c => objIds.Contains(c.Id));
            return Json(json);
        }
        #endregion
    }
}
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
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;
using Sikiro.Web.Admin.Models.Info;
using Sikiro.Web.Admin.Models.User;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 站内信
    /// </summary>
    public class InfoController : BaseController
    {
        private readonly UserService _userService;
        private readonly InfoService _infoService;
        private readonly ChatService _chatService;
        private readonly AdministratorService _administratorService;
        public InfoController(UserService userServic, InfoService infoService, ChatService chatService, AdministratorService administratorService)
        {
            _userService = userServic;
            _infoService = infoService;
            _chatService = chatService;
            _administratorService =administratorService;
        }


        /// <summary>
        /// 用户信息 
        /// </summary>
        /// <returns></returns>

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///用户信息-查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [Permission(PermCode.Info_list)]
        public IActionResult List(Models.PageListParams<UserParams> model, string userName, DateTime? date)
        {
            var where = ExpressionBuilder.Init<User>();

            if (!string.IsNullOrEmpty(userName))
            {

             where = where.And(a => a.UserName.ToLower().Contains(userName.ToLower()) || a.WxName.ToLower().Contains(userName.ToLower()) || a.Phone.Contains(userName));

            }
  
            if (date != null)
            {
                where = where.And(a => a.CreateDateTime >= date && a.CreateDateTime < date.Value.AddDays(1));
            }
            var result = _userService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => new UserModel
            {
                Id = a.Id.ToString(),
                UserName = !string.IsNullOrEmpty(a.UserName) ? a.UserName : "",
                NickName = a.NickName,
                OpenId = !string.IsNullOrEmpty(a.OpenId) ? a.OpenId : "",
                Phone = !string.IsNullOrEmpty(a.Phone) ? a.Phone : "",
                UserStatus = (int)a.Status,
                Progress = AccountHelper.GetProgress(a.UserName, a.Phone, a.OpenId),
                UserLogo = !string.IsNullOrEmpty(a.UserLogo) ? a.UserLogo : "",
            });
            return PageList(result);
        }


        /// <summary>
        /// 用户信息-最近联系人列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [Permission(PermCode.Relation_list)]
        public IActionResult RelationList(Models.PageListParams<UserParams> model, int? date)
        {
            var where = ExpressionBuilder.Init<NearlyChat>();
            var param = model.Params;

            if (date == (int)EnumDate.今天)
            {
                var start = DateTime.Now.Date;
                var end = DateTime.Now.Date.AddDays(1);
                where = where.And(a => a.CreateDateTime >= start && a.CreateDateTime < end);
            }
            if (date == (int)EnumDate.昨天)
            {

                var start = DateTime.Now.Date.AddDays(-1);
                var end = DateTime.Now.Date;
                where = where.And(a => a.CreateDateTime >= start && a.CreateDateTime < end);
            }
            if (date == (int)EnumDate.本周)
            {
                var start = AccountHelper.GetWeekFirstDayMon(DateTime.Now);
                var end = AccountHelper.GetWeekLastDaySun(DateTime.Now).Date.AddDays(1);
                where = where.And(a => a.CreateDateTime >= start && a.CreateDateTime < end);
            }
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            if (!string.IsNullOrEmpty(CurrentUserData.CompanyId))
                where = where.And(a => a.CompanyId == companyId);


            var result = _chatService.GetNearlyChatList(model.Page, model.Limit, where);
            List<UserModel> userList = new List<UserModel>();
            if (result != null && result.Items != null)
            {
                var list = result.Items.Select(c => c.CustomId).ToList();
                var dataList = _userService.GetList(c => list.Contains(c.Id)).ToList();
                userList = dataList.Select(a => new UserModel
                {
                    Id = a.Id.ToString(),
                    UserName = a.UserName ?? "",
                    UserLogo = a.UserLogo ?? "",
                    OpenId = a.OpenId ?? "",
                    Phone = a.Phone ?? "",
                    NickName = a.NickName,
                    Progress = AccountHelper.GetProgress(a.UserName ?? "", a.Phone ?? "", a.OpenId ?? ""),

                }).ToList();
            }

            PageListResult pList = new PageListResult();
            pList.Data = userList;
            pList.Count = (int)_chatService.GetNearlyChatCount(a =>a.CompanyId== companyId);
            return PageList(pList);
        }

        public enum EnumDate
        {
            今天 = 0, 昨天 = 1, 本周 = 2

        }


        /// <summary>
        ///查询列表- 停止
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Permission(PermCode.Info_Stop)]
        public IActionResult StopAJax(string id)
        {

            var json = _userService.Stop(c => c.Id == id.ToObjectId());
            return Json(json);
        }

        [Permission(PermCode.Info_Open)]
        /// <summary>
        /// 查询列表- 启用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult OpenAJax(string id)
        {

            var json = _userService.Open(c => c.Id == id.ToObjectId());
            return Json(json);
        }




        /// <summary>
        /// 站内信息
        /// </summary>
        /// <returns></returns>
        public IActionResult news()
        {
            return View();

        }


        /// <summary>
        /// 站内信息-用户列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Permission(PermCode.Info_news)]
        public IActionResult newsList(Models.PageListParams<InfoParams> model)
        {
            var where = ExpressionBuilder.Init<Info>();
            var param = model.Params;

            if (!param.Desc.IsNullOrEmpty())
                where = where.And(a => a.Desc.Contains(param.Desc));


            var result = _infoService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => new InfoModel
            {
                Desc = a.Desc,
                Title = a.Title,
                UserName = a.ToUser.ToString() != AccountHelper.AllPerson ? _userService.Get(c => c.Id == a.ToUser) != null ? _userService.Get(c => c.Id == a.ToUser).UserName : "" : "",
                CreateDate = a.CreateDateTime
            });
            return PageList(result);
        }

        /// <summary>
        /// 站内信息-回复
        /// </summary>
        /// <returns></returns>

        public IActionResult Add()
        {
            ViewBag.AllPerson = AccountHelper.AllPerson;
            ViewBag.Sendlist = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "所有人",
                    Value = AccountHelper.AllPerson
                },
                new SelectListItem
                {
                    Text = "用户",
                    Value = "1"
                }
            };
            return View();
        }
        [Permission(PermCode.Info_Add)]
        [HttpPost]
        public IActionResult AddAJax(AddInfo model, string toUserId, string hiddenName)
        {
            var entity = model.MapTo<Info>();
            if (AccountHelper.AllPerson == model.sendlist)
            {
                entity.ToUser = "".ToObjectId();
            }
            else
            {

                if (string.IsNullOrEmpty(hiddenName))
                {
                    return Json(ServiceResult.IsFailed("请选择用户"));
                }
                entity.ToUser = hiddenName.ToObjectId();//model.userList.ToObjectId();
            }
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            entity.CompanyId = companyId;
            entity.Type = 0;
            entity.CreateDateTime = System.DateTime.Now;
            var json = _infoService.Add(entity);
            return Json(json);
        }

        /// <summary>
        /// 站内信息-回复的用户列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IActionResult UserList(Models.PageListParams<UserParams> model, string userName)
        {
            var where = ExpressionBuilder.Init<User>();
            var param = model.Params;

            if (!string.IsNullOrEmpty(userName))
                where = where.And(a => a.UserName.Contains(userName));


            var result = _userService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => new UserModel
            {
                Id = a.Id.ToString(),
                UserName = !string.IsNullOrEmpty(a.UserName) ? a.UserName : "",
                NickName = a.NickName,
                OpenId = !string.IsNullOrEmpty(a.OpenId) ? a.OpenId : "",
                Phone = !string.IsNullOrEmpty(a.Phone) ? a.Phone : "",
                UserStatus = (int)a.Status,
                Progress = AccountHelper.GetProgress(a.UserName, a.Phone, a.OpenId)
            });
            return PageList(result);
        }
    }
}
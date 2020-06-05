using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Common.Utils;
using Sikiro.Entity.Admin;
using Sikiro.Service.Admin;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;
using Sikiro.Web.Admin.Models.Form;
using Sikiro.Web.Admin.Models.Home;

namespace Sikiro.Web.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly FormService _formService;
        private readonly ChatService _chatService;
        private readonly MenuService _menuService;
        public HomeController(FormService formService, ChatService chatService, MenuService menuService)
        {
            _formService = formService;
            _chatService = chatService;
            _menuService = menuService;
        }

        public IActionResult Index()
        {
            ViewBag.RealName = CurrentUserData.RealName;

            var menu = _menuService.GetMenuInAdmin(CurrentUserData.UserId);

            return View(menu);
        }

        /// <summary>
        /// 获取表单详情
        /// </summary>
        /// <returns></returns>
        public IActionResult IndexPage()
        {
            List<HomeFormVo> homeFormList = new List<HomeFormVo>();
            var toplist = _formService.GetTopList(8);
            if (toplist.Any())
            {
                foreach (var item in toplist)
                {
                    var formValue = item.Value.FromJson<FormValue>();
                    homeFormList.Add(new HomeFormVo
                    {

                        FormNo = formValue.FormNo,
                        CreateTime = item.CreateDateTime,
                        UserId = !string.IsNullOrEmpty(formValue.UserId) ? formValue.UserId.ToString() : "",
                        TypeName = item.Type.GetDisplayName(),
                        StatusName = item.Status.GetDisplayName(),
                        UserName = !string.IsNullOrEmpty(item.AdministratorName) ? item.AdministratorName : "",
                        OrderNo = !string.IsNullOrEmpty(formValue.OrderNo) ? formValue.OrderNo : ""
                    });
                }
            }
            var dayStart = DateTime.Now.Date;
            var dayEnd = DateTime.Now.Date.AddDays(1);
            var yesterStart = DateTime.Now.Date.AddDays(-1);
            var yesterEnd = DateTime.Now.Date;
            ViewBag.nearlyChatDayCount = _chatService.GetNearlyChatCount(c => c.CreateDateTime >= dayStart && c.CreateDateTime < dayEnd);
            ViewBag.nearlyChatYesterdayCount = _chatService.GetNearlyChatCount(c => c.CreateDateTime >= yesterStart && c.CreateDateTime < yesterEnd);
            ViewBag.formCount = _formService.Count();
            ViewBag.toplist = homeFormList;
            ViewBag.dateName = AccountHelper.GetDateName();
            ViewBag.UserName = CurrentUserData.RealName;
            return View();
        }

        /// <summary>
        /// 获取操作表单的状态
        /// </summary>
        /// <returns></returns>
        public IActionResult ListFormData()
        {
            var toplist = _formService.GetTopList(8).Select(c => new HomeByFormDetail
            {
                AdministratorName = c.AdministratorName ?? "",
                CreateDateTime = c.CreateDateTime,
                Status = c.Status.GetDisplayName(),
                StatusInt = c.Status,
                FormId = c.Id,
                FormName = c.Type.GetDisplayName()
            }).ToList();

            return Json(new { list = toplist });
        }

        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Error(bool isShowButton, string value = null)
        {
            ViewBag.ErrorMessage = value ?? "服务器正在开小差~请稍后重试";
            ViewBag.isShowButton = isShowButton;
            return View();
        }
    }
}

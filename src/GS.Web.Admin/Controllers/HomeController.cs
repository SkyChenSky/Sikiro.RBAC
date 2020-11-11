using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using Sikiro.Service.System;

namespace Sikiro.Web.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MenuService _menuService;
        public HomeController(MenuService menuService)
        {
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
            ViewBag.UserName = CurrentUserData.RealName;
            ViewBag.FrameworkVersion = DependencyContext.Default.Target.Framework;

            return View();
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

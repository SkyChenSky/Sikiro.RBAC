using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using Sikiro.Common.Utils;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Web.Admin.Models.Account;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 账户管理 test
    /// </summary>
    ///
    public class AccountController : BaseController
    {
        private readonly AdministratorService _administratorService;
        private readonly IConfiguration _iConfiguration;

        public AccountController(AdministratorService administratorService, IConfiguration iConfiguration)
        {
            _administratorService = administratorService;
            _iConfiguration = iConfiguration;
        }

        #region 登录
        [AllowAnonymous, HttpGet]
        public IActionResult Logon(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous, HttpPost]
        public IActionResult Logon(LogonModel registerModel)
        {
            var result = _administratorService.LogonCheck(registerModel.UserName, registerModel.Password);

            if (result.Success)
            {
                var admin = (AdministratorData)result.Data;
                SignIn(admin);
            }

            return Json(result);
        }

        private void SignIn(AdministratorData userData)
        {
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userData.UserId),
                new Claim(ClaimTypes.Name, userData.UserName??""),
                new Claim(ClaimTypes.UserData, userData.ToJson())
            }, "Basic"));

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddDays(7),
                    IsPersistent = true,
                    AllowRefresh = true
                });
        }
        #endregion

        #region 注销
        [AllowAnonymous, HttpGet]
        public IActionResult Logout()
        {
            if (CurrentUserData != null)
            {
                HttpContext.SignOutAsync();
            }

            return RedirectToAction("Logon");
        }

        #endregion

        #region 获取基本信息
        /// <summary>
        /// 获取基本信息
        /// </summary>
        /// <returns></returns>
        public IActionResult GetInfo()
        {
            var result = _administratorService.GetById(CurrentUserData.UserId);
            if (result != null)
            {
                return Json(new
                {
                    NickName = result.RealName,
                    customServiceId = CurrentUserData.UserId,
                    Logo = "/upload/userlogo.jpg",
                    Host = _iConfiguration["ImServerUrl"]
                });
            }

            return Json(null);
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码页
        /// </summary>
        /// <returns></returns>
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdatePassByUserId(ChangePasswordByUserId registerModel)
        {
            if (!string.IsNullOrEmpty(CurrentUserData.UserId) && CurrentUserData != null)
            {
                var userId = CurrentUserData.UserId.ToObjectId();
                var result = _administratorService.ChangePassword(userId, registerModel.OldPassword, registerModel.NewPassword);
                return Json(result);
            }

            return Json(ServiceResult.IsFailed("用户不存在"));
        }

        #endregion
    }
}
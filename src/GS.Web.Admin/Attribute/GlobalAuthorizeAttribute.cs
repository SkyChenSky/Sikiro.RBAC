using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Common.Utils;
using Sikiro.Entity.System;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Attribute
{
    /// <summary>
    /// 全局的访问权限控制
    /// </summary>
    public class GlobalAuthorizeAttribute : System.Attribute, IAuthorizationFilter
    {
        #region 初始化
        private string _currentUrl;
        private string _unauthorizedMessage;
        private readonly List<string> _noCheckPage = new List<string> { "home/index", "home/indexpage", "/" };

        private readonly AdministratorService _administratorService;
        private readonly MenuService _menuService;

        public GlobalAuthorizeAttribute(AdministratorService administratorService, MenuService menuService)
        {
            _administratorService = administratorService;
            _menuService = menuService;
        } 
        #endregion

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.ThrowIfNull();

            _currentUrl = PermissionUtil.CurrentUrl(context.HttpContext);

            //不需要验证登录的直接跳过
            if (context.Filters.Count(a => a is AllowAnonymousFilter) > 0)
                return;

            var user = GetCurrentUser(context);
            if (user == null)
            {
                if (_noCheckPage.Contains(_currentUrl))
                    return;

                _unauthorizedMessage = "登录失效";

                if (context.HttpContext.Request.IsAjax())
                    NoUserResult(context);
                else
                    LogoutResult(context);
                return;
            }

            //超级管理员跳过
            if (user.IsSuper)
                return;

            //账号状态判断
            var administrator = _administratorService.GetById(user.UserId);
            if (administrator != null && administrator.Status != EAdministratorStatus.Normal)
            {
                if (_noCheckPage.Contains(_currentUrl))
                    return;

                _unauthorizedMessage = "亲~您的账号已被停用，如有需要请您联系系统管理员";

                if (context.HttpContext.Request.IsAjax())
                    AjaxResult(context);
                else
                    AuthResult(context, 403, GoErrorPage(true));

                return;
            }

            if (_noCheckPage.Contains(_currentUrl))
                return;

            var userUrl = _administratorService.GetUserCanPassUrl(user.UserId);

            // 判断菜单访问权限与菜单访问权限
            if (IsMenuPass(userUrl) && IsActionPass(userUrl))
                return;

            if (context.HttpContext.Request.IsAjax())
                AuthResult(context, 200, GetJsonResult());
            else
                AuthResult(context, 403, GoErrorPage());
        }

        private JsonResult GetJsonResult()
        {
            return new JsonResult(ServiceResult.IsFailed(_unauthorizedMessage));
        }

        private RedirectToActionResult GoErrorPage(bool isShowButton = false)
        {
            return new RedirectToActionResult("error", "home", new
            {
                value = _unauthorizedMessage,
                isShowButton = isShowButton
            });
        }

        /// <summary>
        /// 判断功能操作权限
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        private bool IsMenuPass(string[] urls)
        {
            var menuUrl = _menuService.GetAllMenuUrl();
            //不在系统菜单列表的不需要校验
            if (!menuUrl.Contains(_currentUrl))
                return true;

            return IsCheckPass(urls);
        }

        /// <summary>
        /// 判断功能操作权限
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        private bool IsActionPass(string[] urls)
        {
            //没标识PermissionAttribute不需要做校验
            if (!PermissionUtil.PermissionUrls.ContainsKey(_currentUrl))
                return true;

            return IsCheckPass(urls);
        }

        /// <summary>
        /// 是否拥有该地址访问权限
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        private bool IsCheckPass(string[] urls)
        {
            if (urls.Contains(_currentUrl))
                return true;

            _unauthorizedMessage = "您没有此次操作的访问权限";
            return false;
        }

        private static void AuthResult(AuthorizationFilterContext httpContext, int statusCode, IActionResult result = null)
        {
            httpContext.HttpContext.Response.StatusCode = statusCode;
            if (result != null)
                httpContext.Result = result;
        }

        private static void LogoutResult(AuthorizationFilterContext httpContext)
        {
            httpContext.HttpContext.Response.StatusCode = 307;
            httpContext.Result = new RedirectToActionResult("Logon", "account", null); ;
        }

        private static void NoUserResult(AuthorizationFilterContext httpContext)
        {
            httpContext.HttpContext.Response.StatusCode = 200;
            httpContext.Result = new JsonResult(ServiceResult.IsFailed("登陆失效"));
        }

        private void AjaxResult(AuthorizationFilterContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            context.Result = new JsonResult(ServiceResult.IsError(_unauthorizedMessage, new
            {
                status = 403
            }));
        }

        private static AdministratorData GetCurrentUser(AuthorizationFilterContext context)
        {
            return context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value.FromJson<AdministratorData>();
        }
    }
}

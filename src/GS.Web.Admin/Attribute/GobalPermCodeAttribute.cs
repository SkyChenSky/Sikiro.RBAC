using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Common.Utils;
using Sikiro.Service.System;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Attribute
{
    /// <summary>
    /// 全局权限值验证
    /// </summary>
    public class GobalPermCodeAttribute : IActionFilter
    {
        private readonly AdministratorService _administratorService;

        public GobalPermCodeAttribute(AdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        private static AdministratorData GetCurrentUser(HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value.FromJson<AdministratorData>();
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            ((Controller)context.Controller).ViewData["PermCodes"] = new List<int>();

            if (context.HttpContext.Request.IsAjax())
                return;

            var user = GetCurrentUser(context.HttpContext);
            if (user == null)
                return;

            if (user.IsSuper)
                return;

            ((Controller)context.Controller).ViewData["PermCodes"] = _administratorService.GetActionCode(user.UserId).ToList();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}

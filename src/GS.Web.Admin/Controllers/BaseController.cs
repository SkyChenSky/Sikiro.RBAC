using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        protected AdministratorData CurrentUserData => GetCurrentUser();

        private AdministratorData GetCurrentUser()
        {
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value.FromJson<AdministratorData>();
        }

        /// <summary>
        /// 分页返回
        /// </summary>
        /// <param name="pageListResult"></param>
        /// <returns></returns>
        public JsonResult PageList(PageListResult pageListResult)
        {
            return Json(pageListResult);
        }
    }
}

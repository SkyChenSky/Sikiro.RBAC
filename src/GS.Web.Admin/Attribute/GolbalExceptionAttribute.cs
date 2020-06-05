using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.Web.Admin.Attribute
{
    /// <summary>
    /// 全局异常捕获
    /// </summary>
    public class GolbalExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var exception = context.Exception.GetDeepestException();
                exception.WriteToFile("全局异常捕抓");

                if (context.HttpContext.Request.IsAjax())
                {
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(ServiceResult.IsError(exception));
                }
                else
                {
                    base.OnException(context);
                }
            }
        }
    }
}

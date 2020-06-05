using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Tookits.Base;

namespace Sikiro.Web.Admin.Attribute
{
    /// <summary>
    /// 全局模型验证
    /// </summary>
    public class GobalModelValidAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var builder = new StringBuilder();

            if (context.ModelState.IsValid)
                return;

            foreach (var key in context.ModelState.Keys)
            {
                var errors = context.ModelState[key].Errors;

                foreach (var error in errors)
                {
                    if (builder.Length > 0)
                        builder.Append("<br/>");

                    builder.Append(error.ErrorMessage);
                }
            }

            var msg = builder.ToString();
            if (!string.IsNullOrEmpty(msg))
                context.Result = new JsonResult(ServiceResult.IsFailed(builder.ToString()));

            base.OnActionExecuting(context);
        }
    }
}

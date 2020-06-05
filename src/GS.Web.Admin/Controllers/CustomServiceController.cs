using Microsoft.AspNetCore.Mvc;
using Sikiro.Common.Utils;
using Sikiro.Nosql.Mongo;
using Sikiro.Service.Admin;
using Sikiro.Service.Admin.Bo;
using Sikiro.Service.System;
using Sikiro.Tookits.Base;
using Sikiro.Web.Admin.Permission;

namespace Sikiro.Web.Admin.Controllers
{
    /// <summary>
    /// 客服
    /// </summary>
    public class CustomServiceController : BaseController
    {
        private readonly ChatReplyService _chatReplyService;
        private readonly CustomServiceService _customServiceService;
        private readonly AdministratorService _administratorService;


        public CustomServiceController(ChatReplyService chatReplyService, CustomServiceService customServiceService, AdministratorService administratorService)
        {
            _chatReplyService = chatReplyService;
            _customServiceService = customServiceService;
            _administratorService = administratorService;
        }

        /// <summary>
        /// 客服页面
        /// </summary>
        /// <returns></returns>
        [Permission(PermCode.CustomService_List)]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 预测回复列表
        /// </summary>
        /// <returns></returns>
        public IActionResult ChatReplyList()
        {
            var companyId = _administratorService.GetCompanyId(CurrentUserData.UserId.ToObjectId());
            var result = _chatReplyService.GetAllList(companyId.ToString());
            return Json(result);
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SubmitForm(SubmitFormBo submitFormModel)
        {
            var form = _customServiceService.AddForm(submitFormModel, CurrentUserData.UserId.ToObjectId());

            return Json(ServiceResult.IsSuccess("操作完成", new
            {
                Id = form.Id.ToString(),
                form.Type,
                TypeStr = form.Type.GetDisplayName()
            }));
        }

        /// <summary>
        /// 获取表单值
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetFormValueById(string formId)
        {
            var formValue = _customServiceService.GetFormValueByFormId(formId);

            return Json(ServiceResult.IsSuccess("操作完成", formValue));
        }

        /// <summary>
        /// 客服确认表单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ComfirForm(string formId)
        {
            _customServiceService.UpdateFormForStatus(formId, Entity.Admin.FormStatus.NoLook);
            return Json(ServiceResult.IsSuccess("操作完成"));
        }

        /// <summary>
        /// 客服确认表单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CancelForm(string formId)
        {
            _customServiceService.UpdateFormForStatus(formId, Entity.Admin.FormStatus.Cancel);
            return Json(ServiceResult.IsSuccess("操作完成"));
        }

        /// <summary>
        /// 设置客户备注
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetCustomerRemark(string customerId, string remark)
        {
            _customServiceService.SetUserChatRemark(customerId, remark);
            return Json(ServiceResult.IsSuccess("操作完成"));
        }
    }
}
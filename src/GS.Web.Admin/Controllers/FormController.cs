using System;
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
using Sikiro.Web.Admin.Models.Form;
using Sikiro.Web.Admin.Permission;
using ObjectExtension = Sikiro.Common.Utils.ObjectExtension;

namespace Sikiro.Web.Admin.Controllers
{
    public class FormController : BaseController
    {
        private readonly FormService _formService;
        private readonly FormDetailService _formDetailService;
        private readonly UserService _userService;
        private readonly AdministratorService _administratorService;
        public FormController(FormService formService,
            FormDetailService formDetailService,
            UserService userService,
            AdministratorService administratorService)
        {
            _formService = formService;
            _formDetailService = formDetailService;
            _userService = userService;
            _administratorService = administratorService;
        }

        /// <summary>
        /// 客服表单
        /// </summary>
        /// <returns></returns>

        public IActionResult Index()
        {
            return View();
        }

        [Permission(PermCode.Form_clientList)]
        public IActionResult List(Models.PageListParams<FormParams> model)
        {
            var where = ExpressionBuilder.Init<Form>();
            var param = model.Params;

            if (!param.FormNo.IsNullOrEmpty())
                where = where.And(a => a.FormNo.Contains(param.FormNo));

            if (param.Type.HasValue)
                where = where.And(a => a.Type == param.Type.Value);

            if (param.Status.HasValue)
                where = where.And(a => a.Status == param.Status.Value);

            where = where.And(a => a.Status != FormStatus.Cancel&& a.Status != FormStatus.Wait);

            var result = _formService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => new FormListVo
            {
                Id = a.Id.ToString(),
                FormNo = a.FormNo,
                Status = ObjectExtension.GetDisplayName(a.Status),
                Type = ObjectExtension.GetDisplayName(a.Type),
                UserName =a.AdministratorName?? "等待审核中"
            });
            return PageList(result);
        }



        /// <summary>
        /// 仓库表单
        /// </summary>

        public IActionResult HouseIndex()
        {
            return View();
        }
        [Permission(PermCode.Form_warehouseList)]
        public IActionResult HouseList(Models.PageListParams<FormParams> model)
        {
            var where = ExpressionBuilder.Init<Form>();
            var param = model.Params;

            if (!param.FormNo.IsNullOrEmpty())
                where = where.And(a => a.FormNo.Contains(param.FormNo));

            if (param.Type.HasValue)
                where = where.And(a => a.Type == param.Type.Value);

            if (param.Status.HasValue)
                where = where.And(a => a.Status == param.Status.Value);

            where = where.And(a => a.Status != FormStatus.Cancel && a.Status != FormStatus.Wait);

            var result = _formService.GetPageList(model.Page, model.Limit, where).UpdateForPageListResult(a => new FormListVo
            {
                Id = a.Id.ToString(),
                FormNo = a.FormNo,
                Status = a.Status.GetDisplayName(),
                Type = a.Type.GetDisplayName(),
                UserName = a.AdministratorName??"等待审核中"
            });
            return PageList(result);
        }


        /// <summary>
        /// 未查看，更改成已查看 非客服的操作
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>

        [Permission(PermCode.Form_warehouseOperation)]
        public IActionResult HouseEdit(string formId)
        {
            var model = _formService.Get(c => c.Id == formId.ToObjectId());
            bool isLook = false;
            var administratorId = CurrentUserData.UserId.ToObjectId();
            var administratorName = CurrentUserData.RealName;
            if (model.Status== FormStatus.NoLook)
            {
                _formService.UpdateByStatus(formId, FormStatus.NoLook, c => new Form()
                {
                    Status = FormStatus.Looked,
                    AdministratorId= administratorId,
                    AdministratorName= administratorName
                });
                isLook = true;
            }
            var formValue = model.Value.FromJson<FormValue>();
            var editModel = formValue.MapTo<EditFormModel>();
            editModel.Id = formId;
            editModel.Status = model.Status;
            ViewBag.feedbackList = _formDetailService.GetList(c => c.FormId == formId.ToObjectId());
            ViewBag.getFormStateAdress = AccountHelper.GetFormStateAdress().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.TargetCountry }).ToList();// 发货地址

            ViewBag.getFormStateClaimGoods = AccountHelper.GetFormStateClaimGoods().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.ClaimGoods }).ToList();//取货方式
            ViewBag.getFormAging = AccountHelper.GetFormAging().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.Aging }).ToList();//时效

            ViewBag.getPayment = AccountHelper.GetPayment().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.Payment }).ToList();//付款方式
            ViewBag.getTimeQuantum = AccountHelper.GetTimeQuantum().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.TimeQuantum }).ToList();//选择时间段
            ViewBag.getTimeSection = AccountHelper.GetTimeSection().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.TimeSection }).ToList();//选择时间区间
            ViewBag.isLook = isLook;
            ViewBag.administratorName = administratorName;
            return View(editModel);
        }








        /// <summary>
        /// 客服的操作
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>

        [Permission(PermCode.Form_clientOperation)]
        public IActionResult Edit(string formId)
        {

            var model = _formService.Get(c => c.Id == formId.ToObjectId());
            var formValue = model.Value.FromJson<FormValue>();
            var editModel = formValue.MapTo<EditFormModel>();
            editModel.Id = formId;
            editModel.Status = model.Status;
            ViewBag.feedbackList = _formDetailService.GetList(c => c.FormId == formId.ToObjectId());
            ViewBag.getFormStateAdress = AccountHelper.GetFormStateAdress().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.TargetCountry }).ToList();// 发货地址

            ViewBag.getFormStateClaimGoods = AccountHelper.GetFormStateClaimGoods().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.ClaimGoods }).ToList();//取货方式
            ViewBag.getFormAging = AccountHelper.GetFormAging().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.Aging }).ToList();//时效

            ViewBag.getPayment = AccountHelper.GetPayment().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.Payment }).ToList();//付款方式
            ViewBag.getTimeQuantum = AccountHelper.GetTimeQuantum().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.TimeQuantum }).ToList();//选择时间段
            ViewBag.getTimeSection = AccountHelper.GetTimeSection().Select(c => new SelectListItem { Text = c, Value = c, Selected = c == editModel.TimeSection }).ToList();//选择时间区间
            ViewBag.administratorName = CurrentUserData.RealName;
            return View(editModel);
        }

        /// <summary>
        /// 客服的操作和非客服的操作
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
       
        public IActionResult FromEditAJax(EditFormModel model, string feedback)
        {
            if (model.Operation == (int)FormStatus.Back)
            {
                if (string.IsNullOrEmpty(feedback))
                {
                    return Json(ServiceResult.IsFailed("请填写反馈内容"));
                }
                return SendBack(model, feedback);
            }

            if (model.Operation == (int)FormStatus.Confirmed)
            {
                return AlreadyAffirm(model, feedback);
            }
            if (model.Operation == (int)FormStatus.EndConfirm)
            {
                return EndForm(model, feedback);
            }
            if (model.Operation == 100)//客服点击发送
            {
                if (string.IsNullOrEmpty(feedback))
                {
                    return Json(ServiceResult.IsFailed("请填写反馈内容"));
                }
                return Send(model, feedback);
            }
            return Json(ServiceResult.IsFailed("操作失效"));
        }






        /// <summary>
        /// 已确认
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult AlreadyAffirm(EditFormModel model, string feedback)
        {
            var administratorId = CurrentUserData.UserId.ToObjectId();
            var administratorName = CurrentUserData.RealName;

            _formService.Update(model.Id, c => new Form()
            {
                Status = FormStatus.Confirmed,
                AdministratorId = administratorId,
                AdministratorName = administratorName
            });

            if (!string.IsNullOrEmpty(feedback))
            {
                _formDetailService.Add(new FormDetail()
                {
                    FormId = model.Id.ToObjectId(),
                    Remark = feedback ?? "",
                    AdministratorId = administratorId,
                    AdministratorName = administratorName,
                    Status = FormStatus.Confirmed,
                    CreateDateTime = DateTime.Now


                });
            }

            return Json(ServiceResult.IsSuccess("操作成功"));
        }
        /// <summary>
        /// 退回
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult SendBack(EditFormModel model, string feedback)
        {
            var administratorId = CurrentUserData.UserId.ToObjectId();
            var administratorName = CurrentUserData.RealName;
            _formService.Update(model.Id, c => new Form()
            {
                Status = FormStatus.Back,
                AdministratorId = administratorId,
                AdministratorName = administratorName

            });

            if (!string.IsNullOrEmpty(feedback))
            {
                _formDetailService.Add(new FormDetail()
                {
                    FormId = model.Id.ToObjectId(),
                    Remark = feedback ?? "",
                    AdministratorId = administratorId,
                    AdministratorName = administratorName,
                    Status = FormStatus.Back,
                    CreateDateTime = DateTime.Now

                });

            }
            return Json(ServiceResult.IsSuccess("操作成功"));
        }


        /// <summary>
        /// 结束
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult EndForm(EditFormModel model, string feedback)
        {
            var administratorId = CurrentUserData.UserId.ToObjectId();
            var administratorName = CurrentUserData.RealName;

            _formService.Update(model.Id, c => new Form()
            {
                Status = FormStatus.EndConfirm,
                AdministratorId = administratorId,
                AdministratorName = administratorName

            });
            if (!string.IsNullOrEmpty(feedback))
            {

                _formDetailService.Add(new FormDetail()
                {
                    FormId = model.Id.ToObjectId(),
                    Remark = feedback ?? "",
                    AdministratorId = administratorId,
                    AdministratorName = administratorName,
                    Status = FormStatus.EndConfirm,
                    CreateDateTime = DateTime.Now

                });

            }

            return Json(ServiceResult.IsSuccess("操作成功"));
        }



        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public IActionResult Send(EditFormModel model, string feedback)
        {
            if (!string.IsNullOrEmpty(feedback))
            {

                _formDetailService.Add(new FormDetail()
                {
                    FormId = model.Id.ToObjectId(),
                    Remark = feedback,
                    AdministratorId = CurrentUserData.UserId.ToObjectId(),
                    AdministratorName = CurrentUserData.RealName,
                    Status = model.Status,
                    CreateDateTime = DateTime.Now

                });

            }
            return Json(ServiceResult.IsSuccess("操作成功"));
        }

    }
}
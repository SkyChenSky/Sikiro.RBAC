using System;
using MongoDB.Bson;
using Sikiro.Common.Utils;
using Sikiro.Entity.Admin;
using Sikiro.Nosql.Mongo;
using Sikiro.Repository.Admin;
using Sikiro.Service.Admin.Bo;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.Service.Admin
{
    /// <summary>
    /// 客服
    /// </summary>
    public class CustomServiceService : IDepend
    {
        private readonly MongoRepository _mongoRepository;
        private readonly MongoRep _mongoRep;
        public CustomServiceService(MongoRepository mongoRepository, MongoRep mongoRep)
        {
            _mongoRepository = mongoRepository;
            _mongoRep =mongoRep;
        }

        /// <summary>
        /// 根据ID获取表单值
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        public SubmitFormBo GetFormValueByFormId(string formId)
        {
            var form = _mongoRepository.Get<Form>(a => a.Id == formId.ToObjectId());
            var fromValue = form?.Value?.FromJson<FormValue>();

            if (fromValue != null)
                return new SubmitFormBo
                {
                    Prescription = fromValue.Aging,
                    Brand = fromValue.Brand,
                    TakeOver = fromValue.ClaimGoods,
                    Date = fromValue.Date,
                    ProductName = fromValue.Description,
                    FormType = fromValue.FormType,
                    OrderId = fromValue.OrderNo,
                    Payer = fromValue.Payment,
                    SendAddress = fromValue.ShipAddress,
                    Destination = fromValue.TargetCountry,
                    TimeSlot = fromValue.TimeQuantum,
                    TimeZoom = fromValue.TimeSection,
                    UserId = fromValue.UserId,
                    Weight = fromValue.Weight,
                    Long = fromValue.Volume.Longth,
                    High = fromValue.Volume.Heigth,
                    Wide = fromValue.Volume.Wide,
                    Status = (int)form.Status,
                    StatusStr = form.Status.GetDisplayName()
                };

            return null;
        }

        /// <summary>
        /// 添加
        /// </summary>
        public Form AddForm(SubmitFormBo submitFormModel, ObjectId userId)
        {
            var formValue = new FormValue
            {
                Aging = submitFormModel.Prescription,
                Brand = submitFormModel.Brand,
                ClaimGoods = submitFormModel.TakeOver,
                Date = submitFormModel.Date,
                Description = submitFormModel.ProductName,
                FormNo = Guid.NewGuid().ToString("N"),
                FormType = submitFormModel.FormType,
                OrderNo = submitFormModel.OrderId,
                Payment = submitFormModel.Payer,
                ShipAddress = submitFormModel.SendAddress,
                TargetCountry = submitFormModel.Destination,
                TimeQuantum = submitFormModel.TimeSlot,
                TimeSection = submitFormModel.TimeZoom,
                UserId = submitFormModel.UserId,
                Volume = new VolumeModel
                {
                    Heigth = submitFormModel.High,
                    Longth = submitFormModel.Long,
                    Wide = submitFormModel.Wide
                },
                Weight = submitFormModel.Weight
            };

            var form = new Form
            {
                UserId = userId,
                CreateDateTime = DateTime.Now,
                LastUpdateDateTime = DateTime.Now,
                Value = SerializeExtension.ToJson(formValue),
                FormNo = formValue.FormNo,
                TargetCountry = formValue.TargetCountry,
                Type = (FormType)formValue.FormType
            };
            _mongoRepository.Add(form);

            return form;
        }

        public void UpdateFormForStatus(string formId, FormStatus status)
        {
            _mongoRepository.Update<Form>(a => a.Id == formId.ToObjectId(), a => new Form { Status = status });
        }

        public void SetUserChatRemark(string userId, string remark)
        {
            _mongoRep.Update<User>(a => a.Id == userId.ToObjectId(), a => new User { ChatRemark = remark });
            _mongoRep.Update<NearlyChat>(a => a.CustomId == userId.ToObjectId(), a => new NearlyChat { ChatRemark = remark });
        }
    }
}

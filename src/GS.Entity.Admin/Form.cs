using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Sikiro.Nosql.Mongo.Base;

namespace Sikiro.Entity.Admin
{
    /// <summary>
    /// 表单
    /// </summary>
    [Mongo(DbConfig.Name)]
    public class Form : MongoEntity
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public ObjectId UserId { get; set; }

        /// <summary>
        /// 表单号
        /// </summary>
        public string FormNo { get; set; }

        /// <summary>
        /// 目的国家
        /// </summary>
        public string TargetCountry { get; set; }

        /// <summary>
        /// 表单类型  
        /// </summary>
        public FormType Type { get; set; }

        /// <summary>
        /// 表单当前状态
        /// </summary>
        public FormStatus Status { get; set; }

        /// <summary>
        /// 表单值 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 处理情况
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 最后更新问题
        /// </summary>
        public DateTime LastUpdateDateTime { get; set; }


        #region 后台审核用，记录谁审核

        public ObjectId AdministratorId { get; set; }
        public string AdministratorName { get; set; } 
        #endregion
    }

    public enum FormStatus
    {
        [Display(Name = "待确认")]
        Wait = 0,
        [Display(Name = "未查看")]
        NoLook = 10,
        [Display(Name = "已查看")]
        Looked = 20,
        [Display(Name = "已确认")]
        Confirmed = 90,
        [Display(Name = "结束表单")]
        EndConfirm = 99,
        [Display(Name = "退回")]
        Back = -10,
        [Display(Name = "取消")]
        Cancel = -20
    }

    public enum FormType
    {
        [Display(Name = "预定机仓")]
        BookRoom = 10,
        [Display(Name = "包货柜")]
        Container = 20,
        [Display(Name = "预定付款")]
        BookPay = 30,
        [Display(Name = "预定配送")]
        BookDelivery = 40,
    }

    /// <summary>
    /// form 字段Value对应的json
    /// </summary>
    public class FormValue
    {
        /// <summary>
        /// 表单号
        /// </summary>
        public string FormNo { get; set; }

        /// <summary>
        /// 表单类型
        /// </summary>
        public int FormType { get; set; }


        public string UserId { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public VolumeModel Volume { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string Payment { get; set; }

        /// <summary>
        /// 时效
        /// </summary>
        public string Aging { get; set; }

        /// <summary>
        /// 目的国家
        /// </summary>
        public string TargetCountry { get; set; }

        /// <summary>
        /// 取货方式
        /// </summary>
        public string ClaimGoods { get; set; }

        /// 选择日期
        public string Date { get; set; }

        // 选择时间段
        public string TimeQuantum { get; set; }

        // 选择时间区间
        public string TimeSection { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        public string ShipAddress { get; set; }
    }


    public class VolumeModel
    {
        public decimal Longth { get; set; }
        public decimal Wide { get; set; }
        public decimal Heigth { get; set; }
    }
}

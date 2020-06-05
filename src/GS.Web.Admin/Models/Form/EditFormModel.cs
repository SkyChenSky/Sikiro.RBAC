using System;
using Sikiro.Entity.Admin;

namespace Sikiro.Web.Admin.Models.Form
{
    public class EditFormModel
    {
        public string Id { get; set; }

        public   FormStatus Status { get; set; }


        /// <summary>
        /// 表单号
        /// </summary>
        public string FormNo { get; set; }


        /// <summary>
        /// 表单号
        /// </summary>
        public int FormType { get; set; }





        public string UserId { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }


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
        public DateTime Date { get; set; }


        // 选择时间段
        public string TimeQuantum { get; set; }

        // 选择时间区间
        public string TimeSection { get; set; }




        /// <summary>
        /// 发货地址
        /// </summary>
        public string ShipAddress { get; set; }



        /// <summary>
        /// 用于操作的判断
        /// </summary>

        public int Operation { get; set; }
    }
}

namespace Sikiro.Service.Admin.Bo
{
    public class SubmitFormBo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        public int FormType { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal Long { get; set; }

        /// <summary>
        /// 重
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public decimal Wide { get; set; }

        /// <summary>
        /// 取货方式
        /// </summary>
        public string TakeOver { get; set; }

        /// <summary>
        /// 目的国家
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 发送地址
        /// </summary>
        public string SendAddress { get; set; }

        /// <summary>
        /// 时效
        /// </summary>
        public string Prescription { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public string TimeSlot { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// 时间区间
        /// </summary>
        public string TimeZoom { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusStr { get; set; }
    }
}

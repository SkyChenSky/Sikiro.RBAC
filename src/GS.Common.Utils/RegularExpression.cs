namespace Sikiro.Common.Utils
{

    /// <summary>
    /// 正则式工具
    /// </summary>
    public  class RegularExpression
    {
        /// <summary>
        /// 只允许输入中文、英文字符
        /// </summary>
        public const string ZhongYing = @"^[\u4e00-\u9fa5a-zA-Z]+$";


        /// <summary>
        /// 只允许输入中文、英文字符,数字
        /// </summary>
        public const string ZhongYingShuZi = @"^[1-9]|[\u4e00-\u9fa5a-zA-Z]+$";

        /// <summary>
        /// 数字
        /// </summary>
        public const string  ShuZi = @"^[1-9]+$";

        /// <summary>
        /// 英文字符,数字
        /// </summary>
        public const string YingShuZi = @"^[1-9]|[a-zA-Z]+$";


        /// <summary>
        /// 只允许输入中文
        /// </summary>
        public const string Zhong= @"^[\u4e00-\u9fa5]*$";


        ///<summary>
        /// 身份证格式
        /// </summary>

        public const string ShenFen= @"^[1-9]\d{16}[\dXx]$";

        /// <summary>
        /// 手机号码
        /// </summary>
        public const string Phone= @"^[0-9]{6,11}$";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "^[a-zA-Z0-9_-]{6,18}|[^\\x00-\\xff]{2,6}$";
    }
}

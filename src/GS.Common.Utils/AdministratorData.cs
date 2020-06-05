namespace Sikiro.Common.Utils
{
    /// <summary>
    /// 登陆用户
    /// </summary>
    public class AdministratorData
    {
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuper { get; set; }
    }
}

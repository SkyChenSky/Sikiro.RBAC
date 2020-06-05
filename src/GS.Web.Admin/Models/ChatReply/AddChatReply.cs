using Sikiro.Entity.Admin;

namespace Sikiro.Web.Admin.Models.ChatReply
{
    public class ChatReplyEditModel
    {
        public string Id { get; set; }

        public string CompanyId { get; set; }

        public string ParentId { get; set; }

        public string News { get; set; }

        public int Order { get; set; }

        public ChatReplyStatus Status { get; set; }

        /// <summary>
        /// 简码
        /// </summary>
        public string Code { get; set; }
    }
}

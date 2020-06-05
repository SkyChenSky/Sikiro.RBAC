using System.Collections.Generic;

namespace Sikiro.Service.Admin.Bo
{
    public class ChatReplyBo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public IEnumerable<ChatReplyBo> Child { get; set; }
    }
}

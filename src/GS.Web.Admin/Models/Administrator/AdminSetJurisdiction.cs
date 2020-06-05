using System.Collections.Generic;

namespace Sikiro.Web.Admin.Models.Administrator
{
    public class AdminSetJurisdiction
    {
        public string AdminId { get; set; }
        public List<string> operations { get; set; }
        public string[] layuiTreeCheck { get; set; }

        public List<string> MenuIds { get; set; }
    }
}

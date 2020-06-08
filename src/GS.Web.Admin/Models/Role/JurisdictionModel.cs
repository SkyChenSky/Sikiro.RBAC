using System.Collections.Generic;

namespace Sikiro.Web.Admin.Models.Role
{
    public class JurisdictionModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Field { get; set; }

        public List<JurisdictionModel> Children { get; set; }
    }
}

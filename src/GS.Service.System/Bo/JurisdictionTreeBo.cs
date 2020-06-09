using System.Collections.Generic;

namespace Sikiro.Service.System.Bo
{
    public class JurisdictionTreeBo
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Field { get; set; }

        public IEnumerable<JurisdictionTreeBo> Children { get; set; }

        public bool Checked { get; set; }
    }
}

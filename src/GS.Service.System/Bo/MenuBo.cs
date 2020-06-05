using System.Collections.Generic;

namespace Sikiro.Service.System.Bo
{
    public class MenuBo
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }

        public IEnumerable<MenuBo> Children { get; set; }
    }
}

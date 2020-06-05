namespace Sikiro.Web.Admin.Models.Menu
{
    public class ListVo : TreeTableVo
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public int Order { get; set; }
    }
}

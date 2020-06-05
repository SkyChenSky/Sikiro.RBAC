namespace Sikiro.Web.Admin.Models
{
    public class PageListParams
    {
        private int? _limit;
        public int Limit
        {
            set => _limit = value;
            get => _limit ?? 10;
        }

        private int? _page;
        public int Page
        {
            set => _page = value;
            get => _page ?? 1;
        }
    }

    public class PageListParams<TParam> : PageListParams where TParam : new()
    {
        public PageListParams()
        {
            Params = new TParam();
        }

        public TParam Params { get; set; }
    }
}

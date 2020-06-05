using Sikiro.Entity.Admin;

namespace Sikiro.Web.Admin.Models.Form
{
    public class FormParams
    {

        public string FormNo { get; set; }


        public FormType? Type { get; set; }

        public FormStatus? Status { get; set; }
    }
}

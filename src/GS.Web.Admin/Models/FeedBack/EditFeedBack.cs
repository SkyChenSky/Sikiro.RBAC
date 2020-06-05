using System.ComponentModel.DataAnnotations;

namespace Sikiro.Web.Admin.Models.FeedBack
{
    public class EditFeedBack
    {

        public string Id { get; set; }
        ///<summary>
        /// 回复
        /// </summary>
        [Display(Name = "回复")]
        [Required]
        public string Reply { get; set; }


    }
}

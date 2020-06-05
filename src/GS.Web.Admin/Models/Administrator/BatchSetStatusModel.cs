using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sikiro.Entity.System;

namespace Sikiro.Web.Admin.Models.Administrator
{
    public class BatchSetStatusModel
    {
        public List<string> UserIds { get; set; }

        [Required]
        [Display(Name = "状态")]
        public EAdministratorStatus Status { get; set; }
    }
}

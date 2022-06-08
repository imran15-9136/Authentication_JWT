using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Authentication.Shared.ViewModel
{
    public class RoleCreateViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

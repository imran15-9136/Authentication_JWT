using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Model
{
    public class EmailConfirmMdel
    {
        public string Email { get; set; }
        public bool EmailSent { get; set; }
        public bool IsConfirmed { get; set; }
    }
}

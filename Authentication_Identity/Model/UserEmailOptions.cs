using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Model
{
    public class UserEmailOptions
    {
        public List<string> ToMails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}

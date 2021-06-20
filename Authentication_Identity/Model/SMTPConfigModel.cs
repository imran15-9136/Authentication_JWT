using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Model
{
    public class SMTPConfigModel
    {
        public string SenderAdress { get; set; }
        public string SenderDisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCrenditials { get; set; }
        public bool IsBodyHTML { get; set; }
    }
}

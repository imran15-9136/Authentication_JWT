using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Shared.ViewModel
{
    public class RoleManagerResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Messege { get; set; }
        public bool Result { get; set; }
    }
}

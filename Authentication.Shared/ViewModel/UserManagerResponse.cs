﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.Shared.ViewModel
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string EmailVerificatinToken { get; set; }
        public string UserId { get; set; }

    }
}

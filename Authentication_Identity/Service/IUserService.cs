using Authentication.Shared.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Service
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);
    }
}

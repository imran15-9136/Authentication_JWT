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
        Task<IdentityUser> GetUserByEmailAsync(string email);
        IQueryable<IdentityUser> GetUsers();
        string GetCurrentUserIdAsync();
        Task<IdentityUser> GetUserByIdAsync(string uid);
        Task<string> GenerateTokenAsync(IdentityUser user);
        Task<IdentityUser> ChangePasswordAsync(UserChangePassword model);
        Task<IdentityUser> AdminResetPasswordAsynbc(AdminResetPasswordViewModel model);
        void LogoutAsync();
    }
}


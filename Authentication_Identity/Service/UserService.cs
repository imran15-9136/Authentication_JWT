using Authentication.Shared.ViewModel;
using Authentication_Identity.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Service
{
    public class UserService : IUserService
    {
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        public async Task<IdentityUser> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<IdentityUser> GetUserByIdAsync(string uid) => await _userManager.FindByIdAsync(uid);

        public string GetCurrentUserIdAsync() => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Register Model is null");
            }
            var user = GetUserByEmailAsync(model.Email);
            if (user == null)
            {

                if (model.Password != model.ConfirmPassword)
                {
                    return new UserManagerResponse
                    {
                        Message = "Confirm password dosen't match",
                        IsSuccess = false,
                    };
                }

                var identityUser = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    var validEmailToken = await GenerateTokenAsync(identityUser);
                    return new UserManagerResponse
                    {
                        Message = "User Created Successfully",
                        IsSuccess = true,
                        EmailVerificatinToken = validEmailToken,
                        UserId = identityUser.Id
                    };
                }
                else
                {
                    return new UserManagerResponse
                    {
                        Message = "User didn't create",
                        IsSuccess = false,
                        Errors = result.Errors.Select(c => c.Description),
                    };
                }
            }
            return null;
        }

        public async Task<string> GenerateTokenAsync(IdentityUser user)
        {
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailTken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            return validEmailTken;
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return new UserManagerResponse
                {
                    Message = "There is no user with that email address",
                    IsSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                return new UserManagerResponse
                {
                    Message = "Invalid password",
                    IsSuccess = false,
                };
            }

            var clamis = new[]
            {
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: clamis,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            
            return new UserManagerResponse
            {
                Message = tokenAsString, 
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<IdentityUser> ChangePasswordAsync(UserChangePasswordDto model)
        {
            var userId = GetCurrentUserIdAsync();
            if (userId == null)
            {
                throw new Exception("User is not found while try to change password with user Id: " + userId);
            }
            var user = await GetUserByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception("Unable to change password with user Id: " + userId);
            }

            return user;
        }

        public async Task<IdentityUser> AdminresetPasswordAsync(AdminResetPasswordDto model)
        {
            var user = await GetUserByIdAsync(model.UserId);

            if (user!= null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,model.NewPassword);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            var user = await GetUserByIdAsync(uid);

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return result;
            }

            return null;
        }

        public void LogoutAsync() => _signInManager.SignOutAsync();

    }
}

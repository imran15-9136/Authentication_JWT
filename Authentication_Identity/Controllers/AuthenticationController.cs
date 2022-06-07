using Authentication.Shared.ViewModel;
using Authentication_Identity.API.Model;
using Authentication_Identity.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;


        public AuthenticationController(IUserService userService, IEmailService emailService, IConfiguration configuration)
        {
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;
         }

        // /api/authentication/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                    string appDomain = _configuration.GetSection("Applicaiton:AppDomain").Value;
                    string confirmationLink = _configuration.GetSection("Applicaiton:EmailConfirmation").Value; 

                    UserEmailOptions options = new UserEmailOptions
                    {
                        ToMails = new List<string>() { model.Email },
                        PlaceHolders = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("{{UserName}}",model.Name),
                            new KeyValuePair<string, string>("{{Link}}",string.Format(appDomain + "api/authentication/" + confirmationLink, result.UserId, result.EmailVerificatinToken))
                        } 
                    };

                    await _emailService.SendAccountConfirmationMail(options);
                    
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            else
            {
                return BadRequest("Invalid User Details");
            }
        }


        [HttpGet("GetUsers")]
        public IActionResult GetUsersAsync()
        {
            var user = _userService.GetUsers();
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }


        // /api/Authentication/confirm-email?uid=oi34u3o&token=938457498
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmAccountAsync(string uid, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
                {
                    token = token.Replace(' ', '+');

                    var result = await _userService.ConfirmEmailAsync(uid, token);

                    if (result.Succeeded)
                    {
                        return Ok(result);
                    }
                    return BadRequest("Unable to Activate");
                }
                return NotFound("User Not Found");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto model)
        {
            try
            {
                if (ModelState.IsValid && model.NewPassword == model.ReTypePassword)
                {
                    return Ok(await _userService.ChangePasswordAsync(model));
                }
                return BadRequest("Request Invalid");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("AdminPasswordReset")]
        public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordDto model)
        {
            try
            {
                if (ModelState.IsValid && model.NewPassword == model.ConfirmPassword)
                {
                    return Ok(await _userService.AdminresetPasswordAsync(model));
                }
                return BadRequest("Invalid Operation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // /api/Authentication/confirm-email?uid=oi34u3o&token=938457498
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ResendEmailConfirmationLinkAsync([FromBody]EmailConfirmMdel model)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(model.Email);

                if (user != null)
                {
                    if (user.EmailConfirmed)
                    {
                        model.IsConfirmed = true;
                        return Ok("Email Confirmed");
                    }
                    await _userService.GenerateTokenAsync(user);
                    model.EmailSent = true;
                }
                else
                {
                    ModelState.AddModelError("","Some Thing Went Wrong");
                }
                return BadRequest("Unable To Resend");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Logout")]
        public void Logout() => _userService.LogoutAsync();
    }
}

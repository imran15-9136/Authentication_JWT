using Authentication.Shared.ViewModel;
using Authentication_Identity.API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
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

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] UserChangePassword model)
        {
            if(ModelState.IsValid && model.CurrentPassword == model.NewPassword)
            {
                return Ok(await _userService.ChangePasswordAsync(model));
            }
            return BadRequest(model);
        }

        [HttpPut("AdminPasswordChange")]
        public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _userService.AdminResetPasswordAsynbc(model));
            }
            return BadRequest("Invalid Operation");
        }

        [HttpGet("GetUsers")]
        public  IActionResult GetUsersAsync()
        {
            var user = _userService.GetUsers();
            if (user!=null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost("Logout")]
        public void Logout() => _userService.LogoutAsync();
    }
}

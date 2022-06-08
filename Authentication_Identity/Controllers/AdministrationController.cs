using Authentication.API.Service;
using Authentication.Shared.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public AdministrationController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("RoleCreate")]
        public async Task<IActionResult> CreateRoleAsync(RoleCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleService.CreateRoleAsync(model);
                if (role == null)
                {
                    return Ok(role);
                }
            }
            return BadRequest();
        }
    }
}

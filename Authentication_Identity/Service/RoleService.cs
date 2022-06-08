using Authentication.Shared.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Authentication.API.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleService;

        public RoleService(RoleManager<IdentityRole> roleService)
        {
            _roleService = roleService;
        }

        public async Task<RoleManagerResponse> CreateRoleAsync(RoleCreateViewModel model)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };

            IdentityResult result = await _roleService.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return new RoleManagerResponse
                {
                    RoleName = model.RoleName,
                    Result = true
                };
            }
            return new RoleManagerResponse { Messege = result.Errors.ToString() };
        }
    }
}

using Authentication.Shared.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.API.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleService;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleService, UserManager<IdentityUser> userManager)
        {
            _roleService = roleService;
            _userManager = userManager;
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

        public async Task<IQueryable<IdentityRole>> GetRolesAsync()
        {
            var role = _roleService.Roles;
            if (role!=null)
            {
                return role;
            }
            return null;
        }

        public async Task<IdentityRole> EditRoleAsync(string id)
        {
            var role = await _roleService.FindByIdAsync(id);

            if (role==null)
            {
                return null;
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user,role.Name))
                {
                    
                     user.UserName
                }
            }
        }
    }
}

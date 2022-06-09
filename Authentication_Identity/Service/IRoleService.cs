using Authentication.Shared.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.API.Service
{
    public interface IRoleService
    {
        Task<RoleManagerResponse> CreateRoleAsync(RoleCreateViewModel model);
        Task<IQueryable<IdentityRole>> GetRolesAsync();
    }
}

using Authentication.Shared.ViewModel;
using System.Threading.Tasks;

namespace Authentication.API.Service
{
    public interface IRoleService
    {
        Task<RoleManagerResponse> CreateRoleAsync(RoleCreateViewModel model);
    }
}

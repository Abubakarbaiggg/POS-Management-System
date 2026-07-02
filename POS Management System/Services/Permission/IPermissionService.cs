using System.Security.Claims;

namespace POS_Management_System.Services.Permission
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(ClaimsPrincipal user, string permission);
    }
}

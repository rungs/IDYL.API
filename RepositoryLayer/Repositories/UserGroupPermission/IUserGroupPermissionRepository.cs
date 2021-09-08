using IdylAPI.Models;
using IdylAPI.Models.Syst;

namespace IdylAPI.Services.Interfaces.Authorize
{
    public interface IUserGroupPermissionRepository
    {
        Result GetByUserGroup(int userGroupNo);
        Result GetUserGroupAll();
        Result UpdateUserGroupPermission(UserGroupPermission userGroupPermission);

    }
}

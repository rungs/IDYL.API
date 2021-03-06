using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface IUserGroupMenuService
    {
        IEnumerable<UserGroupMenu> GetByUserGroup(int userGroupId);
        Task Update(UserGroupMenu userGroupMenu);
    }
}

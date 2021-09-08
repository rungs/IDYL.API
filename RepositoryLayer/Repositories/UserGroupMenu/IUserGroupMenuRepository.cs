using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface IUserGroupMenuRepository : IRepository<UserGroupMenu>
    {
        IEnumerable<UserGroupMenu> GetByUserGroup(int userGroupNo);
        Task<UserGroupMenu> GetByMenuUserGroup(int userGroupNo, int menuId);
    }
}

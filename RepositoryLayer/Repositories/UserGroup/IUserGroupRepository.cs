using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface IUserGroupRepository : IRepository<UserGroup>
    {
        Task<UserGroup> GetByUserGroup(int userGroupId);
      
    }
}

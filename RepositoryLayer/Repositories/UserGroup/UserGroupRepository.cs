using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Syst
{
    public class UserGroupRepository : BaseRepositoryV2<UserGroup>, IUserGroupRepository
    {
     
        public UserGroupRepository(AppDBContext context) : base(context)
        {
           
        }
        public async Task<UserGroup> GetByUserGroup(int userGroupNo)
        {
            return await _entities.Where(x => x.UserGroupNo == userGroupNo).FirstOrDefaultAsync();
        }
     
    }
}

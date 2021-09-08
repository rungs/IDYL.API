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
    public class UserGroupMenuRepository : BaseRepositoryV2<UserGroupMenu>, IUserGroupMenuRepository 
    {
     
        public UserGroupMenuRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<UserGroupMenu> GetByMenuUserGroup(int userGroupNo, int menuId)
        {
            return await _entities.Where(x => x.UserGroupNo == userGroupNo && x.MenuId == menuId).FirstOrDefaultAsync();
        }

        public IEnumerable<UserGroupMenu> GetByUserGroup(int userGroupNo)
        {
            return _entities.Where(x => x.UserGroupNo == userGroupNo && x.Menu.Need_Lock == false).Include(i => i.UserGroup).Include(i => i.Menu).OrderBy(i => i.Menu.OrderNo).AsEnumerable();
        }
    }
}

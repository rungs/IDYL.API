using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystPermissionsRepository : BaseRepositoryV2<FormPermission>, ISystPermissionsRepository
    {
        public SystPermissionsRepository(AppDBContext context) : base(context) { }
        public void DeleteByUserNo(int userNo)
        {
             _entities.RemoveRange(_entities.Where(x => x.UserNo == userNo).AsEnumerable());
        }
    }
}

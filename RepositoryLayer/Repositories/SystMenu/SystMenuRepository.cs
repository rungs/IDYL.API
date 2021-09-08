using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystMenuRepository : BaseRepositoryV2<Menu>, ISystMenuRepository
    {
        public SystMenuRepository(AppDBContext context) : base(context) { }

    }
}

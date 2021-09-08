using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystConfigRepository : BaseRepositoryV2<SysConfig>, ISystConfigRepository
    {
        public SystConfigRepository(AppDBContext context) : base(context) { }

    }
}

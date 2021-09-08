using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystActionRepository : BaseRepositoryV2<SystAction>, ISystActionRepository
    {
        public SystActionRepository(AppDBContext context) : base(context) { }

    }
}

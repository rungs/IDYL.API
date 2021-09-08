using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystPermissionsDataRepository : BaseRepositoryV2<FormPermissionData>, ISystPermissionsDataRepository
    {
        public SystPermissionsDataRepository(AppDBContext context) : base(context) { }

    }
}

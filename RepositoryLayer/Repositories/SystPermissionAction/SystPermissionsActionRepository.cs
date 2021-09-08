using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystPermissionsActionRepository : BaseRepositoryV2<FormPermissionAction>, ISystPermissionsActionRepository
    {
        public SystPermissionsActionRepository(AppDBContext context) : base(context) { }

    }
}

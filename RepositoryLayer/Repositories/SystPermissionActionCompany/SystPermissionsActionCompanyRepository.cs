using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystPermissionsActionCompanyRepository : BaseRepositoryV2<SystPermissionsActionCompany>, ISystPermissionsActionCompanyRepository
    {
        public SystPermissionsActionCompanyRepository(AppDBContext context) : base(context) { }

    }
}

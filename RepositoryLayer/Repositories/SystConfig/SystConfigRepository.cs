using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystConfigRepository : BaseRepositoryV2<SysConfig>, ISystConfigRepository
    {
        public SystConfigRepository(AppDBContext context) : base(context) { }

        public IEnumerable<SysConfig> GetConfigByCompanyNo(int companyNo)
        {
            return _entities.Where(x => x.CompanyNo.Equals(companyNo));
        }
    }
}

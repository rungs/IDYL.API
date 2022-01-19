using Domain.Interfaces;
using IdylAPI.Models.Syst;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISystConfigRepository : IRepository<SysConfig>
    {
        IEnumerable<SysConfig> GetConfigByCompanyNo(int companyNo);
    }
}

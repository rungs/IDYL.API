using Domain.Interfaces;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISysParameter1Repository : IRepository<SysParameter1>
    {
        Task<IEnumerable<SysParameter1>> GetSysParameterByCompany(int companyNo);
        Task<SysParameter1> GetSysParameterByParaAndCompany(int companyNo, string paraNo);
        Task GenCodeFormat(int companyNo, string paraNo);
    }
}

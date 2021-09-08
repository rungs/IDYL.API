using IdylAPI.Models;
using IdylAPI.Models.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Service.Specification
{
    public interface IEQSpecService
    {
        Task CopySpec(int eqNo, int eqTypeNo, int userNo);
        IEnumerable<EQSpec> GetByEq(int eqNo);
        Task Delete(int eqNo, int specNo);
        Task<EQSpec> Insert(EQSpec eQSpec);
        Task UpdateValue(EQSpec eQSpec);
        string GetEQSpecAll(int companyNo);
    }
}

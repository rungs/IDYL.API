using IdylAPI.Models;
using IdylAPI.Models.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Service.Specification
{
    public interface IEQTypeSpecService
    {
        IEnumerable<EqTypeSpec> GetByEQType(int eqTypeNo);
        Task Delete(int eqTypeNo, int specNo);
        Task<EqTypeSpec> Insert(EqTypeSpec eqTypeSpec);
    }
}

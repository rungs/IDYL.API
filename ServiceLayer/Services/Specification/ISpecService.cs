using IdylAPI.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Service.Specification
{
    public interface ISpecService
    {
        Task<Result> InsertSpec(Spec Specification);
        Task DeleteSpec(int id, int updatedBy);
        IEnumerable<Spec> GetByCompany(int companyNo, bool isDelete);
        IEnumerable<Spec> ExcludeEqType(int eqTypeNo);
        IEnumerable<Spec> ExcludeEq(int eqNo, int eqTypeNo);
      
    }
}

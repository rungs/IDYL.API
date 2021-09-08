using Domain.Interfaces;
using IdylAPI.Models.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Repository.Specification
{
    public interface IEQSpecRepository : IRepository<EQSpec>
    {
        Task CopySpec(int eqNo, int eqTypeNo, int userNo);
        IEnumerable<EQSpec> GetByEq(int eqNo);
        string GetEQSpecAll(int companyNo);
        void Delete(int eqNo, int specNo);
        void UpdateValue(EQSpec eQSpec);    
    }
}

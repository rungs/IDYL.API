using Domain.Interfaces;
using IdylAPI.Models;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Repository.Specification
{
    public interface ISpecRepository : IRepository<Spec>
    {
        IEnumerable<Spec> GetByCompany(int companyNo, bool isDelete);
        Spec GetByCode(string code, int companyNo);
        IEnumerable<Spec> ExcludeEqType(int eqTypeNo);
        List<Spec> ExcludeEq(int eqNo, int eqTypeNo);
        void UpdateIsUse(int specNo);
    }
}

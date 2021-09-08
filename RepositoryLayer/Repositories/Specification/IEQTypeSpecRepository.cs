using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Specification;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Repository.Specification
{
    public interface IEQTypeSpecRepository : IRepository<EqTypeSpec>
    {
        IEnumerable<EqTypeSpec> GetByEQType(int eqTypeNo);
        void Delete(int eqTypeNo, int specNo);
    }
}

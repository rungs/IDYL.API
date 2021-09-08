using Domain.Interfaces;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface ICraftTypeRepository : IRepository<CraftType>
    {
        IEnumerable<CraftType> GetByCompany(int companyNo);
    }
}

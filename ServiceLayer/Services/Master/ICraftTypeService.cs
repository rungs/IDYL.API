using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ICraftTypeService
    {
        IEnumerable<CraftType> GetByCompany(int companyNo);
    }
}

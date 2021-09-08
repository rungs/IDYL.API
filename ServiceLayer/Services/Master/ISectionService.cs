using IdylAPI.Models;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISectionService
    {
        IEnumerable<Section> GetByCompany(int companyNo);
        Result Retrive(WhereParameter whereParameter);
    }
}

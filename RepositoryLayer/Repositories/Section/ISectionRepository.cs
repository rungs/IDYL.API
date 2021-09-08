using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface ISectionRepository : IRepository<Section>
    {
        Result Retrive(WhereParameter whereParameter);
        IEnumerable<Section> GetByCompany(int companyNo);
        Section GetSectionByCode (string code, int companyNo);
    }
}

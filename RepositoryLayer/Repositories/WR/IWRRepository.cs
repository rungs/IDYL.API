using IdylAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.WR
{
    public interface IWRRepository
    {
        Result Retrive(WhereParameter whereParameter, Models.Authorize.User user);
        Result RetriveById(int id);
        Result Insert(Models.WO.WO wo, Models.Authorize.User user);
        Result CreateWR (Models.WO.WO wo);
        Result RetriveForReportProblem(int siteNo, int systemNo);
        Result CreateWR(DomainLayer.Entities.WR wr);
    }
}

using IdylAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.WO
{
    public interface IWORepository
    {
        Result RetriveWO(WhereParameter whereParameter, Models.Authorize.User user, LoadOptions loadOption);
        Result RetriveById(int id, Models.Authorize.User user);
        Result RetrivePlan(int woNo);
        Result RetriveActual(int id);
        Result RetriveEvaluate(int id);
        Result RetriveInspecFiles(int id);
        Result RetriveWOToLocal(List<Models.WO.WO> wos, Models.Authorize.User user);
        Result RetriveViewFilter(WhereParameter whereParameter, Models.Authorize.User user);
        Result UpdatePlan(Models.WO.WO wo, Models.Authorize.User user);
        Result UpdateActual(Models.WO.WO wo, Models.Authorize.User user);
        Result UpdateStatus(int woNo, int woStatusNo, int updatedBy);


    }
}

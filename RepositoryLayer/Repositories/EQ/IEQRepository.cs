using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface IEQRepository
    {
        Result Retrive(WhereParameter whereParameter, User user);
        Result RetriveEf(WhereParameter whereParameter, User user);
        IEnumerable<EQ>  Retrive2(WhereParameter whereParameter, User user);
        Result RetriveById(int id);
        Result RetriveByCode(WhereParameter whereParameter, User user);
        Result RetriveInProgrss(WhereParameter whereParameter);
        Result RetriveHistory(WhereParameter whereParameter);
        Result RetriveEqResource(WhereParameter whereParameter);
        Result RetriveMaintainance(WhereParameter whereParameter);
        Result Insert(EQ eQ, User user);
    }
}

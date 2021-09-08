using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ILocationService
    {
        Result Retrive(WhereParameter whereParameter, User user);
        Result Insert(Location location, User user);
        IEnumerable<Location> GetByCompany(int companyNo);
       // PagedList<Location> GetLocations(WhereParameter parameters);
    }
}

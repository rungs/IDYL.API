
using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface ILocationRepository : IRepository<Location>
    {
        Result Retrive(WhereParameter whereParameter, User user);
        Result Insert(Location location, User user);
        IEnumerable<Location> GetByCompany(int companyNo);
        //PagedList<Location> GetLocations();
        //iqu GetLocations();
    }
}

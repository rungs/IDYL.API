using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface IResourceRepository
    {
        Result Retrive(WhereParameter whereParameter);
        Result Insert(Resource resource, Models.Authorize.User user);
    }
}

using IdylAPI.Models;
using IdylAPI.Models.WO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.WO
{
    public interface IWOResRepository
    {
        Result Insert(List<WOResource>  wOResource, Models.Authorize.User user);
        Result Copy(List<WOResource> wOResource, Models.Authorize.User user);
        Result Delete(int id);
        
    }
}

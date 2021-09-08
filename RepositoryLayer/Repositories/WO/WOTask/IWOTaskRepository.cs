using IdylAPI.Models;
using IdylAPI.Models.WO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.WO
{
    public interface IWOTaskRepository
    {
        Result RetriveByWO(int woNo);
        Result Insert(List<WOTask> wOTasks, Models.Authorize.User user);
        Result UpdateAbnormal(WOTask wOTasks, Models.Authorize.User user);
        
    }
}

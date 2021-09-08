using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface IFailureObjectRepository : IRepository<FailureObject>
    {
        Result Retrive(WhereParameter whereParameter);
        Result Insert(FailureObject failureObject, Models.Authorize.User user);
        Task<int> InsertBulk(List<FailureObject> failureObject);
    }
}

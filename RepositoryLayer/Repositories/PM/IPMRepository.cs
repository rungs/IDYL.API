 using Domain.Entities.PM;
using IdylAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPMRepository : IRepository<PM>
    {
        Result GetPmAllByEq(WhereParameter whereParameter);
        PM GetPmByCode(string code, int companyNo);
        PM GetPmById(int pmNo);
        Result UpdatePm(PM pM, IdylAPI.Models.Authorize.User user);
    }
}

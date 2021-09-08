using Domain.Entities.PM;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface IPMService
    {
        Result GetPmAllByEq(WhereParameter whereParameter);
        PM GetPmByCode(string code, int companyNo);
        PM GetPmById(int pmNo);
        Task<Result> Update(PM pM, User user);
    }
}

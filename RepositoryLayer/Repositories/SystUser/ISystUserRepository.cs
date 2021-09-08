using Domain.Interfaces;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISystUserRepository : IRepository<SystUser>
    {
        IEnumerable<SystUser> GetByCompany(int companyNo);
        IEnumerable<SystUser> GetSuperUserByCompany(int companyNo);
        IEnumerable<SystUser> GetUserByProductKey(string productKey);
        IEnumerable<SystUser> CheckUsernameDupicate(string username);
        IEnumerable<Models.Master.Customer> CheckEmailDupicate(string email);
        SystUser GetByUserId(int userId, int companyNo);
        Task UpdateUserActiveSite(int CompanyNo, bool IsActive, int UserNo);
        IEnumerable<UserView> GetUserView();
        ActivateUser GetUserCustomerById(int userId, int companyNo);
    }
}

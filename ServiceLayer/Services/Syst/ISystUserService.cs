using IdylAPI.Models;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISystUserService
    {
        IEnumerable<UserView> GetUserView();
        IEnumerable<SystUser> GetByCompany(int companyNo);
        Task<Result> UnlockUser(int userGroupId, int noOfUser, int companyNo);
        Task<Result> UnlockUser(CreateUser userObj);
        Task Update(int id, SystUser obj);
        Task UpdateNotSeeAutoAuthenLocation(int userId, string isNotSeeAuto_AuthenLocation);
        Task<SystUser> GetByid(int id);
        IEnumerable<SystUser> GetUserByProductKey(string productKey);
        SystUser GetByUserId(int userId, int companyNo);
        Task<Result> ActivateUser(ActivateUser systUser);
        IEnumerable<SystUser> CheckUsernameDupicate(string username);
        IEnumerable<Models.Master.Customer> CheckEmailDupicate(string email);
        Task UpdateUserActiveSite(int CompanyNo, bool IsActive, int UserNo);
        Task<Result> SendEmail(int userId);
        ActivateUser GetUserCustomerById(int userId, int companyNo);
    }
}

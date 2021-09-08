using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Company
{
    public interface ICompanyService
    {
        Task<Site> InsertCompany(Site site);
        Task UpdateCompany(Site site, int id);
        Task<Site> GetCompanyById(int id);
        IEnumerable<Site> GetCompanyies();
        IEnumerable<Site> GetCompanyByUserId(int userId);
        Task DeleteCompany(int id, User user);
        Task UpdateSubsite(Site site, User user);
        Task<Site> InsertSubsite(Site site, User user);
        IEnumerable<Site> GetCompanyProductKeyUser(string productkey, int userid);
    }
}

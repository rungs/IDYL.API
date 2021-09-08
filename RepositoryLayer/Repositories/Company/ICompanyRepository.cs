using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICompanyRepository : IRepository<Site>
    {
        IEnumerable<Site> GetCompanyByUserId(int userId);
        Site GetCompanyByProductKey(string productKey);
        IEnumerable<Site> GetCompanyProductKeyUser(string productkey, int userid);
    }
}

using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
namespace IdylAPI.Services.Repository.Master
{
    public class CraftTypeRepository : BaseRepositoryV2<CraftType>, ICraftTypeRepository
    {
        public CraftTypeRepository(AppDBContext context) : base(context)
        {  
        }

        public IEnumerable<CraftType> GetByCompany(int companyNo)
        {
            return _entities.Where(x => x.CompanyNo == companyNo && x.IsDelete == false).AsEnumerable();
        }
    }
}

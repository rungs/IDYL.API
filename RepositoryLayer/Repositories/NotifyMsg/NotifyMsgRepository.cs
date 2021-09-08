using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Syst
{
    public class NotifyMsgRepository : BaseRepositoryV2<NotifyMsg>, INotifyMsgRepository
    {
        public NotifyMsgRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<NotifyMsg>> GetNotifyMsgByCompany(int companyNo)
        {
            return await _entities.Where(x => x.CompanyNo == companyNo).ToListAsync();
        }
    }
}

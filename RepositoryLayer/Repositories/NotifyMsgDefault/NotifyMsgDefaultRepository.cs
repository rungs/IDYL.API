using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Syst
{
    public class NotifyMsgDefaultRepository : BaseRepositoryV2<NotifyMsgDefault>, INotifyMsgDefaultRepository
    {
        public NotifyMsgDefaultRepository(AppDBContext context) : base(context) { }

        public async Task<IEnumerable<NotifyMsgDefault>> GetNotifyMsgDefaultByIndex(int indexNo)
        {
            return await _entities.Where(x => x.IndexNo == indexNo).ToListAsync();
        }
    }
}

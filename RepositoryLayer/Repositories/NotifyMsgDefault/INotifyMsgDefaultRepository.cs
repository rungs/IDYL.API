using Domain.Interfaces;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface INotifyMsgDefaultRepository : IRepository<NotifyMsgDefault>
    {
        Task<IEnumerable<NotifyMsgDefault>> GetNotifyMsgDefaultByIndex(int indexNo);
    }
}

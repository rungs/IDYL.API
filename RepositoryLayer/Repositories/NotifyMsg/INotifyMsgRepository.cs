using Domain.Interfaces;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface INotifyMsgRepository : IRepository<NotifyMsg>
    {
        Task<IEnumerable<NotifyMsg>> GetNotifyMsgByCompany(int companyNo);
    }
}

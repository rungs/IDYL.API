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
    public class NotifyMsgRoleRepository : BaseRepositoryV2<NotifyMsg_Role>, INotifyMsgRoleRepository
    {
        public NotifyMsgRoleRepository(AppDBContext context) : base(context) { }

    }
}

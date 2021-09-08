using Domain.Entities.Syst;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services.Syst
{
    public interface ILoginHistoryService
    {
        IEnumerable<LogInHistory> GetAllLoginHistory();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces
{
    public interface ITypedHubClient
    {
        Task BroadcastMessage();
    }
}

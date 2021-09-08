using IdylAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Hubs
{
    public class NotifyHub : Hub<ITypedHubClient>
    {
        public void Send()
        {
            Clients.All.BroadcastMessage();
        }
    }
}

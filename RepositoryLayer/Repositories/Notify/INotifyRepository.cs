using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Authorize
{
    public interface INotifyRepository
    {
        Result Insert(string token, User user);
        Result RetriveTokenByUser(int userNo);
        Result PushMsg(int companyNo, int woNo, int customerNo, string action);
        Result PushMsg(Models.Notify.NotiObj notiObj);
        Result RetriveInboxByUser(WhereParameter whereParameter, int userNo);
        Result SendAssignMsg(IEnumerable<NotiObj> notiObjs, User user);

    }
}

using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Authorize
{
    public interface IAuthorizeRepository
    {
        Result Login(string username, string password);
        Result RetriveUserInfo(User user, int siteNo);
        Result Logout(int notifyTokenNo);
        Result RefreshToken(string token);
        Result Reset(User user);

    }
}

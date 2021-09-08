using IdylAPI.Models.Authorize;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Helper
{
    public static class TokenHelper
    {
        public static User DecodeTokenToInfo(HttpContext context)
        {
            User objUser = new User();
            if (context.User.Identity != null)
            {
                IEnumerable<System.Security.Claims.Claim> claims = ((System.Security.Claims.ClaimsIdentity)context.User.Identity).Claims;
                objUser.UserNo = InputVal.ToInt(claims.First(claim => claim.Type == "UserNo").Value);
                objUser.CustomerNo = InputVal.ToInt(claims.First(claim => claim.Type == "CustomerNo").Value);
                objUser.CustomerName = InputVal.ToString(claims.First(claim => claim.Type == "CustomerName").Value);
                objUser.Username = InputVal.ToString(claims.First(claim => claim.Type == "Username").Value);
                objUser.SectionNo = InputVal.ToIntNull(claims.First(claim => claim.Type == "SectionNo").Value);
                objUser.UserGroupId = InputVal.ToIntNull(claims.First(claim => claim.Type == "UserGroupId").Value);
            }
            return objUser;

        }
    }


}

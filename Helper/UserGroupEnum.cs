using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Helper
{
    public static class UserGroupEnum
    {
        public  enum UserGroup
        {
            Admin =1,
            General = 2,
            Requester = 3,
            EqOwner = 4,
            Free = 5,
        }
    }
}

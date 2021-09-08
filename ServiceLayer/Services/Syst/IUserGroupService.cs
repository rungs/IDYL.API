using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface IUserGroupService
    {
        Task Update(UserGroup userGroup);
       
    }
}

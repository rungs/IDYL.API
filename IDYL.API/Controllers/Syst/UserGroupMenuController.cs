using Microsoft.AspNetCore.Mvc;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;

namespace IdylAPI.Controllers.Syst
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupMenuController : ControllerBase
    {
        private readonly IUserGroupMenuService _userGroupMenuService;

        public UserGroupMenuController(IUserGroupMenuService userGroupMenuService)
        {
            _userGroupMenuService = userGroupMenuService;
        }

   
        [HttpGet("usergroup/{userGroupNo:int}")]
        public OkObjectResult RetriveByFormId(int userGroupNo)
        {
            return Ok(_userGroupMenuService.GetByUserGroup(userGroupNo));
        }

        [HttpPost]
        public OkObjectResult Update([FromBody] UserGroupMenu userGroupMenu)
        {
            return Ok(_userGroupMenuService.Update(userGroupMenu));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;

namespace IdylAPI.Controllers.Syst
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly IUserGroupService _userGroupService;

        public UserGroupController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

   
        [HttpPost]
        public OkObjectResult Update([FromBody] UserGroup userGroup)
        {
            return Ok(_userGroupService.Update(userGroup));
        }
    }
}

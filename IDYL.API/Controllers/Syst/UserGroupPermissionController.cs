using Microsoft.AspNetCore.Mvc;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Models.Syst;

namespace IdylAPI.Controllers.Syst
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupPermissionController : ControllerBase
    {
        
        private readonly IUserGroupPermissionRepository _userGroupPermissionRepository;

        public UserGroupPermissionController(IUserGroupPermissionRepository userGroupPermissionRepository)
        {
            _userGroupPermissionRepository = userGroupPermissionRepository;
        }

   
        [HttpGet("v1/usergroup/{userGroupNo:int}")]
        public OkObjectResult RetriveByFormId(int userGroupNo)
        {
            return Ok(_userGroupPermissionRepository.GetByUserGroup(userGroupNo));
        }

        [HttpGet("v1/usergroupall")]
        public OkObjectResult RetriveUserGroupAll()
        {
            return Ok(_userGroupPermissionRepository.GetUserGroupAll());
        }
        [HttpPost("v1/usergrouppermision")]
        public OkObjectResult UpdateUserGroupPermission([FromBody] UserGroupPermission userGroupPermission)
        {
            return Ok(_userGroupPermissionRepository.UpdateUserGroupPermission(userGroupPermission));
        }
    }
}

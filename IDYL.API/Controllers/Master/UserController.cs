using Microsoft.AspNetCore.Mvc;
using IdylAPI.Services.Interfaces.Master;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {

            _userRepository = userRepository;
        }

        [HttpGet("v1/All")]
        public OkObjectResult GetAll()
        {
            return Ok(_userRepository.RetriveAll());
        }
    }
}

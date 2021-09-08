using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {

        private readonly ISystemRepository _systemRepository;

        public SystemController(ISystemRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_systemRepository.Retrive(whereParameter));
        }
    }
}

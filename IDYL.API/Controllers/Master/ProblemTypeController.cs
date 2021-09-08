using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemTypeController : ControllerBase
    {

        private readonly IProblemTypeRepository _problemTypeRepository;

        public ProblemTypeController(IProblemTypeRepository problemTypeRepository)
        {
            _problemTypeRepository = problemTypeRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_problemTypeRepository.Retrive(whereParameter));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Helper;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class FailureObjectController : ControllerBase
    {

        private readonly IFailureObjectRepository _failureObjectRepository;

        public FailureObjectController(IFailureObjectRepository failureObjectRepository)
        {

            _failureObjectRepository = failureObjectRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_failureObjectRepository.Retrive(whereParameter));
        }

        [Authorize]
        [HttpPost("v1/Insert")]
        public OkObjectResult Insert([FromBody] FailureObject failureObject)
        {
            return Ok(_failureObjectRepository.Insert(failureObject, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

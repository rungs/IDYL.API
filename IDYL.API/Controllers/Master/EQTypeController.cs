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
    public class EQTypeController : ControllerBase
    {

        private readonly IEQTypeRepository _eqTypeRepository;

        public EQTypeController(IEQTypeRepository eqTypeRepository)
        {

            _eqTypeRepository = eqTypeRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqTypeRepository.Retrive(whereParameter));
        }

        [Authorize]
        [HttpPost("v1/update")]
        public OkObjectResult Insert([FromBody] EQType eQType)
        {
            return Ok(_eqTypeRepository.Insert(eQType, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

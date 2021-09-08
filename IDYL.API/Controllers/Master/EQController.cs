using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Helper;
using Microsoft.AspNetCore.OData.Query;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class EQController : ControllerBase
    {

        private readonly IEQRepository _eqRepository;

        public EQController(IEQRepository eqRepository)
        {

            _eqRepository = eqRepository;
        }

        [Authorize]
        [HttpGet("v1/all")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.Retrive(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpGet("v1/all/ef")]
        public OkObjectResult GetEf([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.RetriveEf(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpGet("v1/all/odata")]
        [EnableQuery]
        public OkObjectResult GetOdata([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.Retrive2(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpGet("v1/{id}")]
        public OkObjectResult RetriveById(int id)
        {
            return Ok(_eqRepository.RetriveById(id));
        }

        [Authorize]
        [HttpGet("v1/code")]
        public OkObjectResult GetByCode([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.RetriveByCode(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpGet("v1/inprogress")]
        public OkObjectResult GetInprogress([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.RetriveInProgrss(whereParameter));
        }

        [Authorize]
        [HttpGet("v1/history")]
        public OkObjectResult GetHistory([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.RetriveHistory(whereParameter));
        }

        [Authorize]
        [HttpGet("v1/ma")]
        public OkObjectResult GetMA([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.RetriveMaintainance(whereParameter));
        }

        [Authorize]
        [HttpGet("v1/partusage")]
        public OkObjectResult GetPartUsage([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_eqRepository.RetriveEqResource(whereParameter));
        }

        [Authorize]
        [HttpPost("v1/Update")]
        public OkObjectResult Insert([FromBody] EQ eQ)
        {
            return Ok(_eqRepository.Insert(eQ, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

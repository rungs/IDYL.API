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
    public class EvaluateController : ControllerBase
    {

        private readonly IEvaluateRepository _evaluateRepository;

        public EvaluateController(IEvaluateRepository evaluateRepository)
        {

            _evaluateRepository = evaluateRepository;
        }

        [Authorize]
        [HttpGet("v1/{woNo}/Site/{companyNo}")]
        public OkObjectResult Get(int woNo, int companyNo)
        {
            return Ok(_evaluateRepository.RetriveByWONo(woNo, companyNo));
        }

        [Authorize]
        [HttpPost("v1")]
        public OkObjectResult Insert(Sign sign)
        {
            return Ok(_evaluateRepository.Insert(sign, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

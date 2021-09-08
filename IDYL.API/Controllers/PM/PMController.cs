using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Helper;

namespace IdylAPI.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMController : ControllerBase
    {

        private readonly IPMService _pmRepository;

        public PMController(IPMService pmRepository)
        {
            _pmRepository = pmRepository;
        }

        [HttpGet("v1/GetPmAllByEq")]
        public OkObjectResult GetPmAllByEq([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_pmRepository.GetPmAllByEq(whereParameter));
        }

        [HttpGet("v1/GetPmByCode/{code}/CompanyNo/{companyNo}")]
        public OkObjectResult GetPmByCode(string code, int companyNo)
        {
            return Ok(_pmRepository.GetPmByCode(code, companyNo));
        }

        [HttpGet("v1/GetPmById/{pmNo}")]
        public OkObjectResult GetPmById(int pmNo)
        {
            return Ok(_pmRepository.GetPmById(pmNo));
        }

        [Authorize]
        [HttpPost("v1/Create")]
        public OkObjectResult UpdatePm([FromBody] Domain.Entities.PM.PM pM)
        {
            return Ok(_pmRepository.Update(pM, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

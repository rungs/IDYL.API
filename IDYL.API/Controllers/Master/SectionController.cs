using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces.Syst;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_sectionService.Retrive(whereParameter));
        }

        [HttpGet("company/{companyNo}")]
        public OkObjectResult GetAll(int companyNo)
        {
            return Ok(_sectionService.GetByCompany(companyNo));
        }
    }
}

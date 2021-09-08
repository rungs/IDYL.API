using Microsoft.AspNetCore.Mvc;
using IdylAPI.Services.Interfaces.Syst;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class CraftTypeController : ControllerBase
    {
        private readonly ICraftTypeService _craftTypeService;
        public CraftTypeController(ICraftTypeService craftTypeService)
        {
            _craftTypeService = craftTypeService;
        }

        [HttpGet("company/{companyNo}")]
        public OkObjectResult GetAll(int companyNo)
        {
            return Ok(_craftTypeService.GetByCompany(companyNo));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Service.Specification;
using IdylAPI.Models.Specification;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class EQTypeSpecController : ControllerBase
    {
        private readonly IEQTypeSpecService _eqTypeSpecService;

        public EQTypeSpecController(IEQTypeSpecService eqTypeSpecService)
        {
            _eqTypeSpecService = eqTypeSpecService;
        }

        [HttpGet("eqtype/{eqTypeNo}")]
        public IActionResult GetByEQType(int eqTypeNo)
        {
            var list = _eqTypeSpecService.GetByEQType(eqTypeNo);
            return Ok(list);
        }

        [HttpPost("deletion/eqtype/{eqTypeNo}/spec/{specNo}")]
        public async Task<IActionResult> DeleteSpec(int eqTypeNo, int specNo)
        {
            await _eqTypeSpecService.Delete(eqTypeNo, specNo);
            return Ok();
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] EqTypeSpec eqTypeSpec)
        {
            eqTypeSpec = await _eqTypeSpecService.Insert(eqTypeSpec);
            return Ok(eqTypeSpec);
        }
    }
}

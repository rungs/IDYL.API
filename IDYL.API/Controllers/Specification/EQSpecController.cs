using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Service.Specification;
using IdylAPI.Models.Specification;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class EQSpecController : ControllerBase
    {
        private readonly IEQSpecService _eqSpecService;

        public EQSpecController(IEQSpecService eqSpecService)
        {
            _eqSpecService = eqSpecService;
        }

        [HttpPost("copy/eq/{eqNo}/eqtype/{eqTypeNo}/user/{userNo}")]
        public async Task<IActionResult> CopySpecToEq(int eqNo, int eqTypeNo, int userNo)
        {
            await _eqSpecService.CopySpec(eqNo, eqTypeNo, userNo);
            return Ok();
        }

        [HttpGet("eq/{eqNo}")]
        public IActionResult GetByEq(int eqNo)
        {
            var list =  _eqSpecService.GetByEq(eqNo);
            return Ok(list);
        }
        
        [HttpGet("company/{companyNo}")]
        public IActionResult GetEQSpecAll(int companyNo)
        {
            var list = _eqSpecService.GetEQSpecAll(companyNo);
            return Ok(list);
        }

        [HttpPost("deletion/eq/{eqNo}/spec/{specNo}")]
        public async Task<IActionResult> DeleteSpec(int eqNo, int specNo)
        {
            await _eqSpecService.Delete(eqNo, specNo);
            return Ok();
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] EQSpec eQSpec)
        {
            eQSpec = await _eqSpecService.Insert(eQSpec);
            return Ok(eQSpec);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] EQSpec eQSpec)
        {
            await _eqSpecService.UpdateValue(eQSpec);
            return Ok(eQSpec);
        }
    }
}

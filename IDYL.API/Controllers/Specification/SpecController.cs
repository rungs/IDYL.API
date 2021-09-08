using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces.Service.Specification;
using IdylAPI.Models.Specification;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecController : ControllerBase
    {
        private readonly ISpecService _specService;

        public SpecController(ISpecService SpecService)
        {
            _specService = SpecService;
        }

        [HttpGet("company/{id}/delete/{isdelete}")]
        public IActionResult GetByCompany(int id, bool isdelete)
        {
            var posts = _specService.GetByCompany(id, isdelete);
            return Ok(posts);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateSpec([FromBody] Spec Specification)
        {
            Result newSite = await _specService.InsertSpec(Specification);
            if(newSite.StatusCode == 200) return Ok(newSite);
            return StatusCode(500, newSite);
        }

        [HttpPost("deletion/{id}/user/{userno}")]
        public async Task<IActionResult> DeleteSpec(int id, int userno)
        {
            await _specService.DeleteSpec(id, userno);
            return Ok();
        }

        [HttpGet("exclude/eqtype/{eqTypeNo}")]
        public IActionResult ExcludeEqType(int eqTypeNo)
        {
            var posts =  _specService.ExcludeEqType(eqTypeNo);
            return Ok(posts);
        }

        [HttpGet("exclude/eq/{eqNo}/eqType/{eqTypeNo}")]
        public IActionResult ExcludeEq(int eqNo, int eqTypeNo)
        {
            var posts = _specService.ExcludeEq(eqNo, eqTypeNo);
            return Ok(posts);
        }
    }
}

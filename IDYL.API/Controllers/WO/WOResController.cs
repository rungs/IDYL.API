using IdylAPI.Helper;
using IdylAPI.Models.WO;
using IdylAPI.Services.Interfaces.WO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdylAPI.Controllers.WO
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOResController : ControllerBase
    {
        private readonly IWOResRepository _woresRepository;
        public WOResController(IWOResRepository woresRepository)
        {
            _woresRepository = woresRepository;
        }

        
        [Authorize]
        [HttpPost("v1/Insert")]
        public OkObjectResult Insert([FromBody] List<WOResource> wOResource)
        {
            return Ok(_woresRepository.Insert(wOResource, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpDelete("v1/{id}")]
        public OkObjectResult Delete (int id)
        {
            return Ok(_woresRepository.Delete(id));
        }

        [Authorize]
        [HttpPost("v1/Copy")]
        public OkObjectResult Copy([FromBody] List<WOResource> wOResource)
        {
            return Ok(_woresRepository.Copy(wOResource, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

    }
}

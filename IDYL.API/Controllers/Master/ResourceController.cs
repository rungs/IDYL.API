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
    public class ResourceController : ControllerBase
    {

        private readonly IResourceRepository _resourceRepository;

        public ResourceController(IResourceRepository resourceRepository)
        {

            _resourceRepository = resourceRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_resourceRepository.Retrive(whereParameter));
        }

        [Authorize]
        [HttpPost("v1")]
        public OkObjectResult UpdateActual([FromBody] Resource resource)
        {
            return Ok(_resourceRepository.Insert(resource, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

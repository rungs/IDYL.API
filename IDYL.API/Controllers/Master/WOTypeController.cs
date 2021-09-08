using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOTypeController : ControllerBase
    {

        private readonly IWOTypeRepository _woTypeRepository;

        public WOTypeController(IWOTypeRepository woTypeRepository)
        {

            _woTypeRepository = woTypeRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_woTypeRepository.Retrive(whereParameter));
        }
    }
}

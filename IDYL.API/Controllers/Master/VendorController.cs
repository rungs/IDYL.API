using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {

        private readonly IVendorRepository _vendorRepository;

        public VendorController(IVendorRepository vendorRepository)
        {

            _vendorRepository = vendorRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_vendorRepository.Retrive(whereParameter));
        }
    }
}

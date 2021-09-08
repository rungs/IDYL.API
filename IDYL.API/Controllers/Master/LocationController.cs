using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;
using IdylAPI.Helper;
using IdylAPI.Models.Master;
using Swashbuckle.AspNetCore.Annotations;
using DevExtreme.AspNet.Data;

namespace IdylAPI.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {

        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_locationRepository.Retrive(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpPost("v1/update")]
        public OkObjectResult Insert([FromBody] Location location)
        {
            return Ok(_locationRepository.Insert(location, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [HttpGet(nameof(GetLocations))]
        [SwaggerOperation(Summary = "Get location all paging")]
        public IActionResult GetLocations(DataSourceLoadOptions loadOptions)
        {
           // var locations = _locationRepository.GetAll();//(parameters.);
            var locations = DataSourceLoader.Load(_locationRepository.GetAll(), loadOptions);
            //Result result = new Result()
            //{
            //    Data = locations.data,
            //    TotalCount = locations.totalCount
            //};
            
            return Ok(locations);
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace IdylAPI.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreqUnitController : ControllerBase
    {

        private readonly IFreqUnitService _freqUnitRepository;

        public FreqUnitController(IFreqUnitService freqUnitRepository)
        {
            _freqUnitRepository = freqUnitRepository;
        }

        [HttpGet("v1/FreqUnit")]
        public OkObjectResult GetAll()
        {
            return Ok(_freqUnitRepository.GetAll());
        }
    }
}

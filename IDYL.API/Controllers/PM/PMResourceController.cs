using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Syst;

namespace IdylAPI.Controllers.PM
{
    [Route("api/[controller]")]
    [ApiController]
    public class PMResourceController : ControllerBase
    {

        private readonly IPMResourceService _pmResourceRepository;

        public PMResourceController(IPMResourceService pmResourceRepository)
        {
            _pmResourceRepository = pmResourceRepository;
        }

        [HttpGet]
        public void GetData()
        {
            //return _pmResourceRepository.GetPMResourcePivotMTRequirement();
        }
    }
}

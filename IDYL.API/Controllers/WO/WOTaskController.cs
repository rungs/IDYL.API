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
    public class WOTaskController : ControllerBase
    {
        private readonly IWOTaskRepository _woTaskRepository;
        public WOTaskController(IWOTaskRepository woTaskRepository)
        {
            _woTaskRepository = woTaskRepository;
        }

        
        [Authorize]
        [HttpGet("v1/{woNo}")]
        public OkObjectResult Get(int woNo)
        {
            return Ok(_woTaskRepository.RetriveByWO(woNo));
        }

        [Authorize]
        [HttpPost("v1/Insert")]
        public OkObjectResult Insert(List<WOTask> tasks)
        {
            return Ok(_woTaskRepository.Insert(tasks, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpPost("v1/Abnormal")]
        public OkObjectResult UpdateAbnormal(WOTask tasks)
        {
            return Ok(_woTaskRepository.UpdateAbnormal(tasks, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
        
    }
}

using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.WO;
using IdylAPI.Services.Repository.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace IdylAPI.Controllers.WO
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOController : ControllerBase
    {
        private readonly IWORepository _woRepository;
        IHubContext<NotifyHub, ITypedHubClient> _chatHubContext;
        public WOController(IWORepository woRepository, IHubContext<NotifyHub, ITypedHubClient> chatHubContext)
        {
            _woRepository = woRepository;
            _chatHubContext = chatHubContext;
        }

        [HttpPost("v1/refresh")]
        public OkResult Refresh()
        {
            _chatHubContext.Clients.All.BroadcastMessage();
            return Ok();
        }

        [Authorize]
        [HttpGet("v1/All")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_woRepository.RetriveWO(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext), null));
        }

        [Route("v2/All")]
        [HttpPost]
        public OkObjectResult RetriveInprogress([FromQuery] WhereParameter whereParameter, [FromBody] LoadOptions loadOption)
        {
            return Ok(_woRepository.RetriveWO(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext), loadOption));
        }

        [Authorize]
        [HttpGet("v1/{id}")]
        public OkObjectResult GetById(int id)
        {
            return Ok(_woRepository.RetriveById(id, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpGet("v1/Plan/{woNo}")]
        public OkObjectResult GetPlan(int woNo)
        {
            return Ok(_woRepository.RetrivePlan(woNo));
        }

        [Authorize]
        [HttpGet("v1/Actual/{woNo}")]
        public OkObjectResult GetActual(int woNo)
        {
            return Ok(_woRepository.RetriveActual(woNo));
        }

        [Authorize]
        [HttpGet("v1/InspecFiles/{woNo}")]
        public OkObjectResult GetInspecFiles(int woNo)
        {
            return Ok(_woRepository.RetriveInspecFiles(woNo));
        }

        [Authorize]
        [HttpGet("v1/ViewFilter")]
        public OkObjectResult RetriveViewFilter([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_woRepository.RetriveViewFilter(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpPost("v1/DownloadToLocal")]
        public OkObjectResult RetriveWOToLocal([FromBody] List<Models.WO.WO> wos)
        {
            return Ok(_woRepository.RetriveWOToLocal(wos, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpPost("v1/Plan")]
        public OkObjectResult UpdatePlan([FromBody] Models.WO.WO wo)
        {
            return Ok(_woRepository.UpdatePlan(wo, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpPost("v1/Actual")]
        public OkObjectResult UpdateActual([FromBody] Models.WO.WO wo)
        {
            return Ok(_woRepository.UpdateActual(wo, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpPost("v1/Status/{woNo}/{woStatusNo}")]
        public OkObjectResult UpdateStatus(int woNo, int woStatusNo)
        {
            return Ok(_woRepository.UpdateStatus(woNo, woStatusNo, TokenHelper.DecodeTokenToInfo(HttpContext).CustomerNo));
        }


    }
}

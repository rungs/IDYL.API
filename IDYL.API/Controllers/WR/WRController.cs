using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces.WR;
using Microsoft.AspNetCore.SignalR;
using IdylAPI.Services.Repository.Hubs;
using IdylAPI.Services.Interfaces;

namespace IdylAPI.Controllers.WR
{
    [Route("api/[controller]")]
    [ApiController]
    public class WRController : ControllerBase
    {
        private readonly IWRRepository _wrRepository;
        IHubContext<NotifyHub, ITypedHubClient> _chatHubContext;
        public WRController(IWRRepository wrRepository, IHubContext<NotifyHub, ITypedHubClient> chatHubContext)
        {
            _wrRepository = wrRepository;
            _chatHubContext = chatHubContext;
        }

        [Authorize]
        [HttpGet("v1/All")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_wrRepository.Retrive(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [Authorize]
        [HttpGet("v1/{id}")]
        public OkObjectResult GetById(int id)
        {
            return Ok(_wrRepository.RetriveById(id));
        }

        [Authorize]
        [HttpPost("v1")]
        public OkObjectResult Insert([FromBody] Models.WO.WO wo)
        {
            Result result = _wrRepository.Insert(wo, TokenHelper.DecodeTokenToInfo(HttpContext));
            _chatHubContext.Clients.All.BroadcastMessage();
            return Ok(result);
        }

        [HttpPost("v1/CreateWR")]
        public OkObjectResult CreateWR([FromBody] Models.WO.WO wo)
        {
            Result result = _wrRepository.CreateWR(wo);
            return Ok(result);
        }

        [HttpGet("v1/ReportProblem")]
        public OkObjectResult RetriveForReportProblem([FromQuery] int siteNo, int systemNo)
        {
            Result result = _wrRepository.RetriveForReportProblem(siteNo, systemNo);
            return Ok(result);
        }


        [HttpGet("v1/TestPrint")]
        public IActionResult TestPrint()
        {
            Result result = new Result();
            return Redirect("http://localhost/IDYL/print.aspx?wono=375152");
            //Redirect("http://localhost/IDYL/print.aspx?wono=375152");
            //return Ok();
        }

        [HttpPost("CreateWR")]
        public OkObjectResult CreateWR([FromBody] DomainLayer.Entities.WR wr)
        {
            Result result = _wrRepository.CreateWR(wr);
            return Ok(result);
        }
    }
}

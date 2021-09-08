using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Helper;
using IdylAPI.Services.Interfaces.Dashboard;

namespace IdylAPI.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {

            _dashboardRepository = dashboardRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_dashboardRepository.RetriveBacklog(whereParameter.SiteNo.Value, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
    }
}

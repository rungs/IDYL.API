using Domain.Interfaces.Services.Syst;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Controllers.Syst
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginHistoryController : ControllerBase
    {
        private readonly ILoginHistoryService _loginHistoryService;

        public LoginHistoryController(ILoginHistoryService loginHistoryService)
        {
            _loginHistoryService = loginHistoryService;
        }

        [HttpGet(nameof(GetAllLoginHistory))]
        [SwaggerOperation(Summary = "Get all data of login history table")]
        public IActionResult GetAllLoginHistory()
        {
            return Ok(_loginHistoryService.GetAllLoginHistory());
        }
    }
}

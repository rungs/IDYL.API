using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Models.Syst;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystUserCustomerController : ControllerBase
    {
        private readonly ISystUserCustomerService _systUserCustomerService;

        public SystUserCustomerController(ISystUserCustomerService systUserCustomerService)
        {
            _systUserCustomerService = systUserCustomerService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] SystUserCustomer systUserCustomer)
        {
            await _systUserCustomerService.Insert(systUserCustomer);
            return Ok();
        }

        [HttpPost("delete/user/{userno}/customer/{customerno}")]
        public async Task<IActionResult> Delete(int userno, int customerno)
        {
            await _systUserCustomerService.Delete(userno, customerno);
            return Ok();
        }
    }
}

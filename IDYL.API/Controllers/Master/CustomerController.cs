using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Models.Syst;
using IdylAPI.Models.Master;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("user/{userno}")]
        public IActionResult GetByUser(int userno)
        {
            var obj = _customerService.GetByUser(userno);
            return Ok(obj);
        }

        [HttpGet("allexclude/user/{userNo}")]
        public IActionResult GetAllExclude(int userNo)
        {
            var obj = _customerService.GetAllExclude(userNo);
            return Ok(obj);
        }

        [HttpPost("v1/register")]
        public async Task<OkObjectResult> RegisterAsync(Customer customer)
        {
            var result = await _customerService.Register(customer);
            return Ok(result);
        }
    }
}

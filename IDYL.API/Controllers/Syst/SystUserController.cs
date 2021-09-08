using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Models.Syst;
using Swashbuckle.AspNetCore.Annotations;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystUserController : ControllerBase
    {
        private readonly ISystUserService _systUserService;

        public SystUserController(ISystUserService systUserService)
        {
            _systUserService = systUserService;
        }

        [HttpPost("userunlock/company/{companyNo}/noOfUser/{noOfUser}/userGroup/{userGroupId}")]
        public async Task<IActionResult> UnlockUser(int userGroupId, int noOfUser, int companyNo)
        {
            return Ok(await _systUserService.UnlockUser(userGroupId, noOfUser, companyNo));
        }

        [HttpPost("userunlock")]
        public async Task<IActionResult> CreateUserSubsite([FromBody] CreateUser userObj)
        {
            return Ok(await _systUserService.UnlockUser(userObj));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SystUser userObj)
        {
            await _systUserService.Update(id,userObj);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<SystUser> GetById(int id)
        {
            return await _systUserService.GetByid(id);
        }

        [HttpGet("company/{companyNo}")]
        public IActionResult GetCompanyies(int companyNo)
        {
            var obj = _systUserService.GetByCompany(companyNo);
            return Ok(obj);
        }

        [HttpPost("notseeauto_authenlocation/user/{userno}/isNotSeeAuto_AuthenLocation/{isNotSeeAuto_AuthenLocation}")]
        public async Task<IActionResult> UpdateNotSeeAutoAuthenLocation(int userno, string isNotSeeAuto_AuthenLocation)
        {
            await _systUserService.UpdateNotSeeAutoAuthenLocation(userno, isNotSeeAuto_AuthenLocation);
            return Ok();
        }

        [HttpGet("user/{productkey}")]
        public IActionResult GetUserByProductKey(string productkey)
        {
            var obj = _systUserService.GetUserByProductKey(productkey);
            return Ok(obj);
        }

        [HttpGet("user/{useno}/company/{companyno}")]
        public IActionResult GetByUserId(int useno, int companyno)
        {
            return Ok(_systUserService.GetByUserId(useno, companyno));
        }

        [HttpGet("activate/user/{useno}/company/{companyno}")]
        public IActionResult GetUserCustomerById(int useno, int companyno)
        {
            return Ok(_systUserService.GetUserCustomerById(useno, companyno));
        }
        
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateUser systUser)
        {
            return Ok(await _systUserService.ActivateUser(systUser));
        }

        [HttpPost("active/user/{userno}/company/{companyno}/active/{isactive}")]
        public async Task<IActionResult> UpdateUserActiveSite(int CompanyNo, bool IsActive, int UserNo)
        {
            return Ok(_systUserService.UpdateUserActiveSite(CompanyNo, IsActive, UserNo));
        }

        [HttpPost("mail/{userId}")]
        public async Task<IActionResult> SendMail(int userId)
        {
            return Ok(await _systUserService.SendEmail(userId));
        }
        
        [HttpGet(nameof(GetUserView))]
        [SwaggerOperation(Summary = "Get users for userview")]
        public IActionResult GetUserView()
        {
            return Ok(_systUserService.GetUserView());
        }
    }
}

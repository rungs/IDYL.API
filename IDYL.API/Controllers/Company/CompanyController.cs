using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Helper;
using IdylAPI.Services.Interfaces.Company;
using System.Threading.Tasks;
using IdylAPI.Models.Master;
using Microsoft.AspNetCore.OData.Query;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Company(Site site)
        {
            Site newSite = await _companyService.InsertCompany(site);
            return Ok(newSite);
        }

        /// <summary>
        /// Retrieve all posts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        public System.Collections.Generic.IEnumerable<Site> GetCompanyies()
        {
            var posts = _companyService.GetCompanyies();
            return posts;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var posts = await _companyService.GetCompanyById(id);

            return Ok(posts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Site companyObj)
        {
            await _companyService.UpdateCompany(companyObj, id);
            return Ok();
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetCompanyByUserId()
        {
            int userId = TokenHelper.DecodeTokenToInfo(HttpContext).UserNo;
            var res = _companyService.GetCompanyByUserId(userId);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("deletion/{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            await _companyService.DeleteCompany(id, TokenHelper.DecodeTokenToInfo(HttpContext));
            return Ok();
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSubsite([FromBody] Site companyObj)
        {
            await _companyService.UpdateSubsite(companyObj, TokenHelper.DecodeTokenToInfo(HttpContext));
            return Ok();
        }

        [Authorize]
        [HttpPost("insert")]
        public async Task<IActionResult> InsertSubsite([FromBody] Site companyObj)
        {
            await _companyService.InsertSubsite(companyObj, TokenHelper.DecodeTokenToInfo(HttpContext));
            return Ok();
        }

     
        [HttpPost("subsite")]
        public async Task<IActionResult> InsertSubsite2([FromBody] Site companyObj)
        {
            if(companyObj.CompanyNo == 0)
            {
                Site newSite = await _companyService.InsertSubsite(companyObj, null);
                return Ok(newSite);
            }
            else
            {
                await _companyService.UpdateSubsite(companyObj, null);
                return Ok();
            }
        }

        [HttpGet("productKey/{productkey}/user/{userid}")]
        public async Task<IActionResult> GetCompanyProductKeyUser(string productkey, int userid)
        {
            var posts =  _companyService.GetCompanyProductKeyUser(productkey, userid);
            return Ok(posts);
        }

        [HttpPost("clear/{companyNo}")]
        public IActionResult ClearData(int companyNo)
        {
             _companyService.ClearData(companyNo);
            return Ok();
        }
    }
}

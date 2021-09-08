using Microsoft.AspNetCore.Mvc;
using IdylAPI.Services.Interfaces.Service.Files;
using IdylAPI.Models.Img;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachFileController : ControllerBase
    {
        private readonly IAttachFileService _attachFileService;

        public AttachFileController(IAttachFileService attachFileService)
        {
            _attachFileService = attachFileService;
        }

        [HttpGet("inspection/link/{linkNo}")]
        public IActionResult GetByCompany(int linkNo)
        {
            var posts = _attachFileService.GetInsepctionFileByLinkNo(linkNo);
            return Ok(posts);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> AddFile(AttachFileObject attachFileObject)
        {
            await _attachFileService.AddFile(attachFileObject);
            return Ok(); 
        }

        [HttpGet("files/linkno/{linkno}")]
        public async Task<IActionResult>  GetFilesByLinkNo(int linkNo)
        {
            return  Ok(_attachFileService.GetAttachFilesByLinkNo(linkNo));
        }

        [Authorize]
        [HttpPost("deletion/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            await _attachFileService.DeleteFile(id);
            return Ok();
        }
    }
}

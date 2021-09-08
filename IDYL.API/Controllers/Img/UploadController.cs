using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdylAPI.Models.Authorize;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Helper;
using IdylAPI.Services.Repository.Img;
using IdylAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using IdylAPI.Models.Master;

namespace IdylAPI.Controllers.Img
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        private readonly IUploadRespository _uploadRepository;

        public UploadController(IUploadRespository uploadRepository)
        {
            _uploadRepository = uploadRepository;
        }

        // [Authorize]
        [HttpPost("v1/{type}")]///{site}v
        public OkObjectResult Upload(string type, [FromForm] IdylAPI.Models.Img.Upload upload)
        {
            return Ok(_uploadRepository.UploadImage(type, upload.SiteName, upload.Files));
        }

        [HttpPost("files")]
        public OkObjectResult UploadFiles(IFormFile file)
        {
             return Ok(_uploadRepository.UploadImage(file));
        }
    }
}

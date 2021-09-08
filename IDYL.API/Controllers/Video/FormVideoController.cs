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
using IdylAPI.Models.Syst;

namespace IdylAPI.Controllers.Video
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormVideoController : ControllerBase
    {
        
        private readonly IFormVideoRepository _formVideoRepository;

        public FormVideoController(IFormVideoRepository formVideoRepository)
        {
            _formVideoRepository = formVideoRepository;
        }

   
        [HttpGet("v1/form/{formId}")]
        public OkObjectResult RetriveByFormId(string formId)
        {
            return Ok(_formVideoRepository.RetriveByFormId(formId));
        }

        [HttpGet("v1/all")]
        public OkObjectResult RetriveAll()
        {
            return Ok(_formVideoRepository.RetriveAll());
        }

        [HttpPost("v1")]
        public OkObjectResult Insert([FromBody] FormVideo formVideo)
        {
            return Ok(_formVideoRepository.Insert(formVideo));
        }

        [HttpPut("v1/{formId}")]
        public OkObjectResult Update(string formId, [FromBody] FormVideo formVideo)
        {
            return Ok(_formVideoRepository.Update(formId, formVideo));
        }

        [HttpDelete("v1/{formId}")]
        public OkObjectResult Delete(string formId)
        {
            return Ok(_formVideoRepository.Delete(formId));
        }
    }
}

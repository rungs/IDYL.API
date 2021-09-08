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

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        
        private readonly IAuthorizeRepository _authorizeRepository;
        private readonly IConfiguration _configuration;
        public AuthorizeController(IAuthorizeRepository authorizeRepository, IConfiguration configuration)
        {
            _authorizeRepository = authorizeRepository;
            _configuration = configuration;
        }

     
        [HttpGet]
        public IEnumerable<string> GetData()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("v1/CanRegister")]
        public string GetCanRegister()
        {
            return _configuration["CanRegister"];
        }

        [HttpGet("v1/registerAddress")]
        public string GetRegisterServerAddress()
        {
            return _configuration["IdylRegisterAPI"];
        }

        [Authorize]
        [HttpGet("v1/userInfo/{siteNo}")]
        public OkObjectResult RetriveUserInfo(int siteNo)
        {
            return Ok(_authorizeRepository.RetriveUserInfo(TokenHelper.DecodeTokenToInfo(HttpContext), siteNo));
        }

        [HttpPost("v1/Login")]
        public OkObjectResult Post([FromQuery] LoginRequest login)
        {
            return Ok(_authorizeRepository.Login(login.Username, login.Password));
        }

        [HttpPost("v1/Logout")]
        public OkObjectResult Logout([FromQuery] int notifyTokenNo)
        {
            return Ok(_authorizeRepository.Logout(notifyTokenNo));
        }

        [HttpPost("v1/Login2")]
        public OkObjectResult Post2()
        {
            return Ok("login2");
        }

        [HttpPost("v1/refreshToken/{token}")]
        public OkObjectResult Post(string token)
        {
            return Ok(_authorizeRepository.RefreshToken(token));
        }

        [Authorize]
        [HttpPost("v1/reset")]
        public OkObjectResult Reset()
        {
            return Ok(_authorizeRepository.Reset(TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

    }
}

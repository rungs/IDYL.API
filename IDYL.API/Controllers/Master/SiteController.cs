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
using IdylAPI.Services.Interfaces.Master;

namespace IdylAPI.Controllers.Authorize
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {

        private readonly ISiteRepository _siteRepository;

        public SiteController(ISiteRepository siteRepository)
        {

            _siteRepository = siteRepository;
        }

        [Authorize]
        [HttpGet("v1")]
        public OkObjectResult Get()
        {
            return Ok(_siteRepository.Retrive(TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [HttpGet("v1/All")]
        public OkObjectResult GetAll()
        {
            return Ok(_siteRepository.RetriveAll());
        }
    }
}

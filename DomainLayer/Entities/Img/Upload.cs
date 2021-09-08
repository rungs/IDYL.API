using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Img
{
    public class Upload
    {
        [FromForm(Name = "Files")]
        public IEnumerable<IFormFile> Files { get; set; }
        [FromForm(Name = "SiteName")]
        public string SiteName { get; set; }
    }
}

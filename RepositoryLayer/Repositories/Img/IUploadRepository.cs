using IdylAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces
{
    public interface IUploadRespository
    {
        Result UploadImage(string type, string siteName, IEnumerable<IFormFile> files);
        Result UploadImage(IFormFile files);
    }
}

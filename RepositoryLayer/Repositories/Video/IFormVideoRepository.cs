using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Syst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Authorize
{
    public interface IFormVideoRepository
    {
        Result RetriveByFormId(string formId);
        Result RetriveAll();
        Result Insert(FormVideo formVideo);
        Result Update(string formId, FormVideo formVideo);
        Result Delete(string formId);
    }
}

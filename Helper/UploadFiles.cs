using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Helper
{
    public static class UploadFiles
    {
        public static string GenerateFolderUploadPath(string folderName, string companyName)
        {
            return $"Files/[{companyName.Replace(" ", "").Trim()}]/{folderName}";
        }
    }
}

using IdylAPI.Models;
using IdylAPI.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Img
{
    public class UploadRepository : IUploadRespository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        [Obsolete]
        private readonly IHostingEnvironment _host;

        [Obsolete]
        public UploadRepository(IConfiguration configuration, IHostingEnvironment host)
        {
            _host = host;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        [Obsolete]
        public Result UploadImage(string type, string siteName, IEnumerable<IFormFile> files)
        {
            Result result = new Result();
            try
            {
                foreach (IFormFile source in files)
                {
                    string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                    IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = filename.Substring(filename.LastIndexOf('.'));
                    string extension = ext.ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        result.StatusCode = 500;
                        result.ErrMsg = "Please Upload image of type .jpg,.gif,.png.";
                    }
                    else
                    {
                        string pPath = _host.ContentRootPath;
                        int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                        for (int j = 0; j <= pathLevel; j++)
                        {
                            pPath = Directory.GetParent(pPath).ToString();
                        }

                        pPath += _configuration["AttPath"] + "\\[" + siteName.Replace(" ", "").Trim() + "]\\" + type + "\\";
                        result.ErrMsg = pPath;
                        bool exists = System.IO.Directory.Exists(pPath);
                        if (!exists)
                        {
                            Directory.CreateDirectory(pPath);
                        }

                        string uidName = Guid.NewGuid().ToString();
                        string newFileName = pPath + uidName + extension;

                        while (File.Exists(newFileName))
                        {
                            uidName = Guid.NewGuid().ToString();
                            newFileName = pPath + uidName + Path.GetExtension(source.FileName);
                        }
                        using (FileStream output = System.IO.File.Create(newFileName))
                            source.CopyTo(output);

                        result.StatusCode = 200;
                        Image image = new Image();
                        image.DocName = source.FileName;
                        image.FileName = uidName + Path.GetExtension(source.FileName);
                        image.UidName = uidName;
                        image.PathName = newFileName;
                        result.Data = image;
                    }

                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        public Result UploadImage(IFormFile files)
        {
            Result result = new Result();
            try
            {
                string filename = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                var ext = filename.Substring(filename.LastIndexOf('.'));
                string extension = ext.ToLower();

                string pPath = _host.ContentRootPath;
                int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                for (int j = 0; j <= pathLevel; j++)
                {
                    pPath = Directory.GetParent(pPath).ToString();
                }

                pPath += _configuration["AttPath"] + "\\Activate\\";
                bool exists = Directory.Exists(pPath);
                if (!exists)
                {
                    Directory.CreateDirectory(pPath);
                }

                string uidName = Guid.NewGuid().ToString();
                string newFileName = pPath + filename.Replace(filename.Substring(filename.LastIndexOf('.')), "") +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                while (File.Exists(newFileName))
                {
                    uidName = Guid.NewGuid().ToString();
                    newFileName = pPath + uidName + Path.GetExtension(files.FileName);
                }
                using (FileStream output = System.IO.File.Create(newFileName))
                    files.CopyTo(output);

                result.StatusCode = 200;
                Image image = new Image();
                image.DocName = files.FileName;
                image.FileName = files.FileName;
                image.UidName = uidName;
                image.PathName = newFileName;
                image.Extension = Path.GetExtension(files.FileName);
                result.Data = image;

            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }

        //public void CheckFolderUpload(string folderName)
        //{
        //    string subPath = $"~/{folderName}/";

        //    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

        //    if (!exists)
        //    {
        //        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));
        //    }
        //}
        //public string GenerateFolderUploadPath(string folderName)
        //{
        //    return string.Format("Files/[" + BL.Data.User.GetCompanyName().Replace(" ", "").Trim() + "]/" + folderName);
        //}
    }
}

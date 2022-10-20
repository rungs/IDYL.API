using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace IDYL.API.Helper
{
    public class LogFile
    {
        public void AddtoLogFile(IConfiguration _configuration, string Message)
        {
            string LogPath = HttpContext.Current.Server.MapPath("~/log"); //.ApplicationPath;//ConfigurationManager.AppSettings["LogPath"].ToString();
            if (!System.IO.Directory.Exists(LogPath))
            {
                System.IO.Directory.CreateDirectory(LogPath);
            }
            string filename = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            string filepath = LogPath + "/" + filename;
            if (File.Exists(filepath))
            {
                using (StreamWriter writer = new StreamWriter(filepath, true))
                {
                    //writer.WriteLine("——————-START————-"+DateTime.Now);
                    //writer.WriteLine("Source:" + ErrorPage);
                    writer.WriteLine(DateTime.Now + ":" + Message);
                    //writer.WriteLine("——————-END————-" +DateTime.Now);
                }
            }
            else
            {
                StreamWriter writer = File.CreateText(filepath);
                //writer.WriteLine("——————-START————-"+DateTime.Now);
                //writer.WriteLine("Source:" +ErrorPage);
                writer.WriteLine(DateTime.Now + ":" + Message);
                // writer.WriteLine("——————-END————-"+DateTime.Now);
                writer.Close();
            }
        }
    }
}

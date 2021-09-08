using Microsoft.Extensions.Configuration;

namespace IdylAPI.Helper
{
    public class PlatformHelper
    {
        private readonly IConfiguration _configuration;
     
        public PlatformHelper(IConfiguration configuration)
        {

            _configuration = configuration;
         
        }

        public  string GetServerAddress(string platform, string customerType)
        {
            if (platform == "FM")
            {
                return customerType == "TRIAL" ? _configuration["IdylFMTrialAPI"] : _configuration["IdylFMAPI"];
            }
            else if (platform == "DEV")
            {
                return _configuration["IdylDevAPI"];
            }
            else if (platform == "QA")
            {
                return _configuration["IdylQaAPI"];
            }
            else if (platform == "MOU")
            {
                return "http://idylmobile.maintenancemanagement.net";
            }
            else
            {
                return customerType == "TRIAL" ? _configuration["IdylTrialAPI"] : _configuration["IdylPrdAPI"];
            }
        }
    }
}

using IdylAPI.Models.Notify;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdylAPI.Helper
{
    public class SendNotify
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public SendNotify(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public async Task<HttpResponseMessage> Send(string title, string body, string token, NotiData notiData)
        {
            var messageInformation = new PushNotify()
            {
                notification = new NotificationInfo()
                {
                    title = title,
                    body = body,
                    android_channel_id = "idylmobile-channel"
                },
                to = token,
                data = notiData
            };


            string jsonMessage = JsonConvert.SerializeObject(messageInformation);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send");
            request.Headers.TryAddWithoutValidation("Authorization", string.Format("key={0}", _configuration["firebaseSecret"]));
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
            HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                result = client.SendAsync(request).Result;
                //result = await client.SendAsync(request);
               
              
            }
            return result;
        }

        
    }
}

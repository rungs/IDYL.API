using FirebaseAdmin.Messaging;
using IdylAPI.Models.Notify;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public async Task Send(string title, string body, string token, NotiData notiData)
        {
            //var messageInformation = new PushNotify()
            //{
            //    notification = new NotificationInfo()
            //    {
            //        title = title,
            //        body = body,
            //        android_channel_id = "idylmobile-channel"
            //    },
            //    to = token,
            //    data = notiData
            //};

            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Data = new Dictionary<string, string>()
                {
                    ["docType"] = notiData.docType,
                    ["docNo"] = notiData.docNo.ToString(),
                    ["siteNo"] = notiData.siteNo.ToString(),
                    ["docCode"] = notiData.docCode,
                },
                Token = token,
                Android = new AndroidConfig()
                {
                    Notification = new AndroidNotification()
                    {
                        ChannelId = "idylmobile-channel", // 👈 This must match the channel created in the app
                        Sound = "default",
                        Priority = (NotificationPriority?)Priority.High
                    }
                }
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);

           
        }

        public void LineNotify(string msg, string token)
        {
            try
            {
                string message = System.Web.HttpUtility.UrlEncode(msg, Encoding.UTF8);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                string postData = string.Format("message={0}", message);
                byte[] data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public enum LineNotifyType
        {
            WRCreated,
            WOFinished,
            WOPlaned
        }

        public string GenerateMessageSendNotify(LineNotifyType notifyType, Models.WO.WO wOInfo, string linkDoc, string sectReq, Models.WO.WO wONew)
        {
            string msg = string.Empty;
            switch (notifyType)
            {
                case LineNotifyType.WRCreated:
                    string wrDate = wOInfo.WRDate.HasValue ? wOInfo.WRDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                    string woDate = wOInfo.WODate.HasValue ? wOInfo.WODate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                    msg = $"IDYL: {wOInfo.WOCode}\n รหัส/ชื่ออุปกรณ์:{wOInfo.EQCode};{wOInfo.EQName}\n รหัส/ชื่อสถานที่/หน่วยผลิต:{wOInfo.LocationCode};{wOInfo.LocationName}\n อาการ/ปัญหา: {wOInfo.WorkDesc}\n วันที่แจ้ง: {wrDate}\n วันที่เกิดปัญหา: {woDate} \n หน่วยงานแจ้ง: {sectReq} \n ผู้แจ้ง: {wOInfo.ReqNameText} \n เบอร์ผู้แจ้ง: {wOInfo.ReqPhone} \n อีเมล์ผู้แจ้ง: {wOInfo.ReqEmail} \n {linkDoc}";
                    break;
                case LineNotifyType.WOFinished:
                    string actFinishDate = Convert.ToDateTime(wONew.ActDate).ToString("dd/MM/yyyy HH:mm");
                    msg = $"IDYL: {wOInfo.WOCode}\n รหัส/ชื่ออุปกรณ์:{wOInfo.EQCode};{wOInfo.EQName}\n รหัส/ชื่อสถานที่/หน่วยผลิต:{wOInfo.LocationCode};{wOInfo.LocationName}\n อาการ/ปัญหา: {wOInfo.WorkDesc}\n วันที่เสร็จงาน: {actFinishDate}\n {linkDoc}";
                    break;
                case LineNotifyType.WOPlaned:
                    string plnDate = wONew.PlnDate.HasValue ? wONew.PlnDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                    msg = $"IDYL: {wOInfo.WOCode}\n รหัส/ชื่ออุปกรณ์:{wOInfo.EQCode};{wOInfo.EQName}\n รหัส/ชื่อสถานที่/หน่วยผลิต:{wOInfo.LocationCode};{wOInfo.LocationName}\n อาการ/ปัญหา: {wOInfo.WorkDesc} \n ประมาณวันเริ่มเวลา: {plnDate}\n {linkDoc}";
                    break;
                default:
                    break;
            }
            return msg;
        }
    }
}

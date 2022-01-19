using IdylAPI.Models;
using IdylAPI.Models.Syst;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using PAUtility;
using System;
using System.IO;

namespace IdylAPI.Helper
{
   
    public class MailReq
    {
        public string Receive { get; set; }
        public string Content { get; set; }
        public string FromPage { get; set; }
        public string DocCode { get; set; }
        public string DocLink { get; set; }
    }
    public static class Mail
    {
        public const string WO = "Work Order";
        public const string PM = "Work Order";
        public const string WR = "Work Request";

        [Obsolete]
        public static Result SendEmail(IConfiguration _configuration, MailReq mailReq)
        {
            Result result = new Result();
            try
            {
                //var contentRoot = Path.Combine(_configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "MailTemplate.html");

                string mailServer = _configuration["MailServer"];
                string senderMail = _configuration["SenderMail"];
                string password = _configuration["Password"];
                bool enableSSL = InputVal.ToBool(_configuration["EnableSSL"]);
                int port = InputVal.ToInt(_configuration["Port"]);

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(senderMail, senderMail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(mailReq.Receive, mailReq.Receive);
                message.To.Add(to);

                message.Subject = GenerateSubject(mailReq);
                
                BodyBuilder bodyBuilder = new BodyBuilder();
                string content = string.Format("<a href='{0}' target='_blank'>{1}</a>", mailReq.DocLink, message.Subject);
                content = content + "<br />" + mailReq.Content;
                bodyBuilder.HtmlBody = content;
                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect(mailServer, port, enableSSL);
                client.Authenticate(senderMail, password);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();

                result.StatusCode = 200;
                //_logger.LogDebug($"send email : @{user.Firstname + " " + user.Lastname} - @{user.Email}");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }

            return result;
        }

        public static string GenerateSubject(MailReq mailReq)
        {
            return string.Format("IDYL : {0} - {1}", GetPageFullName(mailReq.FromPage), mailReq.DocCode);
        }

        public static string GetPageFullName(string fromPage)
        {
            switch (fromPage)
            {
                case "WO":
                    return WO;
                case "WR":
                    return WR;
                default:
                    return "";
            }
        }
    }
    
}

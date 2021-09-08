using IdylAPI.Models;
using IdylAPI.Models.Syst;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using PAUtility;
using SocialMedia.Core.Services;
using System;
using System.IO;
using System.Net.Mail;

namespace IdylAPI.Helper
{
    public static class Mail
    {
        [Obsolete]
        public static Result SendEmail(IConfiguration _configuration, SystUser user, ILogger<SystUserService> _logger, string subject)
        {
            Result result = new Result();
            try
            {
                var contentRoot = Path.Combine(_configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "MailTemplate.html");

                string mailServer = _configuration["MailServer"];
                string senderMail = _configuration["SenderMail"];
                string password = _configuration["Password"];
                bool enableSSL = InputVal.ToBool(_configuration["EnableSSL"]);
                int port = InputVal.ToInt(_configuration["Port"]);

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(senderMail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(user.Firstname + " " + user.Lastname, user.Email);
                message.To.Add(to);

                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(contentRoot))
                {
                    bodyBuilder.HtmlBody = SourceReader.ReadToEnd();
                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("{name}", user.Firstname);
                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("{username}", user.Username);
                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody.Replace("{password}", user.Mobile);
                }

                message.Body = bodyBuilder.ToMessageBody();

                //SmtpClient client = new SmtpClient();
                //client.Connect(mailServer, port, enableSSL);
                //client.Authenticate(senderMail, password);

                //client.Send(message);
                //client.Disconnect(true);
                //client.Dispose();

                result.StatusCode = 200;
                _logger.LogDebug($"send email : @{user.Firstname + " " + user.Lastname} - @{user.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }

            return result;
        }
    }
}

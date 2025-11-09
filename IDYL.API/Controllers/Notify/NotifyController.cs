using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdylAPI.Helper;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Models.Notify;
using IdylAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLayer.Entities.Notify;
using FirebaseAdmin.Messaging;
using System;

namespace IdylAPI.Controllers.Notify
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {

        private readonly INotifyRepository _notifyRepository;

        public NotifyController(INotifyRepository notifyRepository)
        {

            _notifyRepository = notifyRepository;
        }

        [Authorize]
        [HttpGet("v1/Token")]
        public OkObjectResult Get()
        {
            return Ok(_notifyRepository.RetriveTokenByUser(TokenHelper.DecodeTokenToInfo(HttpContext).UserNo));
        }

        [Authorize]
        [HttpGet("v1/Inbox")]
        public OkObjectResult Get([FromQuery] WhereParameter whereParameter)
        {
            return Ok(_notifyRepository.RetriveInboxByUser(whereParameter, TokenHelper.DecodeTokenToInfo(HttpContext).CustomerNo));
        }

        [Authorize]
        [HttpPost("v1/Token")]
        public OkObjectResult Insert([FromBody] NotifyToken notifyToken)
        {
            return Ok(_notifyRepository.Insert(notifyToken.Token, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }

        [HttpPost("v1/SendMsg/company/{companyNo}/docno/{woNo}/userno/{customerNo}/action/{woaction}")]
        public OkObjectResult PushMsg(int companyNo, int woNo, int customerNo, string woaction)
        {
            return Ok(_notifyRepository.PushMsg(companyNo, woNo, customerNo, woaction));
        }

        [HttpPost("v1/SendMsg")]
        public OkObjectResult SendMsg([FromBody] NotiObj notiObj)
        {
            return Ok(_notifyRepository.PushMsg(notiObj));
        }

        [HttpPost("v1/SendAssignMsg")]
        public OkObjectResult SendAssignMsg([FromBody] IEnumerable<NotiObj> notiObjs)
        {
            return Ok(_notifyRepository.SendAssignMsg(notiObjs, TokenHelper.DecodeTokenToInfo(HttpContext)));
        }
        [HttpPost("v1/SendMessageAsync")]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageRequest request)
        {

            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = request.Title,
                    Body = request.Body,
                },
                Data = new Dictionary<string, string>()
                {
                    ["FirstName"] = "John",
                    ["LastName"] = "Doe"
                },
                Token = request.DeviceToken
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);

            if (!string.IsNullOrEmpty(result))
            {
                // Message was sent successfully
                return Ok("Message sent successfully!");
            }
            else
            {
                // There was an error sending the message
                throw new Exception("Error sending the message.");
            }
        }
    }
}

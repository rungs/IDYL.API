using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Notify
{
    public class PushNotify
    {
        public NotificationInfo notification { get; set; }
        public string to { get; set; }
        public NotiData data { get; set; }
    }
    public class NotificationInfo
    {
        public string title { get; set; }
        public string body { get; set; }
        public string android_channel_id { get; set; }
    }

    public class NotiData
    {
        public string docType { get; set; }
        public int docNo { get; set; }
        public int siteNo { get; set; }
        public string docCode { get; set; }
    }

    public class NotiObj
    {
        public string title { get; set; }
        public string body { get; set; }
        public int to { get; set; }
        public string receiveName { get; set; }
        public string docType { get; set; }
        public int docNo { get; set; }
        public int siteNo { get; set; }
        public string docCode { get; set; }
    }
}

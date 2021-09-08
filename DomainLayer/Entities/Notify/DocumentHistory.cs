using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Notify
{
    public class DocumentHistory : Base
    {
        public const string WORKREQEST_CREATE = "แจ้งซ่อม";
        public const string WORKREQEST_ACKNOWLEDGE = "Acknowledge การแจ้งซ่อม";
        public const string WORKORDER_CREATE = "ออกงาน WO";
        public const string PM_GENERATE_WO = "ออกงาน PM";
        public const string WORKORDER_PLAN = "วางแผน";
        public const string WORKORDER_FINISH = "เสร็จงาน";
        public const string WORKORDER_EVALUATE = "ประเมินการทำงาน";
        public const string WORKORDER_CANCEL = "ยกเลิก";
        public const string WORKORDER_CLOSE = "ปิดงาน";
        public const string WORKORDER_BACKLOG = "Backlog";
        public const string ASSIGN_TO = "Assign to";
        public const string ASSIGN_ACKNOWLEDGE = "Acknowledge Assign";
        public const string DONETASK = "Done task";

        

        public DateTime TimeStamp { get; set; }
        public int LinkDocNo { get; set; }
        public string LinkDocType { get; set; }
        public string Description { get; set; }
        public int? AssignFrom { get; set; }
        public int? AssignTo { get; set; }
        public int? DocFrom { get; set; }
        public string DocTypeFrom { get; set; }
        public int? DocTo { get; set; }
        public string DocTypeTo { get; set; }
        public string AssignMessage { get; set; }
        public int CreateBy { get; set; }
        public string DocCode { get; set; }
    }
}

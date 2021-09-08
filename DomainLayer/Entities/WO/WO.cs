using IdylAPI.Models.Img;
using System;
using System.Collections.Generic;

namespace IdylAPI.Models.WO
{
    public class WO : Base
    {
        public int WONo { get; set; }
        public string WOCode { get; set; }
        public string WRCode { get; set; }
        public DateTime? WODate { get; set; }
        public DateTime? WRDate { get; set; }
        public DateTime? ReqDate { get; set; }
        public string WorkDesc { get; set; }
        public int EQNo { get; set; }
        public string EQCode { get; set; }
        public string EQName { get; set; }
        public int LocationNo { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public int WOTypeNo { get; set; }
        public string WOTypeCode { get; set; }
        public string WOTypeName { get; set; }
        public int Requester { get; set; }
        public string ReqName { get; set; }
        public string ReqFirstName { get; set; }
        public int SectReq { get; set; }
        public string SecReqCode { get; set; }
        public string SecReqName { get; set; }
        public int SecProbNo { get; set; }
        public string SecProbName { get; set; }
        public int WOStatusNo { get; set; }
        public string WOStatusCode { get; set; }
        public string WOStatusName { get; set; }
        public int? ProblemTypeNo { get; set; }
        public string ProblemTypeName { get; set; }
        public int SectionNo { get; set; }
        public bool IsDowntime { get; set; }
        public string SecRespCode { get; set; }
        public string SecRespName { get; set; }
        public int? CraftNo { get; set; }
        public string CraftName { get; set; }
        public int? AssignTo { get; set; }
        public DateTime? PlnDate { get; set; }
        public TimeSpan? PlnTime { get; set; }
        public decimal PlnManHours { get; set; }
        public decimal PlnDuration { get; set; }
        public string WOAction { get; set; }
        public DateTime? ActDate { get; set; }
        public DateTime? ActDateStart { get; set; }
        public TimeSpan? ActTime { get; set; }
        public decimal ActManHours { get; set; }
        public decimal ActDuration { get; set; }
        public decimal Meter { get; set; }
        public string WOProblem { get; set; }
        public string WOCause { get; set; }
        public bool IsManualCost { get; set; }
        public decimal CostStock { get; set; }
        public decimal CostDirectPurchase { get; set; }
        public decimal CostTools { get; set; }
        public decimal CostOutsources { get; set; }
        public decimal CostCrafts { get; set; }
        public decimal CostMH { get; set; }
        public AttachFileObject[] Attachment { get; set; }
        public List<string> AttachmentObj { get; set; }
        public List<string> AttachmentAfterObj { get; set; }
        public AttachFileObject[] AttachmentBefore { get; set; }
        public AttachFileObject[] AttachmentAfter { get; set; }
        public DT DTs { get; set; }
        public FM fM { get; set; }
        public IEnumerable<WOResource> WOResPlanLabor { get; set; }
        public IEnumerable<WOResource> WOResPlanMat { get; set; }
        public IEnumerable<WOResource> WOResActLabor { get; set; }
        public IEnumerable<WOResource> WOResActMat { get; set; }
        public List<string> InspectFiles { get; set; }
        public int AttachFileCnt { get; set; }
        public string SubsiteCode { get; set; } 
        public int? SystemNo { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string Email { get; set; }
        public int TotalCount { get; set; }
        public string ReqNameText { get; set; }
        public string ReqPhone { get; set; }
        public string ReqEmail { get; set; }
        public int? EQTypeNo { get; set; }

    }
}

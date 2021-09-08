using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models
{
    public class WhereParameter : PagingParameters
    {
        public string UserToken { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public string Filter { get; set; }
        public string FilterPerson { get; set; }
        public int? SiteNo { get; set; }
        public int? PUNo { get; set; }
        public int? EQNo { get; set; }
        public int? WONo { get; set; }
        public int? EQTypeNo { get; set; }
        public string Type { get; set; }
        public int? ProblemNo { get; set; }
        public int? FailureModeNo { get; set; }
        public bool IsMaintainance { get; set; }
        public bool IsHistory { get; set; }
        public string DataType { get; set; }
        public string WOTypeCode { get; set; }
        public bool IsOverdue { get; set; }
        public bool IsOffline { get; set; }
        public int? PMNo { get; set; }
    }

    public class LoadOptions
    {
        public List<Sort> sorts { get; set; }
        public List<Filter> filters { get; set; }
    }

    public class Sort
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool isdesc { get; set; }
    }
    public class SortMaster
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class GroupFilter
    {
        public string groupName { get; set; }
        public string groupNameTH { get; set; }
        public List<Filter> filters { get; set; }
    }
    public class Filter
    {
        public string id { get; set; }
        public string text { get; set; }
        public string filtertype { get; set; }
        public string options { get; set; }
    }
}

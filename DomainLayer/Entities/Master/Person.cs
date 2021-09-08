using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    public class Person
    {
        public int CustomerNo { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public bool IsMaintainance { get; set; }
        public string CustomerCode { get; set; }
        public object Rate { get; set; }
        public object CraftTypeNo { get; set; }
        public object SectionNo { get; set; }
        public string LineToken { get; set; }
        public bool IsSendLine { get; set; }
        public bool IsSendEmail { get; set; }
        public bool IsHeadSection { get; set; }
        public bool IsHeadCraft { get; set; }
        public string CustomerName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string CraftTypeName { get; set; }
    }
}

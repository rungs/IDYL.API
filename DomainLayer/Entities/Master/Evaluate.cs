using IdylAPI.Models.Img;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.Master
{
    public class Evaluate
    {
        public int EvaluateNo { get; set; }
        public string EvaluateDesc { get; set; }
        public int Score { get; set; }
    }

    public class Sign : Base
    {
        public int SignNo { get; set; }
        public string SignatureIMG { get; set; }
        public DateTime? SignatureDateTime { get; set; }
        public int WONo { get; set; }
        public int CustomerNo { get; set; }
        public string Recommend { get; set; }
        public IEnumerable<Evaluate> Evaluate { get; set; }
        public AttachFileObject Attachment { get; set; }
    }
}

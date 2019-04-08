using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class ApIssue
    {
        public decimal Id { get; set; }
        public DateTime? Close { get; set; }
        public DateTime Open { get; set; }
        public decimal Prio { get; set; }
        public string Issue { get; set; }
        public decimal? ApId { get; set; }
        public decimal IssuetypId { get; set; }
        public string Userid { get; set; }

        public virtual Ap Ap { get; set; }
        public virtual Issuetyp Issuetyp { get; set; }
    }
}

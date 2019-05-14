using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class ApIssue
    {
        public long Id { get; set; }
        public string Issue { get; set; }
        public long Prio { get; set; }
        public DateTime Open { get; set; }
        public DateTime? Close { get; set; }
        public string Userid { get; set; }
        public long? ApId { get; set; }
        public long IssuetypId { get; set; }

        public virtual Ap Ap { get; set; }
        public virtual Issuetyp Issuetyp { get; set; }
    }
}

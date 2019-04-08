using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Issuetyp
    {
        public Issuetyp()
        {
            ApIssue = new HashSet<ApIssue>();
        }

        public decimal Id { get; set; }
        public decimal? Flag { get; set; }
        public string Issuetyp1 { get; set; }

        public virtual ICollection<ApIssue> ApIssue { get; set; }
    }
}

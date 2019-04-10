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

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }

        public virtual ICollection<ApIssue> ApIssue { get; set; }
    }
}

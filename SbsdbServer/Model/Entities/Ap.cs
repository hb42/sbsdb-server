using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Ap
    {
        public Ap()
        {
            ApIssue = new HashSet<ApIssue>();
            ApTag = new HashSet<ApTag>();
            Hw = new HashSet<Hw>();
        }

        public decimal Id { get; set; }
        public string Apname { get; set; }
        public string Bemerkung { get; set; }
        public string Bezeichnung { get; set; }
        public decimal? OeIdVerOe { get; set; }
        public decimal OeId { get; set; }
        public decimal ApklasseId { get; set; }

        public virtual Apklasse Apklasse { get; set; }
        public virtual Oe Oe { get; set; }
        public virtual Oe OeIdVerOeNavigation { get; set; }
        public virtual ICollection<ApIssue> ApIssue { get; set; }
        public virtual ICollection<ApTag> ApTag { get; set; }
        public virtual ICollection<Hw> Hw { get; set; }
    }
}

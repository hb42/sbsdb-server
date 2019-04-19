using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities {
    public class Ap {
        public Ap() {
            ApIssue = new HashSet<ApIssue>();
            ApTag = new HashSet<ApTag>();
            Hw = new HashSet<Hw>();
        }

        public long Id { get; set; }
        public string Apname { get; set; }
        public string Bezeichnung { get; set; }
        public string Bemerkung { get; set; }
        public long OeId { get; set; }
        public long? OeIdVerOe { get; set; }
        public long AptypId { get; set; }

        public virtual Aptyp Aptyp { get; set; }
        public virtual Oe Oe { get; set; }
        public virtual Oe OeIdVerOeNavigation { get; set; }
        public virtual ICollection<ApIssue> ApIssue { get; set; }
        public virtual ICollection<ApTag> ApTag { get; set; }
        public virtual ICollection<Hw> Hw { get; set; }
    }
}

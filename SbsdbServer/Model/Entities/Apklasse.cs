using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Apklasse
    {
        public Apklasse()
        {
            Ap = new HashSet<Ap>();
            Extprog = new HashSet<Extprog>();
            Tagtyp = new HashSet<Tagtyp>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }
        public long AptypId { get; set; }

        public virtual Aptyp Aptyp { get; set; }
        public virtual ICollection<Ap> Ap { get; set; }
        public virtual ICollection<Extprog> Extprog { get; set; }
        public virtual ICollection<Tagtyp> Tagtyp { get; set; }
    }
}

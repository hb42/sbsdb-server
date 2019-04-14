using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Aptyp
    {
        public Aptyp()
        {
            Ap = new HashSet<Ap>();
            Extprog = new HashSet<Extprog>();
            Hwtyp = new HashSet<Hwtyp>();
            Tagtyp = new HashSet<Tagtyp>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }

        public virtual ICollection<Ap> Ap { get; set; }
        public virtual ICollection<Extprog> Extprog { get; set; }
        public virtual ICollection<Hwtyp> Hwtyp { get; set; }
        public virtual ICollection<Tagtyp> Tagtyp { get; set; }
    }
}

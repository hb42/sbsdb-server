using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Aptyp
    {
        public Aptyp()
        {
            Apklasse = new HashSet<Apklasse>();
            Hwtyp = new HashSet<Hwtyp>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }

        public virtual ICollection<Apklasse> Apklasse { get; set; }
        public virtual ICollection<Hwtyp> Hwtyp { get; set; }
    }
}

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

        public decimal Id { get; set; }
        public string Aptyp1 { get; set; }
        public decimal? Flag { get; set; }

        public virtual ICollection<Apklasse> Apklasse { get; set; }
        public virtual ICollection<Hwtyp> Hwtyp { get; set; }
    }
}

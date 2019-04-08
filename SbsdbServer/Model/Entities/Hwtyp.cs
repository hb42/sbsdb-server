using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Hwtyp
    {
        public Hwtyp()
        {
            Hwkonfig = new HashSet<Hwkonfig>();
        }

        public decimal Id { get; set; }
        public string Hwtyp1 { get; set; }
        public decimal? AptypId { get; set; }
        public decimal? Flag { get; set; }

        public virtual Aptyp Aptyp { get; set; }
        public virtual ICollection<Hwkonfig> Hwkonfig { get; set; }
    }
}

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

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }
        public long AptypId { get; set; }

        public virtual Aptyp Aptyp { get; set; }
        public virtual ICollection<Hwkonfig> Hwkonfig { get; set; }
    }
}

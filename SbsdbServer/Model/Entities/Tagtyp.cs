using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Tagtyp
    {
        public Tagtyp()
        {
            ApTag = new HashSet<ApTag>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }
        public string Param { get; set; }
        public long AptypId { get; set; }

        public virtual Aptyp Aptyp { get; set; }
        public virtual ICollection<ApTag> ApTag { get; set; }
    }
}

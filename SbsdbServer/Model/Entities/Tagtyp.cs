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
        public long ApklasseId { get; set; }

        public virtual Apklasse Apklasse { get; set; }
        public virtual ICollection<ApTag> ApTag { get; set; }
    }
}

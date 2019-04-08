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

        public decimal Id { get; set; }
        public string TagTyp1 { get; set; }
        public decimal? Flag { get; set; }
        public string Param { get; set; }
        public decimal ApklasseId { get; set; }

        public virtual Apklasse Apklasse { get; set; }
        public virtual ICollection<ApTag> ApTag { get; set; }
    }
}

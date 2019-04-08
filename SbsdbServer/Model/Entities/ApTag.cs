using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class ApTag
    {
        public decimal Id { get; set; }
        public string TagText { get; set; }
        public decimal TagtypId { get; set; }
        public decimal ApId { get; set; }

        public virtual Ap Ap { get; set; }
        public virtual Tagtyp Tagtyp { get; set; }
    }
}

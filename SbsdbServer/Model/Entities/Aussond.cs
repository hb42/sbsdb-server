using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Aussond
    {
        public decimal Id { get; set; }
        public DateTime? AnschDat { get; set; }
        public decimal? AnschWert { get; set; }
        public DateTime? AussDat { get; set; }
        public string AussGrund { get; set; }
        public string InvNr { get; set; }
        public decimal HwkonfigId { get; set; }
        public string Mac { get; set; }
        public string Smbiosguid { get; set; }
        public string SerNr { get; set; }
        public string WartungBem { get; set; }
        public string WartungFa { get; set; }
        public DateTime? Rewe { get; set; }
        public string Bemerkung { get; set; }

        public virtual Hwkonfig Hwkonfig { get; set; }
    }
}

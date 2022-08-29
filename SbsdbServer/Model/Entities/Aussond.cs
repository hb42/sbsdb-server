using System;

namespace hb.SbsdbServer.Model.Entities
{
    public class Aussond
    {
        public long Id { get; set; }
        public string SerNr { get; set; }
        public DateTime? AnschDat { get; set; }
        public string InvNr { get; set; }
        public decimal? AnschWert { get; set; }
        public long HwkonfigId { get; set; }
        public string Mac { get; set; }
        public string Smbiosguid { get; set; }
        public string WartungFa { get; set; }
        public string Bemerkung { get; set; }
        public DateTime? AussDat { get; set; }
        public string AussGrund { get; set; }
        public DateTime? Rewe { get; set; }

        public virtual Hwkonfig Hwkonfig { get; set; }
    }
}

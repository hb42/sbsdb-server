using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Hwhistory
    {
        public decimal Id { get; set; }
        public decimal ApId { get; set; }
        public string Betriebsstelle { get; set; }
        public string ApBezeichnung { get; set; }
        public string Direction { get; set; }
        public string ApName { get; set; }
        public DateTime Shiftdate { get; set; }
        public decimal? HwId { get; set; }

        public virtual Hw Hw { get; set; }
    }
}

using System;

namespace hb.SbsdbServer.Model.Entities {
    public class Hwhistory {
        public long Id { get; set; }
        public long ApId { get; set; }
        public string Betriebsstelle { get; set; }
        public string ApBezeichnung { get; set; }
        public string Direction { get; set; }
        public string Apname { get; set; }
        public DateTime Shiftdate { get; set; }
        public long HwId { get; set; }

        public virtual Hw Hw { get; set; }
    }
}

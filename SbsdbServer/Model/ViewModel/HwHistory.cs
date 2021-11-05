using System;

namespace hb.SbsdbServer.Model.ViewModel {
    public class HwHistory {
        public long Id { get; set; }
        public string Apname { get; set; }
        public string Apbezeichnung { get; set; }
        public string Betriebsstelle { get; set; }
        public string Direction { get; set; }
        public DateTime Shiftdate { get; set; }
        public long Hwid { get; set; }
        public long Apid { get; set; }
    }
}

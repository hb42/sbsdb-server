namespace hb.SbsdbServer.Model.ViewModel {
    public class HwVlanChange {
        public long HwMacId { get; set; } // null/0 -> new
        public string Mac { get; set; } // null/"" -> del
        public long VlanId { get; set; }
        public long Ip { get; set; }
    }
}

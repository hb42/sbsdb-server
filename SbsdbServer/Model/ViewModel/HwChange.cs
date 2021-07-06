namespace hb.SbsdbServer.Model.ViewModel {
    public class HwChange {
        public long Apid { get; set; }
        public long? NewpriId {get; set; } // exist -> 0 == no HW, >0 == new HW
        public HwVlanChange[] PriVlans { get; set; } // new/del/chg
        public HwPeriChange[] Periph { get; set; } // new/del/chg
    }
}

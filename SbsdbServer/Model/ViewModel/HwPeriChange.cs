namespace hb.SbsdbServer.Model.ViewModel {
    public class HwPeriChange {
        public long HwId { get; set; }
        public bool del { get; set; } // true: del hwId, false: hwId exist -> chg vlans else new hwId/vlans
        public HwVlanChange[] vlans { get; set; } // new/chg/del
    }
}

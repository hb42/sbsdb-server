namespace hb.SbsdbServer.Model.ViewModel {
    public class HwTransport {
        public Hardware Hw { get; set; }
        public long DelHwId { get; set; } // >0 -> DEL HW
    }
}

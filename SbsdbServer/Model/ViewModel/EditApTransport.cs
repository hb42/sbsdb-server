namespace hb.SbsdbServer.Model.ViewModel {
    public class EditApTransport {
        public long Id { get; set; }
        public ApChange Ap { get; set; }
        public HwChange Hw { get; set; }
        public TagChange[] Tags { get; set; }
        public bool DelAp { get; set; }
    }
}

namespace hb.SbsdbServer.Model.ViewModel {
    public class Betrst {
        public long BstId { get; set; }
        public string Betriebsstelle { get; set; }
        public long BstNr { get; set; }
        public string Tel { get; set; }
        public string Oeff { get; set; }
        public bool Ap { get; set; }
        public long? ParentId { get; set; }
        public long AdresseId { get; set; }
    }
}

namespace hb.SbsdbServer.Model.ViewModel {
    public class Betrst {
        public long BstId { get; set; }
        public string Betriebsstelle { get; set; }
        public long BstNr { get; set; }
        public string Fax { get; set; }
        public string Tel { get; set; }
        public string Oeff { get; set; }
        public bool Ap { get; set; }
        public long? ParentId { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Strasse { get; set; }
        public string Hausnr { get; set; }
    }
}

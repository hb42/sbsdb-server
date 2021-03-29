namespace hb.SbsdbServer.Model.ViewModel {
    public class HwKonfig {
        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public string Hersteller { get; set; }
        public string Hd { get; set; }
        public string Prozessor { get; set; }
        public string Ram { get; set; }
        public string Sonst { get; set; }
        public string Video { get; set; }

        public long HwTypId { get; set; }
        public string HwTypBezeichnung { get; set; }
        public long HwTypFlag { get; set; }

        public long ApKatId { get; set; }
        public string ApKatBezeichnung { get; set; }
        public long ApKatFlag { get; set; }
    }
}

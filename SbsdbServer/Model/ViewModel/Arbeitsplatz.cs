using System.Collections.Generic;

namespace hb.SbsdbServer.Model.ViewModel {
    public class Arbeitsplatz {
        public Arbeitsplatz() {
            Tags = new List<Tag>();
            Hw = new List<Hardware>();
            Vlan = new List<Netzwerk>();
        }

        public long ApId { get; set; }
        public string Apname { get; set; }
        public string Bezeichnung { get; set; }
        public string Aptyp { get; set; }
        public Betrst Oe { get; set; }
        public Betrst VerantwOe { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Hardware> Hw { get; set; }
        public List<Netzwerk> Vlan { get; set; }
    }
}

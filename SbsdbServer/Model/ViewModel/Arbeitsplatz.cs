using System.Collections.Generic;

namespace hb.SbsdbServer.Model.ViewModel {
    public class Arbeitsplatz {
        public Arbeitsplatz() {
            Tags = new List<Tag>();
            // Hw = new List<Hardware>();
            // Vlan = new List<Netzwerk>();
        }

        public long ApId { get; set; }
        public string Apname { get; set; }
        public string Bezeichnung { get; set; }
        public long ApTypId { get; set; }
        public string ApTypBezeichnung { get; set; }
        public long ApTypFlag { get; set; }
        public long ApKatId { get; set; }
        public string ApKatBezeichnung { get; set; }
        public long ApKatFlag { get; set; }
        public long OeId { get; set; }
        public long VerantwOeId { get; set; }
        public string Bemerkung { get; set; }
        public List<Tag> Tags { get; set; }
        // public List<Hardware> Hw { get; set; }
        // public List<Netzwerk> Vlan { get; set; }
    }
}

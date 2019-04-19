using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities {
    public class Hwkonfig {
        public Hwkonfig() {
            Aussond = new HashSet<Aussond>();
            Hw = new HashSet<Hw>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public string Hersteller { get; set; }
        public string Hd { get; set; }
        public string Prozessor { get; set; }
        public string Ram { get; set; }
        public string Sonst { get; set; }
        public string Video { get; set; }
        public long HwtypId { get; set; }

        public virtual Hwtyp Hwtyp { get; set; }
        public virtual ICollection<Aussond> Aussond { get; set; }
        public virtual ICollection<Hw> Hw { get; set; }
    }
}

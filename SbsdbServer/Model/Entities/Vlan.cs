using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities {
    public class Vlan {
        public Vlan() {
            Mac = new HashSet<Mac>();
        }

        public long Id { get; set; }
        public long Ip { get; set; }
        public long Netmask { get; set; }
        public string Bezeichnung { get; set; }

        public virtual ICollection<Mac> Mac { get; set; }
    }
}

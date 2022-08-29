using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public class Apkategorie
    {
        public Apkategorie() {
            Aptyp = new HashSet<Aptyp>();
            Hwtyp = new HashSet<Hwtyp>();
            Tagtyp = new HashSet<Tagtyp>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }

        public virtual ICollection<Aptyp> Aptyp { get; set; }
        public virtual ICollection<Hwtyp> Hwtyp { get; set; }
        public virtual ICollection<Tagtyp> Tagtyp { get; set; }
    }
}

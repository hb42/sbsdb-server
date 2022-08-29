using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public class Hwtyp
    {
        public Hwtyp() {
            Hwkonfig = new HashSet<Hwkonfig>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long Flag { get; set; }
        public long ApkategorieId { get; set; }

        public virtual Apkategorie Apkategorie { get; set; }
        public virtual ICollection<Hwkonfig> Hwkonfig { get; set; }
    }
}

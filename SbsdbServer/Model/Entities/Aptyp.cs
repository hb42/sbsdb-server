using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Aptyp
    {
        public Aptyp()
        {
            Ap = new HashSet<Ap>();
            Extprog = new HashSet<Extprog>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }
        public long ApkategorieId { get; set; }

        public virtual Apkategorie Apkategorie { get; set; }
        public virtual ICollection<Ap> Ap { get; set; }
        public virtual ICollection<Extprog> Extprog { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Adresse
    {
        public Adresse()
        {
            Oe = new HashSet<Oe>();
        }

        public long Id { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Strasse { get; set; }
        public string Hausnr { get; set; }

        public virtual ICollection<Oe> Oe { get; set; }
    }
}

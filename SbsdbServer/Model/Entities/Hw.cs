using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public class Hw
    {
        public Hw() {
            Hwhistory = new HashSet<Hwhistory>();
            Mac = new HashSet<Mac>();
        }

        public long Id { get; set; }
        public string SerNr { get; set; }
        public DateTime? AnschDat { get; set; }
        public string InvNr { get; set; }
        public decimal? AnschWert { get; set; }
        public string Smbiosguid { get; set; }
        public string WartungFa { get; set; }
        public string Bemerkung { get; set; }
        public bool Pri { get; protected set; }
        public long? ApId { get; protected set; }
        public long HwkonfigId { get; set; }

        public virtual Ap Ap { get; set; }
        public virtual Hwkonfig Hwkonfig { get; set; }
        public virtual ICollection<Hwhistory> Hwhistory { get; set; }
        public virtual ICollection<Mac> Mac { get; set; }

        /**
        /* die beiden Felder nur zusammen setzen
        /* bei jeder Aenderung auch einen Datensatz in Hwhistory anlegen
         **/
        public void ChangeAp(long? id, bool pri) {
            ApId = id;
            Pri = pri;
        }

    }
}

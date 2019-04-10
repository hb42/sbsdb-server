using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Extprog
    {
        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public string ExtprogName { get; set; }
        public string ExtprogPar { get; set; }
        public long? Flag { get; set; }
        public long ApklasseId { get; set; }

        public virtual Apklasse Apklasse { get; set; }
    }
}

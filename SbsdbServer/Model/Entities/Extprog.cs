using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Extprog
    {
        public decimal Id { get; set; }
        public string Extprog1 { get; set; }
        public string ExtprogName { get; set; }
        public string ExtprogPar { get; set; }
        public decimal? Flag { get; set; }
        public decimal ApklasseId { get; set; }

        public virtual Apklasse Apklasse { get; set; }
    }
}

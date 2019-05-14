using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Apklasse
    {
        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public long? Flag { get; set; }
        public decimal AptypId { get; set; }
    }
}

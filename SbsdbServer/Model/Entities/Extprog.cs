﻿using System;
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
        public long AptypId { get; set; }

        public virtual Aptyp Aptyp { get; set; }
    }
}

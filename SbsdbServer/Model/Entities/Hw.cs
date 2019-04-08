﻿using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Hw
    {
        public Hw()
        {
            Hwhistory = new HashSet<Hwhistory>();
            Mac = new HashSet<Mac>();
        }

        public decimal Id { get; set; }
        public DateTime? AnschDat { get; set; }
        public decimal? AnschWert { get; set; }
        public string InvNr { get; set; }
        public string Smbiosguid { get; set; }
        public bool Pri { get; set; }
        public string SerNr { get; set; }
        public string WartungBem { get; set; }
        public string WartungFa { get; set; }
        public decimal? ApId { get; set; }
        public decimal? HwkonfigId { get; set; }
        public string Bemerkung { get; set; }

        public virtual Ap Ap { get; set; }
        public virtual Hwkonfig Hwkonfig { get; set; }
        public virtual ICollection<Hwhistory> Hwhistory { get; set; }
        public virtual ICollection<Mac> Mac { get; set; }
    }
}

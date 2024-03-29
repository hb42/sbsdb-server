﻿using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.ViewModel {
    public class Hardware {
        public Hardware() {
            Vlans = new List<Netzwerk>();
        }
        
        public long Id { get; set; }
        public string Sernr { get; set; }
        public DateTime AnschDat { get; set; }
        public decimal AnschWert { get; set; }
        public string InvNr { get; set; }
        public string Smbiosgiud { get; set; }
        public string WartungFa { get; set; }
        public string Bemerkung { get; set; }
        public bool Pri { get; set; }
        public long HwKonfigId { get; set; }
        public long ApId { get; set; }
        public List<Netzwerk> Vlans { get; set; }
    }
}

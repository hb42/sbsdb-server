﻿namespace hb.SbsdbServer.Model.ViewModel {
    public class Netzwerk {
        public string Mac { get; set; }
        public long HwMacId { get; set; }
#nullable enable
        public long? VlanId { get; set; }
        public string? Bezeichnung { get; set; }
        public long? Vlan { get; set; }
        public long? Netmask { get; set; }
        public long? Ip { get; set; }
    }
}

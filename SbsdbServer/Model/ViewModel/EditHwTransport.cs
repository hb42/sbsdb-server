using System;

namespace hb.SbsdbServer.Model.ViewModel {
    public class EditHwTransport {
        public long Id { get; set; }
        public bool DelHw { get; set; } 
        public bool RemoveAp { get; set; }
        public HwVlanChange[] Vlans { get; set; }
#nullable enable   
        public string? Sernr { get; set; }
        public long? HwKonfigId { get; set; }
        public DateTime? AnschDat { get; set; }
        public decimal? AnschWert { get; set; }
        public string? InvNr { get; set; }
        public string? Smbiosgiud { get; set; }
        public string? WartungFa { get; set; }
        public string? Bemerkung { get; set; }
        public string? Aussonderung { get; set; }
    }
}

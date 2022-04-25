using System;

namespace hb.SbsdbServer.Model.ViewModel; 

public class NewHwTransport {
    public long KonfigId { get; set; }

    public DateTime AnschDat { get; set; }
    public decimal AnschWert { get; set; }
    public string InvNr { get; set; }
    public string WartungFa { get; set; }
    public string Bemerkung { get; set; }

    public NewHwDetail[] Devices { get; set; }
}

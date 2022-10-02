using System;

namespace hb.SbsdbServer.Model.ViewModel; 

public class Aussonderung {
    public long Id { get; set; }
    public string InvNr { get; set; }
    public DateTime? AnschDat { get; set; }
    public decimal? AnschWert { get; set; }
    public string Bezeichnung { get; set; } // concat(hersteller, ' - ', bezeichnung)
    public string SerNr { get; set; }
    public DateTime? AussDat { get; set; }
    public string AussGrund { get; set; }
    public DateTime? Rewe { get; set; }
}

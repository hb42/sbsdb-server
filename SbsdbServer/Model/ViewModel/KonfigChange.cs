namespace hb.SbsdbServer.Model.ViewModel; 

public class KonfigChange {
    public long Id { get; set; }
    public long HwTypId { get; set; } // new konfig only
    public string Bezeichnung { get; set; }
    public string Hersteller { get; set; }
#nullable enable  
    public string? Hd { get; set; }
    public string? Prozessor { get; set; }
    public string? Ram { get; set; }
    public string? Sonst { get; set; }
    public string? Video { get; set; }

}

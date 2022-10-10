namespace hb.SbsdbServer.Model.ViewModel; 

public class AussondResult {
    public long Meldung { get; set; } // Anzahl der Aussonderungen in der Meldung
    public long Del { get; set; } // geloeschte Aussonderungen aelter als 11 Jahre
}

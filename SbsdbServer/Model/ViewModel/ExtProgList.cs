namespace hb.SbsdbServer.Model.ViewModel; 

public class ExtProgList {
    public string Program { get; set; }
    public string Bezeichnung { get; set; }
    public string Param { get; set; }
    public long Flag { get; set; }
    public ExtprogAptyp[] Types { get; set; }
}

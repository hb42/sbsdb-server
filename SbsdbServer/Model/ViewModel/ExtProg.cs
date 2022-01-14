namespace hb.SbsdbServer.Model.ViewModel; 

public class ExtProg {
    public long Id { get; set; }
    public string Bezeichnung { get; set; }
    public string Program { get; set; }
    public string Param { get; set; }
    public long? Flag { get; set; }
    public long ApkategorieId { get; set; }
}

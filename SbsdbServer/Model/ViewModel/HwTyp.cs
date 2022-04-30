namespace hb.SbsdbServer.Model.ViewModel; 

public class HwTyp {
    public long Id { get; set; }
    public string Bezeichnung { get; set; }
    public long Flag { get; set; }
    public long ApKategorieId { get; set; }
    public string Apkategorie { get; set; }
}

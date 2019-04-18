using System.Collections.Generic;

namespace hb.SbsdbServer.Model.ViewModel {
  public class Hardware {
    public long Id { get; set; }
    public string Hersteller { get; set; }
    public string Bezeichnung { get; set; }
    public string Sernr { get; set; }
    public bool Pri { get; set; }
    public string Hwtyp { get; set; }
    public long HwtypFlag { get; set; }
  }
}

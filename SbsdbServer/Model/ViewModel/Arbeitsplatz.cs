using System.Collections.Generic;

namespace hb.SbsdbServer.Model.ViewModel {
  public class Arbeitsplatz {
    public long ApId { get; set; }
    public string Apname { get; set; }
    public string Bezeichnung { get; set; }
    public string Aptyp { get; set; }
    public Betrst Oe { get; set; }
    public Betrst VerantwOe { get; set; }
    public List<Tag> TypTags { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Hardware> Hw { get; set; }
  }
}

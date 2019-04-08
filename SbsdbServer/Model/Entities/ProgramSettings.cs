using System;
using System.ComponentModel.DataAnnotations;

namespace hb.SbsdbServer.Model.Entities {
  /*
   * Programm-Einstellungen
   */
  public class ProgramSettings {

    [Required]
    public long Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Schluessel darf nicht länger als 100 Stellen sein.")]
    public string Key { get; set; }

    [StringLength(2000, ErrorMessage = "Eintrag darf nicht länger als 2.000 Stellen sein.")]
    public string Value { get; set; }

  }
}

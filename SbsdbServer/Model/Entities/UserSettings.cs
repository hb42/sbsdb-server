using System.ComponentModel.DataAnnotations;

namespace hb.SbsdbServer.Model.Entities {
  /*
   * Benutzereinstellungen
   */
  public class UserSettings {

    public long Id { get; set; }
    // User-ID
    [Required]
    [StringLength(20, ErrorMessage = "Benutzerkennung darf nicht länger als 20 Stellen sein.")]
    public string Uid { get; set; }
    // Einstellungen 
    public User Settings { get; set; }

  }
}

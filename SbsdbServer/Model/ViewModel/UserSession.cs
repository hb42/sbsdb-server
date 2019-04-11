using System.ComponentModel.DataAnnotations;

namespace hb.SbsdbServer.Model.ViewModel {
  /*
   * Programmeinstellungen des Benutzers
   *   
   * Keine eigene Tabelle, werden in UserSettings als JSON gespeichert.
   * Dadurch muss nicht bei jedem zusaetzlichen Wert aus der Benutzeroberflaeche
   * die DB-Tabelle geaendert werden, sondern nur diese Klasse.  
   */
  public class UserSession {

    public UserSession() { }
    public UserSession(string uid) {
      UID = uid;
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Benutzerkennung muss für UserSession angegeben werden.")]
    [StringLength(20, ErrorMessage = "Benutzerkennung darf nicht länger als 20 Stellen sein.")]
    public string UID { get; set; }
    public string Path { get; set; }

  }
}

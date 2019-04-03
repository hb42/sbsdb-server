namespace hb.SbsdbServer.Model.Entities {
  /*
   * Programmeinstellungen des Benutzers
   *   
   * Keine eigene Tabelle, werden in UserSettings als JSON gespeichert.
   * Dadurch muss nicht bei jedem zusaetzlichen Wert aus der Benutzeroberflaeche
   * die DB-Tabelle geaendert werden, sondern nur diese Klasse.  
   */
  public class User {

    public string UID { get; set; }
    public string Path { get; set; }

  }
}

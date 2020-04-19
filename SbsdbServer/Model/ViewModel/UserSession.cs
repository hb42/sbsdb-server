using System.ComponentModel.DataAnnotations;

namespace hb.SbsdbServer.Model.ViewModel {
    /*
     * Programmeinstellungen des Benutzers
     *   
     * Keine eigene Tabelle, dieses Objekt dient dem Austausch mit dem Client.
     * Die Einstellungen werden in UserSettings als JSON gespeichert (Feld Settings).
     * Dadurch muss nicht bei jedem zusaetzlichen Wert aus der Benutzeroberflaeche
     * die DB-Tabelle geaendert werden.  
     */
    public class UserSession {
        public UserSession() {
        }

        public UserSession(string uid) {
            UID = uid;
        }

        // UserID wird im Repository eingetragen
        [Required(AllowEmptyStrings = false, ErrorMessage = "Benutzerkennung muss für UserSession angegeben werden.")]
        [StringLength(20, ErrorMessage = "Benutzerkennung darf nicht länger als 20 Stellen sein.")]
        public string UID { get; set; }

        // Zugriffsrechte, werden jeweils im Controller gesetzt
        public bool IsAdmin { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsHotline { get; set; }
        
        // Benutzereinstellungen als JSON-BLOB
        public string Settings { get; set; }
    }
}

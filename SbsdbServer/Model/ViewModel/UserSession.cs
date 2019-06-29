using System.ComponentModel.DataAnnotations;

namespace hb.SbsdbServer.Model.ViewModel {
    /*
     * Programmeinstellungen des Benutzers
     *   
     * Keine eigene Tabelle, dieses Objekt wird in UserSettings als JSON gespeichert.
     * Dadurch muss nicht bei jedem zusaetzlichen Wert aus der Benutzeroberflaeche
     * die DB-Tabelle geaendert werden, sondern nur diese Klasse.  
     */
    public class UserSession {
        public UserSession() {
        }

        public UserSession(string uid) {
            UID = uid;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Benutzerkennung muss für UserSession angegeben werden.")]
        [StringLength(20, ErrorMessage = "Benutzerkennung darf nicht länger als 20 Stellen sein.")]
        public string UID { get; set; }

        // Zugriffsrechte, werden jeweils im Controller gesetzt
        public bool IsAdmin { get; set; }
        public bool IsReadonly { get; set; }
        public bool IsHotline { get; set; }
        
        // Benutzereinstellungen
        
        public string Path { get; set; }
        
        // AP-Page
        public bool ShowStandort { get; set; }
        public ColumnFilter[] ApFilters { get; set; }
        public string ApSortColumn { get; set; }
        public string ApSortDirection { get; set; }
        public int ApPageSize { get; set; }

    }
}

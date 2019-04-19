using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Entities {
    public class UserSettings {
        // gueltiges Objekt sicherstellen
        private UserSession _settings;
        public long Id { get; set; }
        public string Userid { get; set; }

        public UserSession Settings {
            get => _settings ?? new UserSession(Userid);
            set => _settings = value ?? new UserSession(Userid);
        }
    }
}

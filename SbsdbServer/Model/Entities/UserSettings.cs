using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
  public partial class UserSettings {
    public long Id { get; set; }
    public string Userid { get; set; }
    // gueltiges Objekt sicherstellen
    private UserSession _settings;
    public UserSession Settings {
      get => _settings ?? new UserSession(Userid);
      set => _settings = value ?? new UserSession(Userid);
    }
  }
}

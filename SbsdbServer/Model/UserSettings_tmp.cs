﻿using hb.SbsdbServer.Model.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace hb.SbsdbServer.Model {
  /*
   * Benutzereinstellungen
   */
  public class UserSettings_tmp {

    [Required]
    public long Id { get; set; }
    // User-ID
    [Required]
    [StringLength(20, ErrorMessage = "Benutzerkennung darf nicht länger als 20 Stellen sein.")]
    public string Uid { get; set; }

    // Session Data 
    private UserSession _settings;
    public UserSession Settings {
      get => _settings ?? new UserSession(Uid);
      set => _settings = value ?? new UserSession(Uid);
    }

  }
}

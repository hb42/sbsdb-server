using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class UserSettings
    {
        public decimal Id { get; set; }
        public string Userid { get; set; }
        // Objekt wird als JSON-String abgelegt
        public UserSession Settings { get; set; }
    }
}

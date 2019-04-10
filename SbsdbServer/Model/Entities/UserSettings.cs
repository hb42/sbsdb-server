using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class UserSettings
    {
        public long Id { get; set; }
        public string Userid { get; set; }
        public UserSession Settings { get; set; }
  }
}

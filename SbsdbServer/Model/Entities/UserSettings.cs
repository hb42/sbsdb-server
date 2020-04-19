using System;
using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class UserSettings
    {
        public long Id { get; set; }
        public string Userid { get; set; }
        
        public string Settings { get; set; } // CLOB
    }
}

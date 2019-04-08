using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class UserSettings
    {
        public decimal Id { get; set; }
        public string Userid { get; set; }
        public string Settings { get; set; }
    }
}

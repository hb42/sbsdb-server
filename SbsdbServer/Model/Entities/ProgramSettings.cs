using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class ProgramSettings
    {
        public decimal Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Vlan
    {
        public Vlan()
        {
            Mac = new HashSet<Mac>();
        }

        public decimal Id { get; set; }
        public decimal Netmask { get; set; }
        public string Vlan1 { get; set; }
        public decimal Ip { get; set; }

        public virtual ICollection<Mac> Mac { get; set; }
    }
}

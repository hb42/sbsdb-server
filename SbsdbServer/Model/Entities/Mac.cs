using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Mac
    {
        public decimal Id { get; set; }
        public decimal HwId { get; set; }
        public string Mac1 { get; set; }
        public decimal? Ip { get; set; }
        public decimal VlanVlanId { get; set; }

        public virtual Hw Hw { get; set; }
        public virtual Vlan VlanVlan { get; set; }
    }
}

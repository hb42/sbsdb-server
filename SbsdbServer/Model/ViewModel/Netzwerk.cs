using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model.ViewModel {
  public class Netzwerk {
    public long VlanId { get; set; }
    public string Bezeichnung { get; set; }
    public long Vlan { get; set; }
    public long Netmask { get; set; }
    public long Ip { get; set; }
    public string Mac { get; set; }
  }
}

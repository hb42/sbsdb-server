using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model.ViewModel {
  public class ApQuery {
    public string Search { get; set; }
    public long[] Tags { get; set; }
    public string SearchMac { get; set; }
    public string SearchSernr { get; set; }
  }
}

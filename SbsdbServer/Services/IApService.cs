using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Services {
  public interface IApService {
    Arbeitsplatz GetAp(long id);
    List<Arbeitsplatz> GetAps(string search);
    List<Arbeitsplatz> ApsForOe(long oeid);
    List<Arbeitsplatz> QueryAps(ApQuery query);
  }
}

using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model.Repositories {
  public interface IApRepository {
    Arbeitsplatz GetAp(long id);
    List<Arbeitsplatz> GetAps(string search);
    List<Arbeitsplatz> QueryAps(ApQuery query);
  }
}

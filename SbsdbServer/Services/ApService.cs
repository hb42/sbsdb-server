using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
  public class ApService : IApService {

    private readonly IApRepository apRepository;

    public ApService(IApRepository repo) {
      apRepository = repo;
    }

    public Arbeitsplatz GetAp(long id) {
      List<Arbeitsplatz> aps = apRepository.GetAp(id);
      if (aps.Count == 1) {
        return aps[0];
      } else {
        return null; // Fehler Alt.: leere Liste bis zum Client liefern?
      }
    }

    public List<Arbeitsplatz> GetAps(string search) {
      return apRepository.GetAps(search);
    }

    public List<Arbeitsplatz> ApsForOe(long oeid) {
      return apRepository.ApsForOe(oeid);
    }

    public List<Arbeitsplatz> QueryAps(ApQuery query) {
      return apRepository.QueryAps(query);
    }
  }
}

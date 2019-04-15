using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.sbsdbv4.model;
using System.Collections.Generic;
using System.Linq;

namespace hb.SbsdbServer.Services {

  public class TestService {

    private readonly Sbsdbv4Context v4dbContext;
    private readonly v4Migration Migration;

    public TestService(Sbsdbv4Context sbsdbv4, v4Migration mig) {
      v4dbContext = sbsdbv4;
      Migration = mig;
    }

    public IEnumerable<Arbeitsplatz> GetAps(string query) {
      var aps = v4dbContext.SbsAp.Where(b => b.ApName.Contains(query))
                              .Select(a => new Arbeitsplatz {
                                Apname = a.ApName,
                                Bezeichnung = a.Bezeichnung,
                                Aptyp = a.ApklasseIndexNavigation.Apklasse,
                                Hw = a.SbsHw.Select(h => new Hardware{
                                  Hersteller = h.KonfigIndexNavigation.Hersteller,
                                  Bezeichnung = h.KonfigIndexNavigation.Bezeichnung,
                                  Sernr = h.SerNr
                                }).ToList()
                              });

      return aps.ToList();
      //return aps as IEnumerable<Ap>;
    }

    public string Migrate() {

      return Migration.Run();

    }
 
  }
}

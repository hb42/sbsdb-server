using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.sbsdbv4.model;
using hb.SbsdbServer.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace hb.SbsdbServer.Services {

  public class TestService {

    private readonly Sbsdbv4Context v4dbContext;
    private readonly v4Migration Migration;

    public TestService(Sbsdbv4Context sbsdbv4, v4Migration mig) {
      v4dbContext = sbsdbv4;
      Migration = mig;
    }

    public IEnumerable<Ap> GetAps(string query) {
      var aps = v4dbContext.SbsAp.Where(b => b.ApName.Contains(query))
                              .Select(a => new Ap {
                                Apname = a.ApName,
                                Bezeichnung = a.Bezeichnung,
                                Apklasse = a.ApklasseIndexNavigation.Apklasse,
                                Hw = a.SbsHw.Select(h => new Hardware{
                                  Hersteller = h.KonfigIndexNavigation.Hersteller,
                                  Bezeichnung = h.KonfigIndexNavigation.Bezeichnung,
                                  Sernr = h.SerNr
                                }).ToArray()
                              });

      return aps.ToList();
      //return aps as IEnumerable<Ap>;
    }

    public string Migrate() {

      return Migration.Run();

    }
 
  }
}

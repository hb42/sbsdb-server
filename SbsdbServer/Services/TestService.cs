using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.sbsdbv4.model;
using hb.SbsdbServer.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace hb.SbsdbServer.Services {

  public class TestService {

    private readonly Sbsdbv4Context dbContext;

    public TestService(Sbsdbv4Context sbsdb) {
      dbContext = sbsdb;
    }

    public IEnumerable<Ap> GetAps(string query) {
      var aps = dbContext.SbsAp.Where(b => b.ApName.Contains(query))
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

 
  }
}

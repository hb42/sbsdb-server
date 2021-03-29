using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model;
using hb.SbsdbServer.Model.ViewModel;
using hb.SbsdbServer.sbsdbv4.model;

namespace hb.SbsdbServer.Services {
    public class TestService {
        private readonly v4Migration _migration;

        private readonly Sbsdbv4Context _v4DbContext;

        public TestService(Sbsdbv4Context sbsdbv4, v4Migration mig) {
            _v4DbContext = sbsdbv4;
            _migration = mig;
        }

        public IEnumerable<Arbeitsplatz> GetAps(string query) {
            var aps = _v4DbContext.SbsAp.Where(b => b.ApName.Contains(query))
                .Select(a => new Arbeitsplatz {
                    Apname = a.ApName,
                    Bezeichnung = a.Bezeichnung,
                    // Aptyp = a.ApklasseIndexNavigation.Apklasse,
                    // Hw = a.SbsHw.Select(h => new Hardware {
                    //     Hersteller = h.KonfigIndexNavigation.Hersteller,
                    //     Bezeichnung = h.KonfigIndexNavigation.Bezeichnung,
                    //     Sernr = h.SerNr
                    // }).ToList()
                });

            return aps.ToList();
            //return aps as IEnumerable<Ap>;
        }

        public string Migrate() {
            return _migration.Run();
        }
    }
}

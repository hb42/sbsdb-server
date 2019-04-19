using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class ApService : IApService {
        private readonly IApRepository _apRepository;

        public ApService(IApRepository repo) {
            _apRepository = repo;
        }

        public Arbeitsplatz GetAp(long id) {
            var aps = _apRepository.GetAp(id);
            return aps.Count == 1 ? aps[0] : null;
        }

        public List<Arbeitsplatz> GetAps(string search) {
            return _apRepository.GetAps(search);
        }

        public List<Arbeitsplatz> ApsForOe(long oeid) {
            return _apRepository.ApsForOe(oeid);
        }

        public List<Arbeitsplatz> QueryAps(ApQuery query) {
            return _apRepository.QueryAps(query);
        }
    }
}

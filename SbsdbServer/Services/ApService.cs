using System;
using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class ApService : IApService {
        private readonly IApRepository _apRepository;
        private readonly IConfigRepository _configRepository;

        public ApService(IApRepository repo, IConfigRepository conf) {
            _apRepository = repo;
            _configRepository = conf;
        }

        public List<Arbeitsplatz> GetAll() {
            return _apRepository.GetAll();
        }

        public List<Arbeitsplatz> GetPage(int page, int pagesize) {
            // var ps = _configRepository.GetConfig(Const.AP_PAGE_SIZE);
            // var pagesize = 0;
            // if (!int.TryParse(ps, out pagesize) || pagesize == 0) {
            //     pagesize = 100;
            // }
            return _apRepository.GetPage(page, pagesize);
        }

        public Arbeitsplatz GetAp(long id) {
            var aps = _apRepository.GetAp(id);
            return aps.Count == 1 ? aps[0] : null;
        }

        public List<Arbeitsplatz> GetAps(string search) {
            return _apRepository.GetAps(search);
        }

        public List<Arbeitsplatz> QueryAps(ApQuery query) {
            return _apRepository.QueryAps(query);
        }

        public int GetCount() {
            return _apRepository.GetCount();
        }

        public ApTransport ChangeAp(EditApTransport apt) {
            return _apRepository.ChangeAp(apt);
        }
    }
}

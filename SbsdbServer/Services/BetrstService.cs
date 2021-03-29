using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class BetrstService: IBetrstService {
        private readonly IBetrstRepository _betrstRepository;

        public BetrstService(IBetrstRepository repo) {
            _betrstRepository = repo;
        }

        public List<Betrst> GetAll() {
            return _betrstRepository.GetAll();
        }

        public List<Betrst> GetBetrst(long id) {
            return _betrstRepository.GetBetrst(id);
        }
    }
}
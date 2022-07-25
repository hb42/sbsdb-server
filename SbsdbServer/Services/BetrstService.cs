using System.Collections.Generic;
using hb.SbsdbServer.Model.Entities;
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

        public EditOeTransport ChangeBetrst(EditOeTransport chg) {
            return _betrstRepository.ChangeBetrst(chg);
        }
        
        public List<Adresse> GetAdressen() {
            return _betrstRepository.GetAdressen();
        }

        public EditAdresseTransport ChangeAdresse(EditAdresseTransport chg) {
            return _betrstRepository.ChangeAdresse(chg);
        }
    }
}

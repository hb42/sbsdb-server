using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class HwKonfigService: IHwKonfigService {
        private readonly IHwKonfigRepository _hwKonfigRepository;

        public HwKonfigService(IHwKonfigRepository repo) {
            _hwKonfigRepository = repo;
        }
        
        public List<HwKonfig> GetAll() {
            return _hwKonfigRepository.GetAll();
        }

        public List<HwKonfig> GetHwKonfig(long id) {
            return _hwKonfigRepository.GetHwKonfig(id);
        }
        
        public HwKonfig ChangeKonfig(KonfigChange kc) {
            return _hwKonfigRepository.ChangeKonfig(kc);
        }

    }
}

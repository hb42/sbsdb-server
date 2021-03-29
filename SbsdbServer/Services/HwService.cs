using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class HwService: IHwService {
        private readonly IHwRepository _hwRepository;

        public HwService(IHwRepository repo) {
            _hwRepository = repo;
        }        
        public List<Hardware> GetAll() {
            return _hwRepository.GetAll();
        }

        public List<Hardware> GetHardware(long id) {
            return _hwRepository.GetHardware(id);
        }
    }
}

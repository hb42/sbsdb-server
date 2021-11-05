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

        public List<Hardware> GetPage(int page, int pageSize) {
            return _hwRepository.GetPage(page, pageSize);
        }

        public int GetCount() {
            return _hwRepository.GetCount();
        }

        public HwTransport ChangeHw(EditHwTransport hwt) {
            return _hwRepository.ChangeHw(hwt);
        }

        public List<HwHistory> GetHwHistoryFor(long hwid) {
            return _hwRepository.GetHwHistoryFor(hwid);
        }
    }
}

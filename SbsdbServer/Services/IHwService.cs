using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IHwService {
        List<Hardware> GetAll();
        List<Hardware> GetHardware(long id);
        public List<Hardware> GetPage(int page, int pageSize);
        public int GetCount();
        public HwTransport ChangeHw(EditHwTransport hwt);
        public List<HwHistory> GetHwHistoryFor(long hwid);
    }
}

using System.Collections.Generic;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IHwRepository {
        List<Hardware> GetAll();
        List<Hardware> GetHardware(long id);
        public List<Hardware> GetPage(int page, int pageSize);
        public List<Hardware> GetHwForAp(long apid);
        public List<HwTyp> GetHwTypes();
        public int GetCount();
        public HwTransport ChangeHw(EditHwTransport hwt);
        public AddHwTransport AddHw(NewHwTransport nhw);
        public List<HwHistory> GetHwHistoryFor(long hwid);
        public void ChangeAp(Hw hw, long? newapid, bool pri);
        public void ChangeVlan(long id, string mac, long vlanid, long ip, long hwid);
    }
}

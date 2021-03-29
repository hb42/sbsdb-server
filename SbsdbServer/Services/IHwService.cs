using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IHwService {
        List<Hardware> GetAll();
        List<Hardware> GetHardware(long id);
    }
}

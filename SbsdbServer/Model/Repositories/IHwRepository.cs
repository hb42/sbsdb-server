using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IHwRepository {
        List<Hardware> GetAll();
        List<Hardware> GetHardware(long id);

    }
}

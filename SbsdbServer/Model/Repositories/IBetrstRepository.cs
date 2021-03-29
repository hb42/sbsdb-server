using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IBetrstRepository {
        List<Betrst> GetAll();
        List<Betrst> GetBetrst(long id);
    }
}
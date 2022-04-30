using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IHwKonfigService {
        List<HwKonfig> GetAll();
        List<HwKonfig> GetHwKonfig(long id);
        public HwKonfig ChangeKonfig(KonfigChange kc);

    }
}

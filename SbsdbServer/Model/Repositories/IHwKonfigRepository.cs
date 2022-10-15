using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IHwKonfigRepository {
        List<HwKonfig> GetAll();
        List<HwKonfig> GetHwKonfig(long id);
        public HwKonfig ChangeKonfig(KonfigChange kc);
        public long? DelKonfig(long id);
        public long[] HwKonfigInAussond();

    }
}

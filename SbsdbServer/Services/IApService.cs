using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IApService {
        List<Arbeitsplatz> GetAll();
        List<Arbeitsplatz> GetPage(int page, int pagesize);
        Arbeitsplatz GetAp(long id);
        List<Arbeitsplatz> GetAps(string search);
        List<Arbeitsplatz> QueryAps(ApQuery query);
        int GetCount();
        ApTransport ChangeAp(EditApTransport apt);
        ApTransport ChangeApTyp(ChangeAptypTransport chg);
    }
}

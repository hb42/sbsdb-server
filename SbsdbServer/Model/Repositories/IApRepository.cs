using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IApRepository {
        List<Arbeitsplatz> GetAll();
        List<Arbeitsplatz> GetPage(int page, int pageSize);
        List<Arbeitsplatz> GetAp(long id);
        List<Arbeitsplatz> GetAps(string search);
        List<Arbeitsplatz> QueryAps(ApQuery query);
        int GetCount();
        ApTransport ChangeAp(EditApTransport apt); 
        ApTransport ChangeApTyp(ChangeAptypTransport chg);
    }
}

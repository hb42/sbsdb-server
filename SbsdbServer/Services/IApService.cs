﻿using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IApService {
        List<Arbeitsplatz> GetAll();
        List<Arbeitsplatz> GetPage(int page, int pagesize);
        Arbeitsplatz GetAp(long id);
        List<Arbeitsplatz> GetAps(string search);
        List<Arbeitsplatz> ApsForOe(long oeid);
        List<Arbeitsplatz> QueryAps(ApQuery query);
        List<TypTag> GetTypTags();
        int GetCount();
    }
}

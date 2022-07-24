using System.Collections.Generic;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IBetrstService {
        List<Betrst> GetAll();
        List<Betrst> GetBetrst(long id);
        public List<Adresse> GetAdressen();
    }
}

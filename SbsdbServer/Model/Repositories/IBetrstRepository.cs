using System.Collections.Generic;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IBetrstRepository {
        List<Betrst> GetAll();
        List<Betrst> GetBetrst(long id);
        public EditOeTransport ChangeBetrst(EditOeTransport chg);
        public List<Adresse> GetAdressen();
        public EditAdresseTransport ChangeAdresse(EditAdresseTransport chg);
    }
}

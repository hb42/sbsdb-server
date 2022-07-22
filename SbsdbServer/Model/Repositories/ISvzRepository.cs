using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories; 

public interface ISvzRepository {
    // public List<> GetAdressen();
    List<ApKategorie> GetApKat();
    List<ApTyp> GetApTypes();
    EditAptypTransport ChangeAptyp(EditAptypTransport chg);
    List<ExtProg> GetExtprog();
    bool ChangeExtprog(EditExtprogTransport chg);
    List<HwTyp> GetHwTypes();
    // public List<> GetOes();
    List<TagTyp> GetTagTypes();
    public EditTagtypTransport ChangeTagtyp(EditTagtypTransport chg);
    List<Vlan> GetVlans();

}

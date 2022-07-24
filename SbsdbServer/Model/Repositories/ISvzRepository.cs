using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories; 

public interface ISvzRepository {
    // public List<> GetAdressen();
    // public EditAdresseTransport ChangeAdresse(EditAdresseTransport chg);
    List<ApKategorie> GetApKat();
    public EditApkategorieTransport ChangeApkategorie(EditApkategorieTransport chg);
    List<ApTyp> GetApTypes();
    EditAptypTransport ChangeAptyp(EditAptypTransport chg);
    List<ExtProg> GetExtprog();
    bool ChangeExtprog(EditExtprogTransport chg);
    List<HwTyp> GetHwTypes();
    public EditHwtypTransport ChangeHwtyp(EditHwtypTransport chg);
    // public List<> GetOes();
    // public EditOeTransport ChangeOe(EditOeTransport chg);
    List<TagTyp> GetTagTypes();
    public EditTagtypTransport ChangeTagtyp(EditTagtypTransport chg);
    List<Vlan> GetVlans();
    // public EditVlanTransport ChangeVlan(EditVlanTransport chg);

}

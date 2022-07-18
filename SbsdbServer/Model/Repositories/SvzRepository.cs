using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Vlan = hb.SbsdbServer.Model.ViewModel.Vlan;

namespace hb.SbsdbServer.Model.Repositories; 

public class SvzRepository : ISvzRepository {
    private readonly SbsdbContext _dbContext;
    private readonly ILogger<SvzRepository> _log;

    public SvzRepository(SbsdbContext context, ILogger<SvzRepository> log) {
        _dbContext = context;
        _log = log;
    }
    
    // --- Adresse ---

    // TODO ViewModel.adresse fehlt
    // public List<> GetAdressen() {
    //     
    // }
    
    // --- ApKategorie ---
    
    public List<ApKategorie> GetApKat() {
        return _dbContext.Apkategorie
            .Select(a => new ApKategorie {
                Id = a.Id,
                Bezeichnung = a.Bezeichnung,
                Flag = a.Flag.Value
            })
            .ToList();
    }

    // --- ApTyp ---
    
    public List<ApTyp> GetApTypes() {
        return _dbContext.Aptyp
            .Select(a => new ApTyp {
                Id = a.Id,
                Bezeichnung = a.Bezeichnung,
                Flag = a.Flag,
                ApKategorieId = a.ApkategorieId,
                Apkategorie = a.Apkategorie.Bezeichnung,
            })
            .ToList();
    }

    public EditAptypTransport ChangeAptyp(EditAptypTransport chg) {
        Aptyp at;
        if (chg.Del) {
            // del
            at = _dbContext.Aptyp.Find(chg.Aptyp.Id);
            if (at != null) {
                _dbContext.Aptyp.Remove(at);
            }
        } else if (chg.Aptyp.Id == 0) {
            // new
            at = new Aptyp {
                Bezeichnung = chg.Aptyp.Bezeichnung,
                Flag = chg.Aptyp.Flag,
                ApkategorieId = chg.Aptyp.ApKategorieId
            };
            _dbContext.Aptyp.Add(at);
        } else {
            // chg  
            at = _dbContext.Aptyp.Find(chg.Aptyp.Id);
            if (at != null) {
                at.ApkategorieId = chg.Aptyp.ApKategorieId;
                at.Bezeichnung = chg.Aptyp.Bezeichnung;
                at.Flag = chg.Aptyp.Flag;
                _dbContext.Aptyp.Update(at);
            }
        }
        var rc = _dbContext.SaveChanges();
        if (rc == 1) {
            chg.Aptyp.Id = at.Id;
            return chg;
        }
        else {
            return null;
        }
    }
    
    // --- ExtProg ---
    
    public List<ExtProg> GetExtprog() {
        return _dbContext.Extprog
            .AsNoTracking()
            .Select(ext => new ExtProg {
                Id = ext.Id,
                Bezeichnung = ext.Bezeichnung,
                Program = ext.ExtprogName,
                Param = ext.ExtprogPar,
                Flag = ext.Flag,
                AptypId = ext.AptypId
            }).ToList();
    }

    public bool ChangeExtprog(EditExtprogTransport chg) {
        var count = 0;
        if (chg.OutDel != null) {
            // del
            foreach (var type in chg.OutDel.Types) {
                var ex = _dbContext.Extprog.Find(type.Id);
                if (ex != null) {
                    count++;
                    _dbContext.Extprog.Remove(ex);
                }
            }
        }

        if (chg.OutChg != null) {
            // change
            foreach (var type in chg.OutChg.Types) {
                var ex = _dbContext.Extprog.Find(type.Id);
                if (ex != null) {
                    count++;
                    ex.ExtprogName = chg.OutChg.Program;
                    ex.ExtprogPar = chg.OutChg.Param;
                    ex.Bezeichnung = chg.OutChg.Bezeichnung;
                    ex.Flag = chg.OutChg.Flag;
                    _dbContext.Extprog.Update(ex);
                }
            }
        }

        if (chg.OutNew != null) {
            // new
            foreach (var types in chg.OutNew.Types) {
                count++;
                var ex = new Extprog {
                    ExtprogName = chg.OutNew.Program,
                    ExtprogPar = chg.OutNew.Param,
                    Bezeichnung = chg.OutNew.Bezeichnung,
                    Flag = chg.OutNew.Flag,
                    AptypId = types.Aptyp.Id
                };
                _dbContext.Extprog.Add(ex);
            }
        }

        var check = _dbContext.SaveChanges();
        return check == count;
    }
    
    // --- HwTyp ---
    
    public List<HwTyp> GetHwTypes() {
        return _dbContext.Hwtyp
            .Select(a => new HwTyp {
                Id = a.Id,
                Bezeichnung = a.Bezeichnung,
                Flag = a.Flag,
                ApKategorieId = a.ApkategorieId,
                Apkategorie = a.Apkategorie.Bezeichnung,
            })
            .ToList();
    }

    // --- Oe ---
    
    // TODO ViewModel.oe fehlt
    // public List<> GetOes() {
    //     
    // }

    // --- TagTyp ---

    public List<TagTyp> GetTagTypes() {
        return _dbContext.Tagtyp
            .Select(t => new TagTyp {
                Id = t.Id,
                Bezeichnung = t.Bezeichnung,
                Flag = t.Flag,
                Param = t.Param,
                ApKategorieId = t.ApkategorieId,
                Apkategorie = t.Apkategorie.Bezeichnung,
            })
            .ToList();
    }
    
    // --- Vlan ---
    
    public List<Vlan> GetVlans() {
        return _dbContext.Vlan
            .Select(v => new Vlan {
                Id = v.Id,
                Ip = v.Ip,
                Netmask = v.Netmask,
                Bezeichnung = v.Bezeichnung,
            })
            .ToList();
    }

}

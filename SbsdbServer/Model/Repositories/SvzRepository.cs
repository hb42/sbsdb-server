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
    
    // public EditAdresseTransport ChangeAdresse(EditAdresseTransport chg) {
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

    public EditApkategorieTransport ChangeApkategorie(EditApkategorieTransport chg) {
        Apkategorie ak;
        if (chg.Del) {
            // del
            ak = _dbContext.Apkategorie.Find(chg.Apkategorie.Id);
            if (ak != null) {
                _dbContext.Apkategorie.Remove(ak);
            }
        } else if (chg.Apkategorie.Id == 0) {
            // new
            ak = new Apkategorie {
                Bezeichnung = chg.Apkategorie.Bezeichnung,
                Flag = chg.Apkategorie.Flag,
            };
            _dbContext.Apkategorie.Add(ak);
        } else {
            // chg  
            ak = _dbContext.Apkategorie.Find(chg.Apkategorie.Id);
            if (ak != null) {
                ak.Bezeichnung = chg.Apkategorie.Bezeichnung;
                ak.Flag = chg.Apkategorie.Flag;
                _dbContext.Apkategorie.Update(ak);
            }
        }
        var rc = _dbContext.SaveChanges();
        if (rc == 1) {
            if (!chg.Del) {
                var ret = _dbContext.Apkategorie.Where(a => a.Id == ak.Id)
                    .Select(a => new ApKategorie {
                        Id = a.Id,
                        Bezeichnung = a.Bezeichnung,
                        Flag = a.Flag.Value,
                    }).First();
                chg.Apkategorie = ret;
            }
            return chg;
        }
        else {
            return null;
        }
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
            if (!chg.Del) {
                var ret = _dbContext.Aptyp.Include(a => a.Apkategorie)
                    .Where(a => a.Id == at.Id)
                    .Select(a => new ApTyp {
                        Id = a.Id,
                        Bezeichnung = a.Bezeichnung,
                        Flag = a.Flag,
                        ApKategorieId = a.ApkategorieId,
                        Apkategorie = a.Apkategorie.Bezeichnung,
                    }).First();
                chg.Aptyp = ret;
            }
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

    public EditHwtypTransport ChangeHwtyp(EditHwtypTransport chg) {
        Hwtyp ht;
        if (chg.Del) {
            // del
            ht = _dbContext.Hwtyp.Find(chg.Hwtyp.Id);
            if (ht != null) {
                _dbContext.Hwtyp.Remove(ht);
            }
        } else if (chg.Hwtyp.Id == 0) {
            // new
            ht = new Hwtyp {
                Bezeichnung = chg.Hwtyp.Bezeichnung,
                Flag = chg.Hwtyp.Flag,
                ApkategorieId = chg.Hwtyp.ApKategorieId
            };
            _dbContext.Hwtyp.Add(ht);
        } else {
            // chg  
            ht = _dbContext.Hwtyp.Find(chg.Hwtyp.Id);
            if (ht != null) {
                ht.ApkategorieId = chg.Hwtyp.ApKategorieId;
                ht.Bezeichnung = chg.Hwtyp.Bezeichnung;
                ht.Flag = chg.Hwtyp.Flag;
                _dbContext.Hwtyp.Update(ht);
            }
        }
        var rc = _dbContext.SaveChanges();
        if (rc == 1) {
            if (!chg.Del) {
                var ret = _dbContext.Hwtyp.Include(a => a.Apkategorie)
                    .Where(a => a.Id == ht.Id)
                    .Select(a => new HwTyp {
                        Id = a.Id,
                        Bezeichnung = a.Bezeichnung,
                        Flag = a.Flag,
                        ApKategorieId = a.ApkategorieId,
                        Apkategorie = a.Apkategorie.Bezeichnung,
                    }).First();
                chg.Hwtyp = ret;
            }
            return chg;
        }
        else {
            return null;
        }
    }

    // --- Oe ---
    
    // TODO ViewModel.oe fehlt
    // public List<> GetOes() {
    //     
    // }
    
    // public EditOeTransport ChangeOe(EditOeTransport chg) {
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

    public EditTagtypTransport ChangeTagtyp(EditTagtypTransport chg) {
        Tagtyp tt;
        if (chg.Del) {
            // del
            tt = _dbContext.Tagtyp.Find(chg.Tagtyp.Id);
            if (tt != null) {
                _dbContext.Tagtyp.Remove(tt);
            }
        } else if (chg.Tagtyp.Id == 0) {
            // new
            tt = new Tagtyp {
                Bezeichnung = chg.Tagtyp.Bezeichnung,
                Param = chg.Tagtyp.Param,
                Flag = chg.Tagtyp.Flag,
                ApkategorieId = chg.Tagtyp.ApKategorieId
            };
            _dbContext.Tagtyp.Add(tt);
        }
        else {
            // chg
            tt = _dbContext.Tagtyp.Find(chg.Tagtyp.Id);
            tt.Bezeichnung = chg.Tagtyp.Bezeichnung;
            tt.Param = chg.Tagtyp.Param;
            tt.Flag = chg.Tagtyp.Flag;
            tt.ApkategorieId = chg.Tagtyp.ApKategorieId;
            _dbContext.Tagtyp.Update(tt);
        }
        var rc = _dbContext.SaveChanges();
        if (rc == 1) {
            if (!chg.Del) {
                var ret = _dbContext.Tagtyp.Include(t => t.Apkategorie)
                    .Where(t => t.Id == tt.Id)
                    .Select(t => new TagTyp {
                        Id = t.Id,
                        Bezeichnung = t.Bezeichnung,
                        Param = t.Param,
                        Flag = t.Flag,
                        ApKategorieId = t.ApkategorieId,
                        Apkategorie = t.Apkategorie.Bezeichnung
                    })
                    .First();
                chg.Tagtyp = ret;
            }

            return chg;
        } else {
            return null;
        }
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

    // public EditVlanTransport ChangeVlan(EditVlanTransport chg) {
    //     
    // }

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Vlan = hb.SbsdbServer.Model.ViewModel.Vlan;

namespace hb.SbsdbServer.Model.Repositories {
    public class ApRepository : IApRepository {
        private readonly SbsdbContext _dbContext;
        private readonly IHwRepository _hwRepository;
        private readonly ILogger<ApRepository> _log;

        public ApRepository(SbsdbContext context, IHwRepository hwRepository , ILogger<ApRepository> log) {
            _dbContext = context;
            _hwRepository = hwRepository;
            _log = log;
        }

        public List<Arbeitsplatz> GetAll() {
            return Convert(
                GetArbeitsplatzListQuery(
                    _dbContext.Ap
                ).ToList()
            );
        }
        public List<Arbeitsplatz> GetPage(int page, int pageSize) {  
            int skipRows = page * pageSize;  // page is zero based!
            var tmp = Convert(
                GetArbeitsplatzQuery(
                    _dbContext.Ap
                ).Skip(skipRows).Take(pageSize).ToList()
            );
            _log.LogDebug("GetPage " + page + " skip=" + skipRows + " take=" + pageSize + " length=" + tmp.Count());
            return tmp;
        }
        /*
         * Einzelnen AP anhand der ID holen
         * 
         * Liefert Liste: check auf leere Liste muss aufrufende Routine erledigen.
         */
        public List<Arbeitsplatz> GetAp(long id) {
            return Convert(
                GetArbeitsplatzQuery(
                    _dbContext.Ap.Where(ap => ap.Id == id)
                ).ToList()
            );
        }

        /*
         * Suchstring in AP-Name und AP-Bezeichnung suchen
         */
        public List<Arbeitsplatz> GetAps(string search) {
            var watch = Stopwatch.StartNew();
            var like = search.ToUpper();
            var aps = Convert(
                GetArbeitsplatzQuery(
                    _dbContext.Ap.Where(
                        a => a.Apname.ToUpper().Contains(like) || a.Bezeichnung.ToUpper().Contains(like))
                ).ToList()
            );
            watch.Stop();
            var delta = watch.ElapsedMilliseconds;
            _log.LogDebug($"--- select-query for search '{like}' took {delta}ms");
            return aps;
        }

        /*
         * AP anhand der Kriterien in query holen
         */
        public List<Arbeitsplatz> QueryAps(ApQuery query) {
            throw new NotImplementedException();
        }

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
        
        public int GetCount() {
            return _dbContext.Ap.Count();
        }

        public ApTransport ChangeAp(EditApTransport apt) {
            if (apt == null) {
                return null;
            }
            try {
                Ap ap;
                List<Hw> chg = new List<Hw>();
                if (apt.Id == 0) {
                    // ** neuer AP
                    if (apt.Ap.StandortId == null || apt.Ap.ApTypId == null) {
                        _log.LogDebug("Error for new AP: invalid idx oe: {oeid}, typ: {typid}", apt.Ap.StandortId, apt.Ap.ApTypId);
                        return null;
                    }
                    ap = new Ap {
                        Apname = apt.Ap.Apname,
                        Bezeichnung = apt.Ap.Bezeichnung,
                        Bemerkung = apt.Ap.Bemnerkung ?? "",
                        OeId = apt.Ap.StandortId.Value,
                        OeIdVerOe = apt.Ap.VerantwId,
                        AptypId = apt.Ap.ApTypId.Value
                    };
                    _dbContext.Ap.Add(ap);
                    _dbContext.SaveChanges();
                    ap = _dbContext.Ap.Include(a => a.Aptyp).First(a => a.Id == ap.Id);
                    apt.Id = ap.Id;
                    if ((ap.Aptyp.Flag & Const.FREMDE_HW) > 0) {
                        // ** fremde HW -> neue HW + MAC eintragen
                        var hwkonf = _dbContext.Hwkonfig.First(h =>
                            h.Hwtyp.Apkategorie.Aptyp.First(a => a.Id == ap.AptypId).ApkategorieId == h.Hwtyp.ApkategorieId && (h.Hwtyp.Flag & Const.FREMDE_HW) > 0);
                        var hw = new Hw {
                            HwkonfigId = hwkonf.Id,
                            SerNr = ap.Apname
                        };
                        hw.ChangeAp(ap.Id, true);
                        _dbContext.Hw.Add(hw);
                        _dbContext.SaveChanges();
                        apt.Hw.NewpriId = hw.Id;
                        foreach (var vlan in apt.Hw.PriVlans) {
                            _hwRepository.ChangeVlan(vlan.HwMacId, vlan.Mac, vlan.VlanId, vlan.Ip, hw.Id);
                        }
                    }
                } else {
                    ap = _dbContext.Ap.Include(a => a.Aptyp).First(a => a.Id == apt.Id);
                    if (apt.DelAp == false) {
                        // ** change AP
                        if (apt.Ap != null) {
                            if (apt.Ap.Apname != null) {
                                ap.Apname = apt.Ap.Apname;
                            }
                            if (apt.Ap.Bemnerkung != null) {
                                ap.Bemerkung = apt.Ap.Bemnerkung;
                            }
                            if (apt.Ap.Bezeichnung != null) {
                                ap.Bezeichnung = apt.Ap.Bezeichnung;
                            }
                            if (apt.Ap.StandortId != null) {
                                ap.OeId = apt.Ap.StandortId.Value;
                            }
                            if (apt.Ap.VerantwId != null) {
                                ap.OeIdVerOe = apt.Ap.VerantwId.Value;
                            }
                            _dbContext.Ap.Update(ap);
                        }

                        ChangeTags(apt);
                        chg = ChangeHw(apt);
                    } else {
                        // ** DEL AP
                        _dbContext.ApTag.RemoveRange(_dbContext.ApTag.Where(aptag => aptag.ApId == apt.Id));
                        var aphw = _dbContext.Hw.Include(h => h.Hwkonfig.Hwtyp).Where(h => h.ApId == apt.Id);
                        foreach (var h in aphw) {
                            if ((h.Hwkonfig.Hwtyp.Flag & Const.FREMDE_HW) > 0) {
                                _log.LogDebug("remove fremde HW");
                                var rc = new Hw {
                                    Id = h.Id,
                                    SerNr = null,
                                    AnschDat = null,
                                    InvNr = null,
                                    AnschWert = null,
                                    Smbiosguid = null,
                                    WartungFa = null,
                                    Bemerkung = null,
                                    // Pri = false,
                                    // ApId = null,
                                    HwkonfigId = 0,
                                    Ap = null,
                                    Hwkonfig = null,
                                    Hwhistory = null,
                                    Mac = null,
                                };
                                chg.Add(rc);
                                _dbContext.Mac.RemoveRange(_dbContext.Mac.Where(m => m.HwId == h.Id));
                                _dbContext.Hwhistory.RemoveRange(_dbContext.Hwhistory.Where(hh => hh.HwId == h.Id));
                                _dbContext.Hw.Remove(h);
                            } else {
                                chg.Add(RemoveHwFromAp(h));
                            }
                        }
                        _dbContext.Ap.Remove(ap);
                    }
                }
                
                _dbContext.SaveChanges(); // sichert alles in einer Transaction
                
                var aps = GetAp(apt.Id);
                var hws = _hwRepository.GetHwForAp(apt.Id);
                
                foreach (var ch in chg) {
                    if (!hws.Exists(h => h.Id == ch.Id)) {
                        if (ch.SerNr != null) {
                            hws.AddRange(_hwRepository.GetHardware(ch.Id));
                        } else {
                          // DEL HW
                          hws.Add(new Hardware {
                              Id = ch.Id,
                              Sernr = null,
                              AnschDat = default,
                              AnschWert = 0,
                              InvNr = null,
                              Smbiosgiud = null,
                              WartungFa = null,
                              Bemerkung = null,
                              // Pri = false,
                              HwKonfigId = 0,
                              // ApId = 0,
                              Vlans = null
                          });
                        }
                    }
                }
                return new ApTransport {
                    Ap = aps == null || aps.Count == 0 ? null : aps[0],
                    Hw = hws.ToArray(),
                    DelApId = apt.DelAp ? apt.Id : 0
                };
            } catch(Exception ex) {
                _log.LogError(ex, "Error in ChangeAp() ApId: {Id}", apt.Id);
                return null;
            }
        }

        private void ChangeTags(EditApTransport apt) {
            if (apt.Tags == null || apt.Tags.Length == 0) {
                return;
            }
            _log.LogDebug("change ap tags count " + apt.Tags.Length);
            foreach (var tag in apt.Tags) {
                if (tag.ApTagId == null) {
                    var nTag = new ApTag {
                        ApId = apt.Id,
                        TagtypId = tag.TagId ?? 0,
                        Text = tag.Text
                    };
                    _dbContext.ApTag.Add(nTag);
                } else if (tag.TagId == null) {
                    _dbContext.ApTag.Remove(_dbContext.ApTag.Find(tag.ApTagId));
                } else {
                  var cTag = _dbContext.ApTag.Find(tag.ApTagId);
                  cTag.TagtypId = tag.TagId ?? 0;
                  cTag.Text = tag.Text;
                  _dbContext.ApTag.Update(cTag);
                }              
            }
        }

        private List<Hw> ChangeHw(EditApTransport apt) {
            Hw hw = null;
            var rc = new List<Hw>();
            if (apt.Hw == null) {
                _log.LogDebug("HW Change: nothing to do");
                return rc;
            }
            var hwlist = _dbContext.Hw.Where(h => h.ApId == apt.Id && h.Pri == true).ToList();
            if (hwlist.Count > 1) {
                _log.LogDebug($"HW Change: ERROR {hwlist.Count} entries for pri HW @apId {hw.ApId}");
                return rc;
            } else if (hwlist.Count == 1) {
                hw = hwlist[0];
                rc.Add(hw);
            } 
            // pri hw changed
            if (apt.Hw.NewpriId != null) {
                // remove old pri 
                _log.LogDebug("HW Change: remove old pri");
                if (hw != null) {
                    RemoveHwFromAp(hw);
                } 
                if (apt.Hw.NewpriId != 0) {
                    var newhw = _dbContext.Hw.Find(apt.Hw.NewpriId);
                    // change pri
                    _log.LogDebug("HW Change: change pri");
                    _hwRepository.ChangeAp(newhw, apt.Id, true);
                    ResetVlans(newhw.Id); // zur Sicherheit (sollte eigentlich sauber sein)
                    _dbContext.Hw.Update(newhw);
                    rc.Add(newhw);
                    hw = newhw;
                }
            }
            // chg pri vlans
            foreach (var vlan in apt.Hw.PriVlans) {
                _log.LogDebug("HW Change: change pri vlans");
                _hwRepository.ChangeVlan(vlan.HwMacId, vlan.Mac, vlan.VlanId, vlan.Ip, hw.Id);
            }
            // periph.
            foreach (var peri in apt.Hw.Periph) {
                var phw = _dbContext.Hw.Find(peri.HwId);
                if (peri.del) {
                    phw = RemoveHwFromAp(phw);
                    _log.LogDebug("HW Change: remove peri #" + peri.HwId);
                } else {
                    if (phw.ApId != apt.Id) {
                        // change peri
                        _log.LogDebug("HW Change: change peri #" + peri.HwId + ", apid " + apt.Id + " hw.apid " + phw.ApId);
                        _hwRepository.ChangeAp(phw, apt.Id == 0 ? null : apt.Id, false);
                        ResetVlans(phw.Id);
                        _dbContext.Hw.Update(phw);
                    }
                    foreach (var vlan in peri.vlans) {
                        _log.LogDebug("HW Change: change peri vlan");
                        _hwRepository.ChangeVlan(vlan.HwMacId, vlan.Mac, vlan.VlanId, vlan.Ip, peri.HwId);
                    }
                }
                rc.Add(phw);
            }

            return rc;
        }

        private Hw RemoveHwFromAp(Hw hardware) {
            var hw = _dbContext.Hw.Include(h => h.Hwkonfig.Hwtyp).First(h => h.Id == hardware.Id);
            if ((hw.Hwkonfig.Hwtyp.Flag & Const.FREMDE_HW) > 0) {
                _log.LogDebug("remove fremde HW");
                var rc = new Hw {
                    Id = hw.Id,
                    SerNr = null,
                    AnschDat = null,
                    InvNr = null,
                    AnschWert = null,
                    Smbiosguid = null,
                    WartungFa = null,
                    Bemerkung = null,
                    // Pri = false,
                    // ApId = null,
                    HwkonfigId = 0,
                    Ap = null,
                    Hwkonfig = null,
                    Hwhistory = null,
                    Mac = null,
                };
                _dbContext.Mac.RemoveRange(_dbContext.Mac.Where(m => m.HwId == hw.Id));
                _dbContext.Hwhistory.RemoveRange(_dbContext.Hwhistory.Where(hh => hh.HwId == hw.Id));
                _dbContext.Hw.Remove(hw);
                return rc;
            } else {
                _hwRepository.ChangeAp(hw, null, false);
                ResetVlans(hw.Id);
                _dbContext.Hw.Update(hw);
                return hw;
            }
        }
        private void ResetVlans(long hwid) {
            var vlans = _dbContext.Mac.Where(m => m.HwId == hwid).ToList();
            foreach (var vlan in vlans) {
                vlan.Ip = 0;
                vlan.VlanId = null;
                _dbContext.Mac.Update(vlan);
            }
        }

        /*
         * Alle benoetigten Daten fuer einen Arbeitsplatz aus der DB holen
         * 
         * IN: Query auf Ap mit den notwendigen Bedingungen
         * OUT: Query ergaenzt um Select fuer die AP-Daten
         * 
         * Die Abfrage liefert tmpAP-Objekte, da das die effizienteste DB-Abfrage
         * ist. Anschließend muss das Objekt noch in ein Arbeitsplatz-Objekt 
         * umgewandelt werden.
         */
        private IQueryable<TmpAp> GetArbeitsplatzQuery(IQueryable<Ap> apq) {
            return apq
                .AsNoTracking()
                .OrderBy(ap => ap.Id)
                .Select(ap => new TmpAp {
                    ApId = ap.Id,
                    Apname = ap.Apname,
                    Bezeichnung = ap.Bezeichnung,
                    // Aptyp = ap.Aptyp.Bezeichnung,
                    Bemerkung = ap.Bemerkung,
                    OeId = ap.OeId,
                    ApTypId = ap.Aptyp.Id,
                    ApTypBezeichnung = ap.Aptyp.Bezeichnung,
                    ApTypFlag = ap.Aptyp.Flag,
                    ApKatId = ap.Aptyp.Apkategorie.Id,
                    ApKatBezeichnung = ap.Aptyp.Apkategorie.Bezeichnung,
                    ApKatFlag = ap.Aptyp.Apkategorie.Flag ?? 0,
                    //     new Betrst {
                    //     BstId = ap.Oe.Id,
                    //     Betriebsstelle = ap.Oe.Betriebsstelle,
                    //     BstNr = ap.Oe.Bst,
                    //     Fax = ap.Oe.Fax,
                    //     Tel = ap.Oe.Tel,
                    //     Oeff = ap.Oe.Oeff,
                    //     Ap = (bool) ap.Oe.Ap,
                    //     ParentId = ap.Oe.OeId,
                    //     Plz = ap.Oe.Adresse.Plz,
                    //     Ort = ap.Oe.Adresse.Ort,
                    //     Strasse = ap.Oe.Adresse.Strasse,
                    //     Hausnr = ap.Oe.Adresse.Hausnr
                    // },
                    VerantwOeId = ap.OeIdVerOe ?? 0, // == null || ap.OeIdVerOe == ap.OeId
                        // ? null
                        // : new Betrst {
                        //     BstId = ap.OeIdVerOeNavigation.Id,
                        //     Betriebsstelle = ap.OeIdVerOeNavigation.Betriebsstelle,
                        //     BstNr = ap.OeIdVerOeNavigation.Bst,
                        //     Fax = ap.OeIdVerOeNavigation.Fax,
                        //     Tel = ap.OeIdVerOeNavigation.Tel,
                        //     Oeff = ap.OeIdVerOeNavigation.Oeff,
                        //     Ap = (bool) ap.OeIdVerOeNavigation.Ap,
                        //     ParentId = ap.OeIdVerOeNavigation.OeId,
                        //     Plz = ap.OeIdVerOeNavigation.Adresse.Plz,
                        //     Ort = ap.OeIdVerOeNavigation.Adresse.Ort,
                        //     Strasse = ap.OeIdVerOeNavigation.Adresse.Strasse,
                        //     Hausnr = ap.OeIdVerOeNavigation.Adresse.Hausnr
                        // },
                    // Hw = ap.Hw.Select(hw => new TmpHw {
                    //     Id = hw.Id,
                    //     Hersteller = hw.Hwkonfig.Hersteller,
                    //     Bezeichnung = hw.Hwkonfig.Bezeichnung,
                    //     Sernr = hw.SerNr,
                    //     Pri = hw.Pri,
                    //     Hwtyp = hw.Hwkonfig.Hwtyp.Bezeichnung,
                    //     HwtypFlag = (long) hw.Hwkonfig.Hwtyp.Flag,
                    //     Vlan = hw.Mac.Select(m => new Netzwerk {
                    //         VlanId = m.VlanId,
                    //         Bezeichnung = m.Vlan.Bezeichnung,
                    //         Vlan = m.Vlan.Ip,
                    //         Netmask = m.Vlan.Netmask,
                    //         Ip = (long) m.Ip,
                    //         Mac = m.Adresse
                    //     }).ToList()
                    // }).ToList(),
                    Tags = ap.ApTag.Select(t => new Tag {
                        ApTagId = t.Id,
                        TagId = t.TagtypId,
                        Bezeichnung = t.Tagtyp.Bezeichnung,
                        Text = t.Text,
                        Param = t.Tagtyp.Param,
                        Flag = t.Tagtyp.Flag,
                        ApkategorieId = t.Tagtyp.ApkategorieId
                    }).ToList()
                });
        }

        /*
         * Liste der Arbeitsplaetze, es werden nur die Felder befuellt, die fuer
         * die Listendarstellung benoetigt werden. Der Rest muss mit Einzelabfragen
         * auf den jeweiligen AP geholt werden.
         */
        private IQueryable<TmpAp> GetArbeitsplatzListQuery(IQueryable<Ap> apq) {
            return apq
                .AsNoTracking()
                .OrderBy(ap => ap.Id)
                .Select(ap => new TmpAp {
                    ApId = ap.Id,
                    Apname = ap.Apname,
                    Bezeichnung = ap.Bezeichnung,
                    Aptyp = ap.Aptyp.Bezeichnung,
                    OeId = ap.Oe.Id,
                    ApTypId = ap.Aptyp.Id,
                    ApTypBezeichnung = ap.Aptyp.Bezeichnung,
                    ApTypFlag = ap.Aptyp.Flag,
                    ApKatId = ap.Aptyp.Apkategorie.Id,
                    ApKatBezeichnung = ap.Aptyp.Apkategorie.Bezeichnung,
                    ApKatFlag = ap.Aptyp.Apkategorie.Flag ?? 0,
                    //     new Betrst {
                    //     Betriebsstelle = ap.Oe.Betriebsstelle,
                    //     BstNr = ap.Oe.Bst,
                    // },
                    VerantwOeId = ap.OeIdVerOe ?? 0, // == null || ap.OeIdVerOe == ap.OeId
                        // ? null
                        // : new Betrst {
                        //     Betriebsstelle = ap.OeIdVerOeNavigation.Betriebsstelle,
                        //     BstNr = ap.OeIdVerOeNavigation.Bst,
                        // },
                    // Hw = ap.Hw.Where(hw => hw.Pri).Select(hw => new TmpHw {
                    //     Hersteller = hw.Hwkonfig.Hersteller,
                    //     Bezeichnung = hw.Hwkonfig.Bezeichnung,
                    //     Sernr = hw.SerNr,
                    //     Pri = hw.Pri,
                    //     Hwtyp = hw.Hwkonfig.Hwtyp.Bezeichnung,
                    //     HwtypFlag = (long) hw.Hwkonfig.Hwtyp.Flag,
                    //     Vlan = hw.Mac.Select(m => new Netzwerk {
                    //         VlanId = m.VlanId,
                    //         Bezeichnung = m.Vlan.Bezeichnung,
                    //         Vlan = m.Vlan.Ip,
                    //         Netmask = m.Vlan.Netmask,
                    //         Ip = (long) m.Ip,
                    //         Mac = m.Adresse
                    //     }).ToList()
                    // }).ToList()
                });
        }

        /*
         * tmpAp-Liste in Liste mit Arbeitsplatz-Objekten umwandeln.
         */
        private List<Arbeitsplatz> Convert(List<TmpAp> tmp) {
            var aps = new List<Arbeitsplatz>();
            foreach (var t in tmp) {
                var ap = new Arbeitsplatz {
                    ApId = t.ApId,
                    Apname = t.Apname,
                    Bezeichnung = t.Bezeichnung,
                    Bemerkung = t.Bemerkung,
                    // Aptyp = t.Aptyp,
                    OeId = t.OeId,
                    VerantwOeId = t.VerantwOeId,
                    ApTypId = t.ApTypId,
                    ApTypBezeichnung = t.ApTypBezeichnung,
                    ApTypFlag = t.ApTypFlag,
                    ApKatId = t.ApKatId,
                    ApKatBezeichnung = t.ApKatBezeichnung,
                    ApKatFlag = t.ApKatFlag,
                    Tags = t.Tags ?? new List<Tag>()
                };
                // foreach (var h in t.Hw) {
                //     var hw = new Hardware {
                //         Id = h.Id,
                //         Hersteller = h.Hersteller,
                //         Bezeichnung = h.Bezeichnung,
                //         Sernr = h.Sernr,
                //         Pri = h.Pri,
                //         Hwtyp = h.Hwtyp,
                //         HwtypFlag = h.HwtypFlag,
                //         Vlan = h.Vlan ?? new List<Netzwerk>()
                //     };
                //     ap.Hw.Add(hw);
                // }
                aps.Add(ap);
            }
            return aps;
        }

        private class TmpHw {
            public long Id { get; set; }
            public string Hersteller { get; set; }
            public string Bezeichnung { get; set; }
            public string Sernr { get; set; }
            public bool Pri { get; set; }
            public string Hwtyp { get; set; }
            public long HwtypFlag { get; set; }
            public List<Netzwerk> Vlan { get; set; }
        }

        private class TmpAp {
            public long ApId { get; set; }
            public string Apname { get; set; }
            public string Bezeichnung { get; set; }
            public string Aptyp { get; set; }
            public long OeId { get; set; }
            public long VerantwOeId { get; set; }
            public string Bemerkung { get; set; }
            public long ApTypId { get; set; }
            public string ApTypBezeichnung { get; set; }
            public long ApTypFlag { get; set; }
            public long ApKatId { get; set; }
            public string ApKatBezeichnung { get; set; }
            public long ApKatFlag { get; set; }
            public List<Tag> Tags { get; set; }
            public List<TmpHw> Hw { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class HwRepository: IHwRepository {
        private readonly SbsdbContext _dbContext;
        private readonly ILogger<HwRepository> _log;

        public HwRepository(SbsdbContext context, ILogger<HwRepository> log) {
            _dbContext = context;
            _log = log;
        }
        
        public List<Hardware> GetAll() {
            return QueryHw(_dbContext.Hw).ToList();
        }

        public List<Hardware> GetHardware(long id) {
            return QueryHw(_dbContext.Hw.Where(hw => hw.Id == id)).ToList();
        }
        
        public List<Hardware> GetPage(int page, int pageSize) {  
            int skipRows = page * pageSize;  // page is zero based!
            return QueryHw(_dbContext.Hw).Skip(skipRows).Take(pageSize).ToList();
        }

        public List<Hardware> GetHwForAp(long apid) {
            return QueryHw(_dbContext.Hw.Where((hw) => hw.ApId == apid)).ToList();
        }
        
        public int GetCount() {
            return _dbContext.Hw.Count();
        }

        public HwTransport ChangeHw(EditHwTransport hwt) {
            if (hwt == null) {
                return null;
            }
            Hw hw;
            if (hwt.DelHw) {
                // DEL hw
                // include() => auch records aus HwHistory loeschen
                hw = _dbContext.Hw.Include(hdw => hdw.Hwhistory).First(hdw => hdw.Id == hwt.Id);
                if (hw.ApId is > 0) {
                    ChangeAp(hw, null, false);
                }
                var macs = "";
                foreach (var mac in hw.Mac) {
                    macs += (macs.Length > 0 ? " " : "") + mac.Adresse;
                    ChangeVlan(mac.Id, "", 0, 0, 0);
                }
                var auss = new Aussond {
                    Id = hw.Id,
                    SerNr = hw.SerNr,
                    AnschDat = hw.AnschDat,
                    InvNr = hw.InvNr,
                    AnschWert = hw.AnschWert,
                    HwkonfigId = hw.HwkonfigId,
                    Mac = macs,
                    Smbiosguid = hw.Smbiosguid,
                    WartungFa = hw.WartungFa,
                    Bemerkung = hw.Bemerkung,
                    AussDat = DateTime.Today,
                    AussGrund = hwt.Aussonderung,
                    Rewe = null,
                };
                _dbContext.Aussond.Add(auss);
                _dbContext.Hw.Remove(hw);
                _dbContext.SaveChanges();
                return new HwTransport {
                    Hw = null,
                    DelHwId = hwt.Id,
                };
            }
            
            if (hwt.Id == 0) {
                // new hw
                hw = new Hw {
                    Bemerkung = hwt.Bemerkung,
                    Smbiosguid = hwt.Smbiosgiud,
                    AnschDat = hwt.AnschDat,
                    AnschWert = hwt.AnschWert,
                    InvNr = hwt.InvNr,
                    SerNr = hwt.Sernr ?? "--",
                    WartungFa = hwt.WartungFa,
                };
                if (hwt.HwKonfigId.HasValue) {
                    hw.HwkonfigId = hwt.HwKonfigId.Value;
                }
                else {
                    return null;
                }
                _dbContext.Hw.Add(hw);
                _dbContext.SaveChanges();

            } else {
                // change hw
                hw = _dbContext.Hw.First(hdw => hdw.Id == hwt.Id);
                if (hwt.Sernr != null) {
                    hw.Bemerkung = hwt.Bemerkung;
                    hw.SerNr = hwt.Sernr ?? "--";
                    hw.Smbiosguid = hwt.Smbiosgiud;
                    hw.AnschDat = hwt.AnschDat;
                    hw.AnschWert = hwt.AnschWert;
                    hw.InvNr = hwt.InvNr;
                    hw.WartungFa = hwt.WartungFa;
                }
                if (hwt.RemoveAp) {
                    ChangeAp(hw, null, false);
                }
            }
            foreach (var vlan in hwt.Vlans) {
                _log.LogDebug("HW Change: change peri vlan");
                ChangeVlan(vlan.HwMacId, vlan.Mac, vlan.VlanId, vlan.Ip, hw.Id);
            }
        
            _dbContext.SaveChanges();
            
            // return changes
            return new HwTransport {
                Hw = GetHardware(hw.Id).First(),
                DelHwId = 0,
            };
        }

        public HwTransport[] ChangeHwMulti(EditHwTransport[] hws) {
            var result = new List<HwTransport>();
            foreach (var hw in hws) {
                var chg = ChangeHw(hw);
                if (chg != null) {
                    result.Add(chg);
                }
            }
            return result.Count > 0 ? result.ToArray() : null;
        }

        public AddHwTransport AddHw(NewHwTransport nhw) {
            var ids = new Stack<long>();
            foreach (var device in nhw.Devices) {
                var hw = new Hw {
                    HwkonfigId = nhw.KonfigId,
                    SerNr = device.Sernr,
                    InvNr = nhw.InvNr,
                    AnschDat = nhw.AnschDat,
                    AnschWert = nhw.AnschWert,
                    WartungFa = nhw.WartungFa,
                    Bemerkung = nhw.Bemerkung
                };
                hw.ChangeAp(null, false);
                _dbContext.Hw.Add(hw);
                _dbContext.SaveChanges();
                if (!string.IsNullOrEmpty(device.Mac)) {
                    var mac = new Mac {
                        Adresse = device.Mac,
                        HwId = hw.Id
                    };
                    _dbContext.Mac.Add(mac);
                    _dbContext.SaveChanges();
                }
                ids.Push(hw.Id);
            }
            return new AddHwTransport {
                NewHw = QueryHw(_dbContext.Hw.Where((hw) => ids.ToArray().Contains(hw.Id))).ToArray()
            };
        }
        
        public List<HwHistory> GetHwHistoryFor(long hwid) {
            return _dbContext.Hwhistory.Where(hwh => hwh.HwId == hwid).AsNoTracking().OrderByDescending(hwh => hwh.Shiftdate)
                .Select(hwh => new HwHistory {
                    Id = hwh.Id,
                    Apbezeichnung = hwh.ApBezeichnung,
                    Apname = hwh.Apname,
                    Betriebsstelle = hwh.Betriebsstelle,
                    Direction = hwh.Direction,
                    Shiftdate = hwh.Shiftdate,
                    Apid = hwh.ApId,
                    Hwid = hwh.HwId
                }).ToList();
        }
        
        /**
         * Aenderung des AP in Hwhistory vermerken
         *
         * Die fn macht keinen Commit, das ist Aufgabe des Callers
         */
        public void ChangeAp(Hw hw, long? newapid, bool pri) {
            if (hw.ApId != null) {
                // alter Eintrag vorhanden, in der History vermerken
                var oldap = _dbContext.Ap.Include(ap => ap.Oe).First(ap => ap.Id == hw.ApId);
                var shift = new Hwhistory {
                    HwId = hw.Id,
                    Direction = "-",
                    Apname = oldap.Apname,
                    ApBezeichnung = oldap.Bezeichnung,
                    ApId = oldap.Id,
                    Betriebsstelle = oldap.Oe.Betriebsstelle,
                    Shiftdate = DateTime.Now
                };
                _dbContext.Hwhistory.Add(shift);
            }
            if (newapid != null) {
                // neuen AP eintragen
               var newap = _dbContext.Ap.Include(ap => ap.Oe).First(ap => ap.Id == newapid.Value);
               var shift = new Hwhistory {
                    HwId = hw.Id,
                    Direction = "+",
                    Apname = newap.Apname,
                    ApBezeichnung = newap.Bezeichnung,
                    ApId = newap.Id,
                    Betriebsstelle = newap.Oe.Betriebsstelle,
                    Shiftdate = DateTime.Now
                };
                _dbContext.Hwhistory.Add(shift);
            }
            // im Datensatz eintragen
            hw.ChangeAp(newapid, pri);
        }
        
        public void ChangeVlan(long id, string mac, long vlanid, long ip, long hwid) {
            if (id == 0) {
                var nVlan = new Mac {
                    Adresse = mac,
                    Ip = ip,
                    VlanId = vlanid == 0 ? null : vlanid,
                    HwId = hwid
                };
                _dbContext.Mac.Add(nVlan);
            } else if (mac == "") {
                var vlan = _dbContext.Mac.Find(id);
                if (vlan != null) {
                    _dbContext.Mac.Remove(vlan);
                } else {
                    _log.LogError("Error in ChangeVlan() Mac to remove {Id} ist nicht vorhanden!", id);
                }
            } else {
                var vlan = _dbContext.Mac.Find(id);
                if (vlan != null) {
                    vlan.Adresse = mac;
                    vlan.Ip = ip;
                    vlan.VlanId = vlanid == 0 ? null : vlanid;
                    _dbContext.Mac.Update(vlan);
                } else {
                    _log.LogError("Error in ChangeVlan() Mac to change {Id} ist nicht vorhanden!", id);
                }
            }
        }

        private IQueryable<Hardware> QueryHw(IQueryable<Hw> ctx) {
            return ctx
                .AsNoTracking()
                .OrderBy(hw => hw.Id)
                .Select(hw => new Hardware {
                    Id = hw.Id,
                    Sernr = hw.SerNr,
                    // leeres Datum als UNIX-Epoch => in JS: aschDat.valueOf() === 0 
                    AnschDat = hw.AnschDat ?? new DateTime(1970, 1, 1,1,0,0,0),
                    AnschWert = hw.AnschWert ?? 0,
                    InvNr = hw.InvNr,
                    Smbiosgiud = hw.Smbiosguid,
                    WartungFa = hw.WartungFa,
                    Bemerkung = hw.Bemerkung,
                    Pri = hw.Pri,
                    HwKonfigId = hw.HwkonfigId,
                    ApId = hw.ApId ?? 0,
                    Vlans = hw.Mac.Select(m => new Netzwerk {
                        HwMacId = m.Id,
                        VlanId = m.VlanId,
                        Bezeichnung = m.Vlan != null ? m.Vlan.Bezeichnung : "",
                        Vlan = m.Vlan.Ip, // != null ? m.Vlan.Ip : null,
                        Netmask = m.Vlan.Netmask, // != null ? m.Vlan.Netmask : null,
                        Ip = m.Ip,
                        Mac = m.Adresse
                    }).ToList()
                });
        }
    }
}

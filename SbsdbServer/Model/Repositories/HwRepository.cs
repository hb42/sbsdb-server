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
            Hw hw = null;
            if (hwt.Id == 0) {
                // TODO zuerst handling der HW-Konfig auf Clientseite klaeren
                // new hw
                
                hw = new Hw {
                    Bemerkung = hwt.Bemerkung,
                    Smbiosguid = hwt.Smbiosgiud,
                    AnschDat = hwt.AnschDat,
                    AnschWert = hwt.AnschWert,
                    InvNr = hwt.InvNr,
                    SerNr = hwt.Sernr,
                    WartungFa = hwt.WartungFa,
                };
                
                // _dbContext.Hw.Add(hw);
                // _dbContext.SaveChanges();
                
            } else {
                // change hw
                hw = _dbContext.Hw.First(hdw => hdw.Id == hwt.Id);
                if (hwt.Bemerkung != null) {
                    hw.Bemerkung = hwt.Bemerkung;
                }
                if (hwt.Sernr != null) {
                    hw.SerNr = hwt.Sernr;
                }
                if (hwt.Smbiosgiud != null) {
                    hw.Smbiosguid = hwt.Smbiosgiud;
                }
                if (hwt.AnschDat != null) {
                    hw.AnschDat = hwt.AnschDat;
                }
                if (hwt.AnschWert != null) {
                    hw.AnschWert = hwt.AnschWert;
                }
                if (hwt.InvNr != null) {
                    hw.InvNr = hwt.InvNr;
                }
                if (hwt.WartungFa != null) {
                    hw.WartungFa = hwt.WartungFa;
                }
                // TODO Ap-change-handling??
                // if (hwt.ApId.HasValue && hwt.ApId != hw.ApId) {
                //     ChangeAp(hw, hwt.ApId, hw.Pri);
                // }
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
            Hardware h = null;
            if (hw != null) {
                h = GetHardware(hw.Id).First();
            }

            return new HwTransport {
                Hw = h,
                DelHwId = 0, // TODO set >0 if del HW
                    
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
            Ap oldap = null;
            Ap newap = null;
            if (hw.ApId != null) {
                // alter Eintrag vorhanden, in der History vermerken
                oldap = _dbContext.Ap.Include(ap => ap.Oe).First(ap => ap.Id == hw.ApId);
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
               newap = _dbContext.Ap.Include(ap => ap.Oe).First(ap => ap.Id == newapid.Value);
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
                _dbContext.Mac.Remove(vlan);
            } else {
                var vlan = _dbContext.Mac.Find(id);
                vlan.Adresse = mac;
                vlan.Ip = ip;
                vlan.VlanId = vlanid == 0 ? null : vlanid;
                _dbContext.Mac.Update(vlan);
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

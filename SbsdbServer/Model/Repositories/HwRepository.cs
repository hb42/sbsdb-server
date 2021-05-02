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

using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class HwKonfigRepository: IHwKonfigRepository {
        private readonly SbsdbContext _dbContext;
        private readonly ILogger<HwKonfigRepository> _log;

        public HwKonfigRepository(SbsdbContext context, ILogger<HwKonfigRepository> log) {
            _dbContext = context;
            _log = log;
        }
        
        public List<HwKonfig> GetAll() {
            return QueryHwKonfig(_dbContext.Hwkonfig).ToList();
        }

        public List<HwKonfig> GetHwKonfig(long id) {
            return QueryHwKonfig(_dbContext.Hwkonfig.Where(hwkonfig => hwkonfig.Id == id)).ToList();
        }
        
        public HwKonfig ChangeKonfig(KonfigChange kc) {
            if (kc == null) {
                return null;
            }
            Hwkonfig konfig;
            if (kc.Id > 0) {
                //change   
                konfig = _dbContext.Hwkonfig.Find(kc.Id);
                if (konfig != null) {
                    konfig.Hersteller = kc.Hersteller;
                    konfig.Bezeichnung = kc.Bezeichnung;
                    konfig.Hd = kc.Hd;
                    konfig.Ram = kc.Ram;
                    konfig.Prozessor = kc.Prozessor;
                    konfig.Video = kc.Video;
                    konfig.Sonst = kc.Sonst;
                } else {
                    _log.LogError("Error in ChangeKonfig() Konfig {Id} ist nicht vorhanden!", kc.Id);
                    return null;
                }
            }
            else {
                // new
                konfig = new Hwkonfig {
                    Hersteller = kc.Hersteller,
                    Bezeichnung = kc.Bezeichnung,
                    Hd = kc.Hd,
                    Ram = kc.Ram,
                    Prozessor = kc.Prozessor,
                    Video = kc.Video,
                    Sonst = kc.Sonst,
                    HwtypId = kc.HwTypId
                };
                _dbContext.Hwkonfig.Add(konfig);
            }
            _dbContext.SaveChanges();
            return GetHwKonfig(konfig.Id).First();
        }

        public long? DelKonfig(long id) {
            var konfig = _dbContext.Hwkonfig.Find(id);
            if (konfig != null) {
                _dbContext.Hwkonfig.Remove(konfig);
                _dbContext.SaveChanges();
                return id;
            }
            return null;
        }
        
        /**
         * Liefert IDs aller HwKonfig, fuer die eine Aussonderung gespeichert ist.
         * Loeschen der HwKonfig ist erst moeglich, wenn die Aussonderungen
         * geloescht wurden.
         */
        public long[] HwKonfigInAussond() {
            return _dbContext.Aussond
                .GroupBy(au => au.HwkonfigId)
                .OrderBy(au => au.Key)
                .Select(au => au.Key)
                .ToArray();
        }
        
        private IQueryable<HwKonfig> QueryHwKonfig(IQueryable<Hwkonfig> ctx) {
            return ctx
                .AsNoTracking()
                .Select(hwkonfig => new HwKonfig {
                    Id = hwkonfig.Id,
                    Bezeichnung = hwkonfig.Bezeichnung,
                    Hersteller = hwkonfig.Hersteller,
                    Hd = hwkonfig.Hd,
                    Prozessor = hwkonfig.Prozessor,
                    Ram = hwkonfig.Ram,
                    Sonst = hwkonfig.Sonst,
                    Video = hwkonfig.Video,

                    HwTypId = hwkonfig.HwtypId,
                    HwTypBezeichnung = hwkonfig.Hwtyp.Bezeichnung,
                    HwTypFlag = hwkonfig.Hwtyp.Flag,

                    ApKatId = hwkonfig.Hwtyp.ApkategorieId,
                    ApKatBezeichnung = hwkonfig.Hwtyp.Apkategorie.Bezeichnung,
                    ApKatFlag = hwkonfig.Hwtyp.Apkategorie.Flag ?? 0,
                });
        }
    }
}

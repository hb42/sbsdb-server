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

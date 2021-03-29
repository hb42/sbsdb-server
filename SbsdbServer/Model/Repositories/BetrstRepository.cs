using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class BetrstRepository: IBetrstRepository {
        private readonly SbsdbContext _dbContext;
        private readonly ILogger<BetrstRepository> _log;

        public BetrstRepository(SbsdbContext context, ILogger<BetrstRepository> log) {
            _dbContext = context;
            _log = log;
        }
        
        public List<Betrst> GetAll() {
            return QueryBst(_dbContext.Oe).ToList();
        }

        public List<Betrst> GetBetrst(long id) {
            return QueryBst(_dbContext.Oe.Where(oe => oe.Id == id)).ToList();
        }

        private IQueryable<Betrst> QueryBst(IQueryable<Oe> ctx) {
            return ctx
                .AsNoTracking()
                .Select(bst => new Betrst {
                    BstId = bst.Id,
                    Betriebsstelle = bst.Betriebsstelle,
                    BstNr = bst.Bst,
                    Fax = bst.Fax,
                    Tel = bst.Tel,
                    Oeff = bst.Oeff,
                    Ap = (bool) bst.Ap,
                    ParentId = bst.OeId,
                    Plz = bst.Adresse.Plz,
                    Ort = bst.Adresse.Ort,
                    Strasse = bst.Adresse.Strasse,
                    Hausnr = bst.Adresse.Hausnr
                });
        }
    }
}
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

        public EditOeTransport ChangeBetrst(EditOeTransport chg) {
            // TODO
            return null;
        }

        public List<Adresse> GetAdressen() {
            return _dbContext.Adresse.ToList();
        }

        public EditAdresseTransport ChangeAdresse(EditAdresseTransport chg) {
            Adresse adr;
            if (chg.Del) {
                // del
                adr = _dbContext.Adresse.Find(chg.Adresse.Id);
                if (adr != null) {
                    _dbContext.Adresse.Remove(adr);
                }
            } else if (chg.Adresse.Id == 0) {
                // new
                adr = new Adresse { 
                    Plz= chg.Adresse.Plz,
                    Ort = chg.Adresse.Ort,
                    Strasse = chg.Adresse.Strasse,
                    Hausnr = chg.Adresse.Hausnr
                };
                _dbContext.Adresse.Add(adr);
            } else {
                // chg  
                adr = _dbContext.Adresse.Find(chg.Adresse.Id);
                if (adr != null) {
                    adr.Plz = chg.Adresse.Plz;
                    adr.Ort = chg.Adresse.Ort;
                    adr.Strasse = chg.Adresse.Strasse;
                    adr.Hausnr = chg.Adresse.Hausnr;
                    _dbContext.Adresse.Update(adr);
                }
            }
            var rc = _dbContext.SaveChanges();
            if (rc == 1) {
                if (!chg.Del) {
                    var ret = _dbContext.Adresse.Find(adr.Id);
                    chg.Adresse = ret;
                }
                return chg;
            }
            else {
                return null;
            }
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
                    AdresseId = bst.AdresseId
                });
        }
    }
}

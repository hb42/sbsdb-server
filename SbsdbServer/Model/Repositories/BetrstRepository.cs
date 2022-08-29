using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class BetrstRepository : IBetrstRepository {
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
            Oe oe;
            if (chg.Del) {
                // del
                oe = _dbContext.Oe.Find(chg.Oe.BstId);
                if (oe != null) {
                    _dbContext.Oe.Remove(oe);
                }
            }
            else if (chg.Oe.BstId == 0) {
                // new
                oe = new Oe {
                    Betriebsstelle = chg.Oe.Betriebsstelle,
                    Bst = chg.Oe.BstNr,
                    Tel = chg.Oe.Tel,
                    Oeff = chg.Oe.Oeff,
                    Ap = chg.Oe.Ap,
                    AdresseId = chg.Oe.AdresseId,
                    OeId = chg.Oe.ParentId
                };
                _dbContext.Oe.Add(oe);
            }  else {
                // chg  
                oe = _dbContext.Oe.Find(chg.Oe.BstId);
                if (oe != null) {
                    oe.Betriebsstelle = chg.Oe.Betriebsstelle;
                    oe.Bst = chg.Oe.BstNr;
                    oe.Tel = chg.Oe.Tel;
                    oe.Oeff = chg.Oe.Oeff;
                    oe.Ap = chg.Oe.Ap;
                    oe.AdresseId = chg.Oe.AdresseId;
                    oe.OeId = chg.Oe.ParentId;
                    _dbContext.Oe.Update(oe);
                }
            }

            var rc = _dbContext.SaveChanges();
            if (rc == 1) {
                if (!chg.Del) {
                    var ret = _dbContext.Oe.Where(o => o.Id == oe.Id)
                        .Select(bst => new Betrst {
                            BstId = bst.Id,
                            Betriebsstelle = bst.Betriebsstelle,
                            BstNr = bst.Bst,
                            Tel = bst.Tel,
                            Oeff = bst.Oeff,
                            Ap = (bool)bst.Ap,
                            ParentId = bst.OeId,
                            AdresseId = bst.AdresseId
                        }).First();
                    chg.Oe = ret;
                }
                return chg;
            } else {
                return null;
            }
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
                    Plz = chg.Adresse.Plz,
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
                if (!chg.Del && adr != null) {
                    var ret = _dbContext.Adresse.Find(adr.Id);
                    chg.Adresse = ret;
                }

                return chg;
            } else {
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
                    Tel = bst.Tel,
                    Oeff = bst.Oeff,
                    Ap = (bool)bst.Ap,
                    ParentId = bst.OeId,
                    AdresseId = bst.AdresseId
                });
        }
    }
}

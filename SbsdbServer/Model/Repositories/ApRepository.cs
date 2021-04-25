using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class ApRepository : IApRepository {
        private readonly SbsdbContext _dbContext;
        private readonly ILogger<ApRepository> _log;

        public ApRepository(SbsdbContext context, ILogger<ApRepository> log) {
            _dbContext = context;
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

        public int GetCount() {
            return _dbContext.Ap.Count();
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
                    ApTypFlag = ap.Aptyp.Flag ?? 0,
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
                    ApTypFlag = ap.Aptyp.Flag ?? 0,
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

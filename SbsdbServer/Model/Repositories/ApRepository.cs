using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
  public class ApRepository : IApRepository {

    private readonly SbsdbContext dbContext;
    private readonly ILogger<ApRepository> LOG;

    public ApRepository(SbsdbContext context, ILogger<ApRepository> log) {
      dbContext = context;
      LOG = log;
    }

    public Arbeitsplatz GetAp(long id) {
      return GetArbeitsplatz(dbContext.Ap.Find(id));
    }

    public List<Arbeitsplatz> GetAps2(string search) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      //string like = "%" + search.ToUpper() + "%";
      string like = search.ToUpper();
      var aps = dbContext.Ap
        //.Where(a => EF.Functions.Like(a.Apname.ToUpper(), like) || EF.Functions.Like(a.Bezeichnung.ToUpper(), like))
        .Where(a => a.Apname.ToUpper().Contains(like) || a.Bezeichnung.ToUpper().Contains(like))
        .Include(a => a.Aptyp)
        .Include(a => a.Hw)
        .ThenInclude(h => h.Hwkonfig)
        .Include(a => a.Oe)
        .ThenInclude(o => o.Adresse)
        .Include(a => a.OeIdVerOeNavigation)
        .ThenInclude(o => o.Adresse)
        .Include(a => a.ApTag)
        .ThenInclude(t => t.Tagtyp)
        .ToList();
      List<Arbeitsplatz> rc = new List<Arbeitsplatz>();
      foreach (Ap ap in aps) {
        rc.Add(GetArbeitsplatz(ap));
      }
      watch.Stop();
      long delta = watch.ElapsedMilliseconds;
      LOG.LogDebug("--- include-query for search '" + like + "' took " + delta + "ms");
      return rc;
    }

    public List<Arbeitsplatz> GetAps(string search) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      //string like = "%" + search.ToUpper() + "%";
      string like = search.ToUpper();
      var aps = dbContext.Ap
        //.Where(a => EF.Functions.Like(a.Apname.ToUpper(), like) || EF.Functions.Like(a.Bezeichnung.ToUpper(), like))
        .Where(a => a.Apname.ToUpper().Contains(like) || a.Bezeichnung.ToUpper().Contains(like))
        .Select(ap => new Arbeitsplatz {
          ApId = ap.Id,
          Apname = ap.Apname,
          Bezeichnung = ap.Bezeichnung,
          Aptyp = ap.Aptyp.Bezeichnung,
          Oe = new Betrst {
            BstId = ap.Oe.Id,
            Betriebsstelle = ap.Oe.Betriebsstelle,
            BstNr = ap.Oe.Bst,
            Fax = ap.Oe.Fax,
            Tel = ap.Oe.Tel,
            Oeff = ap.Oe.Oeff,
            Ap = (bool)ap.Oe.Ap,
            ParentId = ap.Oe.OeId,
            Plz = ap.Oe.Adresse.Plz,
            Ort = ap.Oe.Adresse.Ort,
            Strasse = ap.Oe.Adresse.Strasse,
            Hausnr = ap.Oe.Adresse.Hausnr
          },
          VerantwOe = ap.OeIdVerOe == null || ap.OeIdVerOe == ap.OeId ? null
            : new Betrst {
              BstId = ap.OeIdVerOeNavigation.Id,
              Betriebsstelle = ap.OeIdVerOeNavigation.Betriebsstelle,
              BstNr = ap.OeIdVerOeNavigation.Bst,
              Fax = ap.OeIdVerOeNavigation.Fax,
              Tel = ap.OeIdVerOeNavigation.Tel,
              Oeff = ap.OeIdVerOeNavigation.Oeff,
              Ap = (bool)ap.OeIdVerOeNavigation.Ap,
              ParentId = ap.OeIdVerOeNavigation.OeId,
              Plz = ap.OeIdVerOeNavigation.Adresse.Plz,
              Ort = ap.OeIdVerOeNavigation.Adresse.Ort,
              Strasse = ap.OeIdVerOeNavigation.Adresse.Strasse,
              Hausnr = ap.OeIdVerOeNavigation.Adresse.Hausnr
            },
          Hw = ap.Hw.Select(hw => new Hardware {
            Hersteller = hw.Hwkonfig.Hersteller,
            Bezeichnung = hw.Hwkonfig.Bezeichnung,
            Sernr = hw.SerNr
          }).ToList(),
          Tags = ap.ApTag.Select(t => new Tag {
            ApTagId = t.Id,
            TagId = t.TagtypId,
            Bezeichnung = t.Tagtyp.Bezeichnung,
            Text = t.Text,
            Param = t.Tagtyp.Param,
            Flag = t.Tagtyp.Flag,
            AptypId = t.Tagtyp.AptypId
          }).ToList()
        })
        .ToList();
      long delta = watch.ElapsedMilliseconds;
      LOG.LogDebug("--- select-query for search '" + like + "' took " + delta + "ms");
      return aps;
    }

    public List<Arbeitsplatz> QueryAps(ApQuery query) {
      throw new NotImplementedException();
    }

    private Arbeitsplatz GetArbeitsplatz(Ap ap) {
      if (ap != null) {
        Arbeitsplatz rc = new Arbeitsplatz {
          ApId = ap.Id,
          Apname = ap.Apname,
          Bezeichnung = ap.Bezeichnung,
          Aptyp = ap.Aptyp.Bezeichnung,
          Oe = new Betrst {
            BstId = ap.Oe.Id,
            Betriebsstelle = ap.Oe.Betriebsstelle,
            BstNr = ap.Oe.Bst,
            Fax = ap.Oe.Fax,
            Tel = ap.Oe.Tel,
            Oeff = ap.Oe.Oeff,
            Ap = (bool)ap.Oe.Ap,
            ParentId = ap.Oe.OeId,
            Plz = ap.Oe.Adresse.Plz,
            Ort = ap.Oe.Adresse.Ort,
            Strasse = ap.Oe.Adresse.Strasse,
            Hausnr = ap.Oe.Adresse.Hausnr
          },
          VerantwOe = ap.OeIdVerOe == null || ap.OeIdVerOe == ap.OeId ? null 
            : new Betrst {
              BstId = ap.OeIdVerOeNavigation.Id,
              Betriebsstelle = ap.OeIdVerOeNavigation.Betriebsstelle,
              BstNr = ap.OeIdVerOeNavigation.Bst,
              Fax = ap.OeIdVerOeNavigation.Fax,
              Tel = ap.OeIdVerOeNavigation.Tel,
              Oeff = ap.OeIdVerOeNavigation.Oeff,
              Ap = (bool)ap.OeIdVerOeNavigation.Ap,
              ParentId = ap.OeIdVerOeNavigation.OeId,
              Plz = ap.OeIdVerOeNavigation.Adresse.Plz,
              Ort = ap.OeIdVerOeNavigation.Adresse.Ort,
              Strasse = ap.OeIdVerOeNavigation.Adresse.Strasse,
              Hausnr = ap.OeIdVerOeNavigation.Adresse.Hausnr
            }
        };
        rc.Hw = new List<Hardware>();
        foreach (Hw hw in ap.Hw) {
          rc.Hw.Add(new Hardware {
            Hersteller = hw.Hwkonfig.Hersteller,
            Bezeichnung = hw.Hwkonfig.Bezeichnung,
            Sernr = hw.SerNr
          });
        }
        rc.Tags = new List<Tag>();
        foreach (ApTag t in ap.ApTag) {
          rc.Tags.Add(new Tag {
            ApTagId = t.Id,
            TagId = t.TagtypId,
            Bezeichnung = t.Tagtyp.Bezeichnung,
            Text = t.Text,
            Param = t.Tagtyp.Param ?? "",
            Flag = t.Tagtyp.Flag,
            AptypId = t.Tagtyp.AptypId
          });
        }
        return rc;
      } else {
        return null;
      }

    }
  }
}

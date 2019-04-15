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

    public List<Arbeitsplatz> GetAps(string search) {
      //string like = "%" + search + "%";
      string like = search.ToUpper();
      var aps = dbContext.Ap
        //.Where(a => EF.Functions.Like(a.Apname, like) || EF.Functions.Like(a.Bezeichnung, like))
        .Where(a => a.Apname.ToUpper().Contains(like) || a.Bezeichnung.ToUpper().Contains(like))
        .ToList();
      List<Arbeitsplatz> rc = new List<Arbeitsplatz>();
      foreach (Ap ap in aps) {
        rc.Add(GetArbeitsplatz(ap));
      }
      return rc;
    }

    public List<Arbeitsplatz> QueryAps(ApQuery query) {
      //dbContext.Ap.Where(a => EF.Functions.Like(a.Apname, "%blah%"));
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
          long tmp = t.Tagtyp.Flag;
          LOG.LogDebug("tag " + t.Tagtyp.Id + " " + t.Id + " flag=" + tmp);
          Tag tag = new Tag {
            ApTagId = t.Id,
            TagId = t.TagtypId,
            Bezeichnung = t.Tagtyp.Bezeichnung,
            Text = t.Text,
            Flag = 2, //t.Tagtyp.Flag,
            Param = t.Tagtyp.Param ?? "",
            AptypId = t.Tagtyp.AptypId
          };
          //LOG.LogDebug("tag.Flag=" + tag.Flag + " tmp=" + tmp);
          //tag.Flag = tmp * 1;
          if (tag.Flag == 1) {
            rc.TypTags.Add(tag);
          } else {
            rc.Tags.Add(tag);
          }
        }
        return rc;
      } else {
        return null;
      }

    }
  }
}

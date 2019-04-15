using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace hb.SbsdbServer.Model.Repositories {
  public class ApRepository : IApRepository {

    private readonly SbsdbContext dbContext;

    public ApRepository(SbsdbContext context) {
      dbContext = context;
    }

    public Arbeitsplatz GetAp(long id) {
      return GetArbeitsplatz(dbContext.Ap.Find(id));
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
          Tag tag = new Tag {
            ApTagId = t.Id,
            TagId = t.TagtypId,
            Bezeichnung = t.Tagtyp.Bezeichnung,
            Text = t.Text,
            Flag = t.Tagtyp.Flag,
            Param = t.Tagtyp.Param,
            AptypId = t.Tagtyp.AptypId
          });
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

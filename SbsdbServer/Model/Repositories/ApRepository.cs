using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
  public class ApRepository : IApRepository {
    private class tmpHw {
      public long Id { get; set; }
      public string Hersteller { get; set; }
      public string Bezeichnung { get; set; }
      public string Sernr { get; set; }
      public bool Pri { get; set; }
      public string Hwtyp { get; set; }
      public long HwtypFlag { get; set; }
      public List<Netzwerk> Vlan { get; set; }
    }
    private class tmpAp {
      public long ApId { get; set; }
      public string Apname { get; set; }
      public string Bezeichnung { get; set; }
      public string Aptyp { get; set; }
      public Betrst Oe { get; set; }
      public Betrst VerantwOe { get; set; }
      public List<Tag> Tags { get; set; }
      public List<tmpHw> Hw { get; set; }
    }


    private readonly SbsdbContext dbContext;
    private readonly ILogger<ApRepository> LOG;

    public ApRepository(SbsdbContext context, ILogger<ApRepository> log) {
      dbContext = context;
      LOG = log;
    }

    /*
     * Einzelnen AP anhand der ID holen
     * 
     * Liefert Liste: check auf leere Liste muss aufrufende Routine erledigen.
     */
    public List<Arbeitsplatz> GetAp(long id) {
      return Convert(
        GetArbeitsplatzQuery(
          dbContext.Ap.Where(ap => ap.Id == id)
        ).ToList()
      );
        
    }

    /*
     * Suchstring in AP-Name und AP-Bezeichnung suchen
     */
    public List<Arbeitsplatz> GetAps(string search) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      string like = search.ToUpper();
      List<Arbeitsplatz> aps = Convert(
        GetArbeitsplatzQuery(
          dbContext.Ap.Where(a => a.Apname.ToUpper().Contains(like) || a.Bezeichnung.ToUpper().Contains(like))
        ).ToList()
      );
      watch.Stop();
      long delta = watch.ElapsedMilliseconds;
      LOG.LogDebug("--- select-query for search '" + like + "' took " + delta + "ms");
      return aps;
    }

    /*
     * Alle APs einer OE, sowie der untergeordneten OEs
     */
    public List<Arbeitsplatz> ApsForOe(long oeid) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      List<OeTreeItem> lookup = new List<OeTreeItem>();
      var oes = dbContext.Oe.Select(o => new OeTreeItem {
        Id = o.Id,
        ParentId = o.OeId,
      }).ToList();
      var oe = oes.Where(o => o.Id == oeid).ToList();
      lookup.AddRange(oe);
      lookup.AddRange(FindChildren(oe, oes));
      long[] oeids = lookup.Select(o => o.Id).ToArray();
      long delta1 = watch.ElapsedMilliseconds;
      List<tmpAp> tmp = GetArbeitsplatzQuery(
          dbContext.Ap.Where(ap => oeids.Contains(ap.OeId))
        ).ToList();
      long delta2 = watch.ElapsedMilliseconds;
      List<Arbeitsplatz> aps = Convert(tmp);
      watch.Stop();
      long delta3 = watch.ElapsedMilliseconds;
      LOG.LogDebug("--- select-query for OE pre=" + delta1 + "ms/ query=" + (delta2 - delta1) + "ms/ post=" + (delta3 - delta2) + "ms/ sum=" + delta3 + "ms" );
      return aps;
    }

    // rekursiv alle untergeordneten OEs holen
    private List<OeTreeItem> FindChildren(List<OeTreeItem> parents, List<OeTreeItem> all) {
      List<OeTreeItem> found = new List<OeTreeItem>();
      foreach(OeTreeItem oe in parents) {
        //LOG.LogDebug("recurse OE id=" + oe.Id);
        // Sonderfall root beruecksichtigen: id == 0 && parentId == 0
        var children = all.Where(o => o.ParentId == oe.Id && o.Id != 0).ToList();
        if (children.Count > 0) {
          found.AddRange(children);
          found.AddRange(FindChildren(children, all));
        }
      }
      return found;
    }

    /*
     * AP anhand der Kriterien in query holen
     */
    public List<Arbeitsplatz> QueryAps(ApQuery query) {
      throw new NotImplementedException();
    }

    /*
     * Alle benoetigten Daten fuer einen Arbeitsplatz aus der DB holen
     * 
     * IN: Query auf Ap mit den notwendigen Bedingungen
     * OUT: Query ergaenzt um Select fuer die AP-Daten
     * 
     * Die Abfrage liefert eine tmpAP-Objekte, da das die effizienteste DB-Abfrage
     * ist. Anschließend muss das Objekt noch in ein Arbeitsplatz-Objekt 
     * umgewandelt werden.
     */
    private IQueryable<tmpAp> GetArbeitsplatzQuery(IQueryable<Ap> apq) {
      return apq
        .AsNoTracking()
        .Select(ap => new tmpAp {
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
          Hw = ap.Hw.Select(hw => new tmpHw {
            Id = hw.Id,
            Hersteller = hw.Hwkonfig.Hersteller,
            Bezeichnung = hw.Hwkonfig.Bezeichnung,
            Sernr = hw.SerNr,
            Pri = hw.Pri,
            Hwtyp = hw.Hwkonfig.Hwtyp.Bezeichnung,
            HwtypFlag = (long)hw.Hwkonfig.Hwtyp.Flag,
            Vlan = hw.Mac.Select(m => new Netzwerk {
              VlanId = m.VlanId,
              Bezeichnung = m.Vlan.Bezeichnung,
              Vlan = m.Vlan.Ip,
              Netmask = m.Vlan.Netmask,
              Ip = (long)m.Ip,
              Mac = m.Adresse
            }).ToList() 
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
        });
    }

    /*
     * tmpAp-Liste in Liste mit Arbeitsplatz-Objekten umwandeln.
     */
    private List<Arbeitsplatz> Convert(List<tmpAp> tmp) {
      List<Arbeitsplatz> aps = new List<Arbeitsplatz>();
      foreach (tmpAp t in tmp) {
        Arbeitsplatz ap = new Arbeitsplatz {
          ApId = t.ApId,
          Apname = t.Apname,
          Bezeichnung = t.Bezeichnung,
          Aptyp = t.Aptyp,
          Oe = t.Oe,
          VerantwOe = t.VerantwOe,
          Tags = t.Tags
        };
        foreach (tmpHw h in t.Hw) {
          Hardware hw = new Hardware {
            Id = h.Id,
            Hersteller = h.Hersteller,
            Bezeichnung = h.Bezeichnung,
            Sernr = h.Sernr,
            Pri = h.Pri,
            Hwtyp = h.Hwtyp,
            HwtypFlag = h.HwtypFlag
          };
          if (h.Pri) {
            ap.Vlan = h.Vlan;
          }
          ap.Hw.Add(hw);
        }
        aps.Add(ap);
      }
      return aps;
    }
  }
}

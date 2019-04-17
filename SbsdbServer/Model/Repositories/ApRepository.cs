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

    /*
     * Einzelnen AP anhand der ID holen
     * 
     * Liefert Liste: check auf leere Liste muss aufrufende Routine erledigen.
     */
    public List<Arbeitsplatz> GetAp(long id) {
      return GetArbeitsplatzQuery(
        dbContext.Ap.Where(ap => ap.Id == id)
      ).ToList();
        
    }

    /*
     * Suchstring in AP-Name und AP-Bezeichnung suchen
     */
    public List<Arbeitsplatz> GetAps(string search) {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      string like = search.ToUpper();
      List<Arbeitsplatz> aps = GetArbeitsplatzQuery(
        dbContext.Ap.Where(a => a.Apname.ToUpper().Contains(like) || a.Bezeichnung.ToUpper().Contains(like))
      ).ToList();
      watch.Stop();
      long delta = watch.ElapsedMilliseconds;
      LOG.LogDebug("--- select-query for search '" + like + "' took " + delta + "ms");
      return aps;
    }

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

      List<Arbeitsplatz> aps = GetArbeitsplatzQuery(
        dbContext.Ap.Where(ap => oeids.Contains(ap.OeId))
      ).ToList();
      watch.Stop();
      long delta = watch.ElapsedMilliseconds;
      LOG.LogDebug("--- select-query for OE " + delta + "ms");
      return aps;
    }

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

    public List<Arbeitsplatz> QueryAps(ApQuery query) {
      throw new NotImplementedException();
    }

    /*
     * Abfrage auf Ap in Arbeitsplatz umwandeln
     * 
     * IN: Query auf Ap mit den notwendigen Bedingungen
     * OUT: Query ergaenzt um Select fuer Arbeitsplatz
     */
    private IQueryable<Arbeitsplatz> GetArbeitsplatzQuery(IQueryable<Ap> apq) {
      return apq
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
            Sernr = hw.SerNr,
            /* ?? hier anhaengen ??
            Vlan = hw.Mac.Select(m => new Netzwerk {
              VlanId = m.VlanId,
              Bezeichnung = m.Vlan.Bezeichnung,
              Vlan = m.Vlan.Ip,
              Netmask = m.Vlan.Netmask,
              Ip = (long)m.Ip,
              Mac = m.Adresse
            }).ToList()
            */
          }).ToList(),
          Tags = ap.ApTag.Select(t => new Tag {
            ApTagId = t.Id,
            TagId = t.TagtypId,
            Bezeichnung = t.Tagtyp.Bezeichnung,
            Text = t.Text,
            Param = t.Tagtyp.Param,
            Flag = t.Tagtyp.Flag,
            AptypId = t.Tagtyp.AptypId
          }).ToList(),
          // TODO so wird das nix -> zu langsam! Unter Hw? var in Arbeitsplatz konvertieren?
          Vlan = ap.Hw.Where(hw => hw.Pri == true).Count() == 1 
            ? ap.Hw.Where(hw => hw.Pri == true).First().Mac.Select(m => new Netzwerk {
                VlanId = m.VlanId,
                Bezeichnung = m.Vlan.Bezeichnung,
                Vlan = m.Vlan.Ip,
                Netmask = m.Vlan.Netmask,
                Ip = (long)m.Ip,
                Mac = m.Adresse
              }).ToList() 
            : new List<Netzwerk>()
        });
    }
  }
}

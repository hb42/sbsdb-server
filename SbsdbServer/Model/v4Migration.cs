using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.sbsdbv4.model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model {

  /*
   * ALte v4-DB auf neues Format migrieren
   * 
   * .ValueGeneratedOnAdd() muss fuer die IDs angeschaltet werden, sonst
   * wird ein vorhandener Index "0" nicht eingefuegt.
   * betrifft Aptyp, Apklasse, Oe
   * 
   * Am Ende koennte die DB fix auf auto increment umgestellt werden.
   * 
   */
  public class v4Migration {

    private readonly Sbsdbv4Context v4dbContext;
    private readonly SbsdbContext v5dbContext;
    private readonly ILogger LOG;

    public v4Migration(Sbsdbv4Context sbsdbv4, SbsdbContext sbsdbv5, ILogger<v4Migration> log) {
      v4dbContext = sbsdbv4;
      v5dbContext = sbsdbv5;
      LOG = log;
    }

    public string Run() {
      LOG.LogDebug("Start migrating v4-DB");

      MigAptyp();
      MigApklasse();
      MigHwtyp();
      MigHwkonfig();
      MigAussond();
      MigVlan();
      MigMac();
      MigIssuetyp();
      MigAdresse();
      MigOe();
      MigExtprog();
      MigTagtyp();
      MigAp();
      MigApissue();
      MigAptag();
      MigHw();
      MigHwhistory();

      return "not yet implemented";
    }

    private void MigAptyp() {
      var old = v4dbContext.SbsAptyp.
        OrderBy(t => t.AptypIndex).ToList();
      foreach(var o in old) {
        var n = new Aptyp {
          Id = o.AptypIndex,
          Bezeichnung = o.Aptyp,
          Flag = o.Flag
        };
        LOG.LogDebug("ApTyp add #" + n.Id);
        v5dbContext.Aptyp.Add(n);
      }
      LOG.LogDebug("ApTyp saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("ApTyp OK");
    }
    private void MigApklasse() {
      var old = v4dbContext.SbsApklasse
        .OrderBy(k => k.ApklasseIndex).ToList();
      foreach (var o in old) {
        var n = new Apklasse {
          Id = o.ApklasseIndex,
          Bezeichnung = o.Apklasse,
          Flag = o.Flag,
          AptypId = (long)o.AptypIndex
        };
        LOG.LogDebug("ApKlasse add #" + n.Id);
        v5dbContext.Apklasse.Add(n);
      }
      LOG.LogDebug("ApKlasse saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("ApKlasse OK");
    }
    private void MigHwtyp() {
      var old = v4dbContext.SbsHwtyp.ToList();
      foreach (var o in old) {
        var n = new Hwtyp {
          Id = o.HwtypIndex,
          Bezeichnung = o.Hwtyp,
          Flag = 0,
          AptypId = (long)o.AptypIndex
        };
        LOG.LogDebug("HwTyp add #" + n.Id);
        v5dbContext.Hwtyp.Add(n);
      }
      LOG.LogDebug("HwTyp saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("HwTyp OK");
    }
    private void MigHwkonfig() {
      var old = v4dbContext.SbsKonfig.ToList();
      foreach (var o in old) {
        var n = new Hwkonfig {
          Id = o.KonfigIndex,
          Bezeichnung = o.Bezeichnung,
          Hersteller = o.Hersteller,
          Hd = o.Hd,
          Prozessor = o.Prozessor,
          Ram = o.Ram,
          Sonst = o.Sonst,
          Video = o.Video,
          Nonasset = false,
          HwtypId = (long)o.HwtypIndex
        };
        LOG.LogDebug("HwKonfig add #" + n.Id);
        v5dbContext.Hwkonfig.Add(n);
      }
      LOG.LogDebug("HwKonfig saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("HwKonfig OK");
    }
    private void MigAussond() {
      var old = v4dbContext.SbsAussond.ToList();
      foreach (var o in old) {
        var n = new Aussond {
          Id = o.HwIndex,
          SerNr = o.SerNr,
          AnschDat = o.AnschDat,
          AnschWert = (decimal)(o.AnschWert ?? 0),
          InvNr = o.InvNr,
          HwkonfigId = (long)o.KonfigIndex,
          Mac = o.Mac,
          Smbiosguid = o.Netbootguid,
          WartungBem = o.WartungBem,
          WartungFa = o.WartungFa,
          Bemerkung = "",
          AussDat = o.AussDat,
          AussGrund = o.AussGrund,
          Rewe = new DateTime(2018, 12, 14)
        };
        LOG.LogDebug("Aussond add #" + n.Id);
        v5dbContext.Aussond.Add(n);
      }
      LOG.LogDebug("Aussond saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("Aussond OK");
    }
    private void MigVlan() {
      // segment.index 0 -> id 1 mappen

    }
    private void MigMac() {

    }
    private void MigIssuetyp() {

    }
    private void MigAdresse() {

    }
    private void MigOe() {
      // order by oeindex!
    }
    private void MigExtprog() {

    }
    private void MigTagtyp() {

    }
    private void MigAp() {

    }
    private void MigApissue() {

    }
    private void MigAptag() {

    }
    private void MigHw() {

    }
    private void MigHwhistory() {

    }

  }
}

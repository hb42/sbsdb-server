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
      var old = v4dbContext.SbsAptyp.ToList();
      foreach(var o in old) {
        var n = new Aptyp {
          Id = o.AptypIndex,
          Aptyp1 = o.Aptyp,
          Flag = o.Flag
        };
        LOG.LogDebug("ApTyp add #" + n.Id);
        v5dbContext.Aptyp.Add(n);
      }
      LOG.LogDebug("ApTyp saving...");
      v5dbContext.SaveChanges();
    }
    private void MigApklasse() {

    }
    private void MigHwtyp() {

    }
    private void MigHwkonfig() {

    }
    private void MigAussond() {

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

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
   */

  /* TODO Abhaengigkeiten, die bei der DB-Erstellung beruecksichtigt werden muessen, d.h.
   *      beim Programm-Start muessen diese Dinge ueberprueft und ggf. korrigiert werden.
   * 
   *      OE
   *      Die Ueber- Unterordnung wird mit dem Feld parent abgebildet. Das Feld
   *      bekommt die ID der naechsthoeheren OE. Der root-Knoten muss ID 0 und
   *      parent 0 bekommen (Name "Sparkasse" || "Gesamthaus" ...).   
   *      
   *      APTYP/ APKLASSE
   *      Peripherie hat Index 0 (koennte zusaetzlichen Wert in flag sparen)
   *      
   *      KONFIG
   *      Fuer jeden HWTYP eine Konfig anlegen, mit NONASSET=true + Bezeichnung/Hersteller
   *      "fremde HW" o.ae.
   */

  /* Migration
   * *********
   * 1. DDL aus Data Modeler in Oracle ausfuehren -> leere Tabellen
   *    (identity column muss jeweils im physical model eingestellt werden)
   *    
   * 2. Code-Generierung
   *    > dotnet ef dbcontext scaffold "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=e077spwmve2131n)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=E077SPKP)));User ID=SBSDB_MASTER;Password=sbsdbpwd;", "Oracle.EntityFrameworkCore" --context SbsdbContext --context-dir Model --output-dir Model/Entities --no-build --force

   *    Anpassungen in automatisch erstellten Entites/Context
   * 
   *    SbsdbContext.cs
   *    - leeren c'tor raus
   *   
   *    - Namespace aendern (.Model statt .Model.Entities)
   *   
   *    - modelBuilder.Entity<UserSettings>(entity => {
   *        ...
   *        entity.Property(e => e.Settings)
   *             .HasColumnName("SETTINGS")
   *             .HasColumnType("clob")
                 .HasConversion(
                   v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                   v => JsonConvert.DeserializeObject<ViewModel.UserSession>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
   * 
   *    - .ValueGeneratedOnAdd() muss fuer die IDs angeschaltet werden, sonst
   *      wird ein vorhandener Index "0" bei der Migration nicht eingefuegt.
   *      betrifft Aptyp, 
   *               Apklasse, 
   *               Oe,
   *               Vlan
   *      Alt.: identity column mit "START WITH 0 MINVALUE 0" anlegen  
   *      [aus igrendeinem Grund wird .ValueGeneratedOnAdd() nicht mehr generiert,
   *       EF scheint aber trotzdem den auto increment zu beruecksichtigen (wenn id == 0 || id == null
   *       wird das Feld nicht im insert angegeben => seq.nextval). 
   *       Das ist noch genau auszutesten, dto. die fn .UseOracleIdentityColumn().]
   *            
   *    - Sequences koennen raus         
   *   
   *    UserSettings.cs
           // gueltiges Objekt sicherstellen
           private UserSession _settings;
           public UserSession Settings {
             get => _settings ?? new UserSession(Userid);
             set => _settings = value ?? new UserSession(Userid);
   * 
   * 3. Migration
   *    -> 791/sbsdb/ws/test/migration
   *    
   *    
   * 4. Nach der Migration
   * 
   *    Nach Import die identity columns auf Automatik und den hoechsten aktuellen Wert stellen:
   * 
   *      ALTER TABLE <table> MODIFY id
   *      GENERATED ALWAYS AS IDENTITY (START WITH LIMIT VALUE);
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
      MigMac();

      // TODO Statistik -> Tag

      return "done";
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
    private void MigApklasse() {  // geht's auch ohne -> als Tag abbilden (Param nutzen)
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
      var old = v4dbContext.SbsSegment
        .OrderBy(k => k.SegmentIndex).ToList();
      foreach (var o in old) {
        var n = new Vlan {
          Id = o.SegmentIndex,
          Bezeichnung = o.SegmentName,
          Ip = o.Tcp,
          Netmask = o.Netmask
        };
        LOG.LogDebug("Vlan add #" + n.Id);
        v5dbContext.Vlan.Add(n);
      }
      LOG.LogDebug("Vlan saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("Vlan OK");
    }
    private void MigIssuetyp() {
      var old = v4dbContext.SbsTtKategorie.ToList();
      foreach (var o in old) {
        var n = new Issuetyp {
          Id = o.KategorieIndex,
          Bezeichnung = o.Kategorie,
          Flag = o.Flag
        };
        LOG.LogDebug("IssueTyp add #" + n.Id);
        v5dbContext.Issuetyp.Add(n);
      }
      LOG.LogDebug("IssueTyp saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("IssueTyp OK");

    }
    private void MigAdresse() {
      var old = v4dbContext.SbsFiliale.ToList();
      foreach (var o in old) {
        var n = new Adresse {
          Id = o.FilialeIndex,
          Hausnr = o.Hausnr,
          Ort = o.Ort,
          Plz = o.Plz,
          Strasse = o.Strasse
        };
        LOG.LogDebug("Adresse add #" + n.Id);
        v5dbContext.Adresse.Add(n);
      }
      LOG.LogDebug("Adresse saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("Adresse OK");

    }
    private void MigOe() {
      var old = v4dbContext.SbsOe
        .OrderBy(k => k.OeIndex).ToList();
      foreach (var o in old) {
        var n = new Oe {
          Id = o.OeIndex,
          AdresseId = (long)o.FilialeIndex,
          Ap = (o.Ap ?? 0) > 0 ? true : false,
          Betriebsstelle = o.Betriebsstelle,
          Bst = o.Bst,
          Fax = o.Fax,
          Oeff = o.Oeff,
          OeId = o.ParentOe,
          Tel = o.Tel,
        };
        LOG.LogDebug("OE add #" + n.Id);
        v5dbContext.Oe.Add(n);
      }
      LOG.LogDebug("Oe saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("Oe OK");
    }
    private void MigExtprog() {
      var old = v4dbContext.SbsExtprog.ToList();
      foreach (var o in old) {
        var n = new Extprog {
          Id = o.ExtprogIndex,
          Bezeichnung = o.Extprog,
          ApklasseId = (long)o.ApklasseIndex,
          ExtprogName = o.ExtprogName,
          ExtprogPar = o.ExtprogPar,
          Flag = o.Flag
        };
        LOG.LogDebug("ExtProg add #" + n.Id);
        v5dbContext.Extprog.Add(n);
      }
      LOG.LogDebug("ExtProg saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("ExtProg OK");

    }
    private void MigTagtyp() {
      var old = v4dbContext.SbsAdrtyp.ToList();
      foreach (var o in old) {
        var n = new Tagtyp {
          Id = o.AdrIndex,
          Bezeichnung = o.AdrTyp,
          AptypId = (long)o.AptypIndex,
          Flag = o.Flag,
          Param = o.Param
        };
        LOG.LogDebug("TagTyp add #" + n.Id);
        v5dbContext.Tagtyp.Add(n);
      }
      LOG.LogDebug("TagTyp saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("TagTyp OK");
    }
    private void MigAp() {
      var old = v4dbContext.SbsAp.ToList();
      foreach (var o in old) {
        var n = new Ap {
          Id = o.ApIndex,
          Bezeichnung = o.Bezeichnung,
          ApklasseId = (long)o.ApklasseIndex,
          Apname = o.ApName,
          Bemerkung = o.Bemerkung,
          OeId = (long)o.StandortIndex,
          OeIdVerOe = o.OeIndex
        };
        LOG.LogDebug("AP add #" + n.Id);
        v5dbContext.Ap.Add(n);
      }
      LOG.LogDebug("AP saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("AP OK");
    }
    private void MigApissue() {
      var old = v4dbContext.SbsAp.ToList();
      foreach (var o in old) {
        var n = new Ap {
          Id = o.ApIndex,
          Bezeichnung = o.Bezeichnung,
          ApklasseId = (long)o.ApklasseIndex,
          Apname = o.ApName,
          Bemerkung = o.Bemerkung,
          OeId = (long)o.StandortIndex,
          OeIdVerOe = o.OeIndex
        };
        LOG.LogDebug("AP add #" + n.Id);
        v5dbContext.Ap.Add(n);
      }
      LOG.LogDebug("AP saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("AP OK");

    }
    private void MigAptag() {
      var old = v4dbContext.SbsApAdr.ToList();
      foreach (var o in old) {
        var n = new ApTag {
          Id = o.ApadrIndex,
          ApId = o.ApIndex,
          TagtypId = o.AdrIndex,
          Text = o.AdrText
        };
        LOG.LogDebug("ApTag add #" + n.Id);
        v5dbContext.ApTag.Add(n);
      }
      LOG.LogDebug("ApTag saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("ApTag OK");

    }
    private void MigHw() {
      var old = v4dbContext.SbsHw.ToList();
      foreach (var o in old) {
        var n = new Hw {
          Id = o.HwIndex,
          ApId = o.ApIndex,
          AnschDat = o.AnschDat,
          AnschWert = (decimal)o.AnschWert,
          Bemerkung = o.WartungBem,
          HwkonfigId = (long)o.KonfigIndex,
          InvNr = o.InvNr,
          Pri = o.Pri == "J",
          SerNr = o.SerNr,
          Smbiosguid = o.Netbootguid,
          WartungFa = o.WartungFa,  // ?
          WartungBem = o.WartungBem  // ?
        };
        LOG.LogDebug("Hw add #" + n.Id);
        v5dbContext.Hw.Add(n);
      }
      LOG.LogDebug("Hw saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("Hw OK");

    }
    private void MigHwhistory() {
      var old = v4dbContext.SbsHwshift.ToList();
      foreach (var o in old) {
        var n = new Hwhistory {
          Id = o.HwshiftIndex,
          ApId = o.ApIndex,
          ApBezeichnung = o.Bezeichnung,
          Apname = o.Host,
          Betriebsstelle = o.Betriebsstelle,
          Direction = o.Direction,
          Shiftdate = o.Shiftdate,
          HwId = (long)o.HwIndex
        };
        LOG.LogDebug("HwHistory add #" + n.Id);
        v5dbContext.Hwhistory.Add(n);
      }
      LOG.LogDebug("HwHistory saving...");
      v5dbContext.SaveChanges();
      LOG.LogDebug("HwHistory OK");

    }
    private void MigMac() {

    }

  }
}

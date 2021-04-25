using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.sbsdbv4.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger LOG;

        private readonly string macRegex =
            @"(?<=\W|^)([0-9,a-f,A-F]{2})[-:]?([0-9,a-f,A-F]{2})[-:]?([0-9,a-f,A-F]{2})[-:]?([0-9,a-f,A-F]{2})[-:]?([0-9,a-f,A-F]{2})[-:]?([0-9,a-f,A-F]{2})(\W|$)";

        private readonly Sbsdbv4Context v4dbContext;
        private readonly SbsdbContext v5dbContext;

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
            // MigExtprog();
            MigTagtyp();
            MigAp();
            MigApissue();
            MigAptag();
            MigHw();
            MigHwhistory();
            MigMac();
            MigKlasseStatistik();

            // TODO Statistik + Klasse -> Tag

            LOG.LogDebug("Migration DONE");
            return "done";
        }

        private void MigAptyp() {
            var old = v4dbContext.SbsAptyp.Where(t => t.AptypIndex != 5).OrderBy(t => t.AptypIndex).ToList();
            foreach (var o in old) {
                var n = new Apkategorie {
                    Id = o.AptypIndex == 0 ? 10 : o.AptypIndex,
                    Bezeichnung = o.AptypIndex == 1 ? "Client" : o.Aptyp,
                    Flag = o.AptypIndex == 0 ? 1 : o.Flag
                };
                LOG.LogDebug("ApKategorie add #" + n.Id);
                v5dbContext.Apkategorie.Add(n);
            }

            LOG.LogDebug("ApKategorie saving...");
            v5dbContext.SaveChanges();
            LOG.LogDebug("ApKategorie OK");
        }

        private void MigApklasse() { // geht's auch ohne -> als Tag abbilden (Param nutzen)
            var old = v4dbContext.SbsApklasse.Where((k => k.ApklasseIndex > 0))
                .OrderBy(k => k.ApklasseIndex).ToList();
            foreach (var o in old) {
                var n = new Aptyp {
                    Id = o.ApklasseIndex,
                    Bezeichnung = o.Apklasse,
                    Flag = o.Flag,
                    ApkategorieId = o.AptypIndex == 5 ? 1 : (long) o.AptypIndex
                };
                LOG.LogDebug("ApTyp add #" + n.Id);
                v5dbContext.Aptyp.Add(n);
            }

            LOG.LogDebug("ApTyp saving...");
            v5dbContext.SaveChanges();
            LOG.LogDebug("ApTyp OK");
        }

        private void MigHwtyp() {
            var old = v4dbContext.SbsHwtyp.ToList();
            foreach (var o in old) {
                var n = new Hwtyp {
                    Id = o.HwtypIndex,
                    Bezeichnung = o.Hwtyp,
                    Flag = 0,
                    ApkategorieId = o.AptypIndex == 0 ? 10 : o.AptypIndex == 5 ? 1 : (long) o.AptypIndex
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
//          Nonasset = false,
                    HwtypId = (long) o.HwtypIndex
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
                    AnschWert = (decimal) (o.AnschWert ?? 0),
                    InvNr = o.InvNr,
                    HwkonfigId = (long) o.KonfigIndex,
                    Mac = o.Mac,
                    Smbiosguid = o.Netbootguid,
                    WartungFa = o.WartungFa,
                    Bemerkung = o.WartungBem,
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
                    Id = (o.SegmentIndex == 0 ? 1 : o.SegmentIndex),
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
                    AdresseId = (long) o.FilialeIndex,
                    Ap = (o.Ap ?? 0) > 0 ? true : false,
                    Betriebsstelle = o.Betriebsstelle,
                    Bst = o.Bst,
                    Fax = o.Fax,
                    Oeff = o.Oeff,
                    OeId = o.ParentOe == 0 ? null : o.ParentOe,
                    Tel = o.Tel
                };
                LOG.LogDebug("OE add #" + n.Id);
                v5dbContext.Oe.Add(n);
            }

            LOG.LogDebug("Oe saving...");
            v5dbContext.SaveChanges();
            LOG.LogDebug("Oe OK");
        }

        private void MigExtprog() {
            var old = v4dbContext.SbsExtprog.Include(e => e.ApklasseIndexNavigation).ToList();
            foreach (var o in old) {
                var n = new Extprog {
                    Id = o.ExtprogIndex,
                    Bezeichnung = o.Extprog,
                    ApkategorieId = (long) o.ApklasseIndex,
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
                    ApkategorieId = o.AptypIndex == 5 ? 1 : (long) o.AptypIndex,
                    Flag = 0,
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
            var old = v4dbContext.SbsAp.Include(a => a.ApklasseIndexNavigation).ToList();
            foreach (var o in old) {
                var n = new Ap {
                    Id = o.ApIndex,
                    Bezeichnung = o.Bezeichnung,
                    AptypId = (long) o.ApklasseIndex,
                    Apname = o.ApName,
                    Bemerkung = o.Bemerkung,
                    OeId = (long) o.StandortIndex,
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
            var old = v4dbContext.SbsTtIssue.Include(t => t.UserIndexNavigation).ToList();
            foreach (var o in old) {
                var n = new ApIssue {
                    Id = o.TtissueIndex,
                    ApId = o.ApIndex,
                    Close = o.Close,
                    Issue = o.Ticket,
                    IssuetypId = o.KategorieIndex,
                    Open = o.Open,
                    Prio = o.Prio,
                    Userid = o.UserIndexNavigation.UserId
                };
                LOG.LogDebug("ApIssue add #" + n.Id);
                v5dbContext.ApIssue.Add(n);
            }

            LOG.LogDebug("ApIssue saving...");
            v5dbContext.SaveChanges();
            LOG.LogDebug("ApIssue OK");
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
                    AnschWert = (decimal) o.AnschWert,
                    Bemerkung = o.WartungBem,
                    HwkonfigId = (long) o.KonfigIndex,
                    InvNr = o.InvNr,
                    Pri = o.Pri == "J",
                    SerNr = o.SerNr,
                    Smbiosguid = o.Netbootguid,
                    WartungFa = o.WartungFa // ?
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
                    HwId = (long) o.HwIndex
                };
                LOG.LogDebug("HwHistory add #" + n.Id);
                v5dbContext.Hwhistory.Add(n);
            }

            LOG.LogDebug("HwHistory saving...");
            v5dbContext.SaveChanges();
            LOG.LogDebug("HwHistory OK");
        }

        private void MigMac() {
            // AP mit zugeordneter HW
            var hws = v4dbContext.SbsHw
                .Where(h => h.ApIndex != null && h.Pri == "J" && h.Mac != null)
                .Select(h => new {
                    mac = h.Mac,
                    ip = h.ApIndexNavigation.Tcp,
                    seg = (h.ApIndexNavigation.SegmentIndex == 0 ? 1 : h.ApIndexNavigation.SegmentIndex),
                    hw = h.HwIndex
                })
                .ToList();
            foreach (var o in hws) {
                var n = new Mac {
                    Adresse = NormalizeMac(o.mac, false),
                    Ip = o.ip,
                    HwId = o.hw,
                    VlanId = (long) o.seg
                };
                LOG.LogDebug("MAC add " + n.Adresse);
                v5dbContext.Mac.Add(n);
            }

            LOG.LogDebug("MAC saving ...");
            v5dbContext.SaveChanges();

            // sonstige MACs
            //      new HWTYP "Fremde HW - {aptyp.bezeichnung}" je APTYP, flag = 1
            long idx = 100;
            var aptyp = v5dbContext.Apkategorie.ToList();
            foreach (var at in aptyp) {
                var n = new Hwtyp {
                    Id = idx,
                    Bezeichnung = "Fremde HW - " + at.Bezeichnung,
                    Flag = 1,
                    ApkategorieId = at.Id
                };
                LOG.LogDebug("new HwTyp add " + n.Bezeichnung);
                v5dbContext.Hwtyp.Add(n);
                var t = new Aptyp {
                    Id = idx++,
                    Bezeichnung = "Adresse - " + at.Bezeichnung,
                    Flag = 1,
                    ApkategorieId = at.Id
                };
                LOG.LogDebug("new ApTyp add " + t.Bezeichnung);
                v5dbContext.Aptyp.Add(t);
            }

            LOG.LogDebug("new Ap/HwTyp saving ...");
            v5dbContext.SaveChanges();
            //      new HWKONFIG Hersteller="Fremde Hardware", Bezeichnung=aptyp.bezeichnung je HWTYP.where(flag == 1)
            var hwtyp = v5dbContext.Hwtyp
                .Where(h => h.Flag == 1)
                .Select(h => new {
                    hwtyp = h.Id,
                    apkat = h.Apkategorie.Bezeichnung
                })
                .ToList();
            foreach (var ht in hwtyp) {
                var n = new Hwkonfig {
                    Id = ht.hwtyp,
                    Hersteller = "Fremde Hardware",
                    Bezeichnung = ht.apkat,
                    HwtypId = ht.hwtyp
                };
                LOG.LogDebug("new HwKonfig " + n.Bezeichnung);
                v5dbContext.Hwkonfig.Add(n);
            }

            LOG.LogDebug("new HwKonfig saving ...");
            v5dbContext.SaveChanges();

            //      liste v5Konfig.where(konfig.hwtyp.flag == 1).select(konfigID, konfig.hwtyp.aptypID)
            var konfigs = v5dbContext.Hwkonfig
                .Where(k => k.Hwtyp.Flag == 1)
                .Select(k => new {
                    konfig = k.Id,
                    apkat = k.Hwtyp.ApkategorieId
                })
                .ToList();
            //      liste ap.where(hws.pri != 'J' and ip > 0)
            idx = 300;
            var aps = v4dbContext.SbsAp.Include(a => a.SbsHw).Include(a => a.ApklasseIndexNavigation).ToList();
            foreach (var ap in aps)
                if (ap.SbsHw.Count == 0 && ap.SegmentIndex != 0 && ap.OeIndex != 999) {
                    //          new Hw pri=true, ap=ap.apID, hwkonfig=liste(where aptyp = ap.aptyp), sernr=apname
                    var h = new Hw {
                        Id = idx++,
                        Pri = true,
                        ApId = ap.ApIndex,
                        HwkonfigId = konfigs.First(k => k.apkat == (ap.ApklasseIndexNavigation.AptypIndex == 5 ? 1 : ap.ApklasseIndexNavigation.AptypIndex)).konfig,
                        SerNr = ap.ApName
                    };
                    LOG.LogDebug("new Hw" + h.SerNr);
                    v5dbContext.Hw.Add(h);
                    // v5dbContext.SaveChanges();
                    //          new mac ip=ap.ip, vlan=ap.seg, adresse=searchMac(ap.bemerkung)|0, hw=(new Hw)
                    var m = new Mac {
                        Adresse = NormalizeMac(ap.Bemerkung, false),
                        Ip = ap.Tcp,
                        HwId = h.Id,
                        VlanId = (long) ap.SegmentIndex
                    };
                    LOG.LogDebug("new MAC " + m.Adresse);
                    v5dbContext.Mac.Add(m);
                }
            v5dbContext.SaveChanges();

            // fuer APs mit fremder HW ApTyp "Adresse" setzen
            var types = v5dbContext.Aptyp.Where(t => t.Flag == 1);
            var fhw = v5dbContext.Hw.Where(h => h.Pri && h.Hwkonfig.Hwtyp.Flag == 1).ToList();

            foreach (var hw in fhw) {
                var ap = v5dbContext.Ap.Find(hw.ApId);
                var typ = types.First(t => t.ApkategorieId == hw.Hwkonfig.Hwtyp.ApkategorieId);
                ap.AptypId = typ.Id;
                LOG.LogDebug("ApTyp f. fremde HW " + ap.Apname);
                v5dbContext.Ap.Update(ap);
            }
            v5dbContext.SaveChanges();
        }

        private void MigKlasseStatistik() {
            var klasse = new List<conv>();
            var statistik = new List<conv>();
            long idx = 100;
            var klas = v4dbContext.SbsApklasse.Where((k => k.ApklasseIndex > 0)).Include(k => k.AptypIndexNavigation).ToList();
            foreach (var k in klas) {
                var n = new Tagtyp {
                    Id = idx++,
                    Flag = 1,
                    Bezeichnung = k.Apklasse,
                    ApkategorieId = k.AptypIndex == 5 ? 1 : (long) k.AptypIndex
                };
                LOG.LogDebug("TagTyp add ApKlasse " + n.Bezeichnung);
                v5dbContext.Tagtyp.Add(n);
                klasse.Add(new conv {
                    oldId = k.ApklasseIndex,
                    newId = n.Id,
                    text = k.AptypIndexNavigation.Aptyp
                });
            }

            var stat = v4dbContext.SbsApstatistik.Include(s => s.AptypIndexNavigation).ToList();
            foreach (var s in stat) {
                var n = new Tagtyp {
                    Id = idx++,
                    Flag = 1,
                    Bezeichnung = s.Apstatistik,
                    ApkategorieId = s.AptypIndex == 5 ? 1 : (long) s.AptypIndex
                };
                LOG.LogDebug("TagTyp add ApStatistik" + n.Bezeichnung);
                v5dbContext.Tagtyp.Add(n);
                statistik.Add(new conv {
                    oldId = s.ApstatistikIndex,
                    newId = n.Id,
                    text = s.AptypIndexNavigation.Aptyp
                });
            }

            LOG.LogDebug("TagTyp for Klasse/Statistik saving ...");
            v5dbContext.SaveChanges();

            // get v4.AP apklasse + apstatistik -> new 
            var aps = v4dbContext.SbsAp.ToList();
            foreach (var ap in aps) {
                var k = new ApTag {
                    ApId = ap.ApIndex,
                    TagtypId = klasse.First(kl => kl.oldId == ap.ApklasseIndex).newId,
                    Text = klasse.First(kl => kl.oldId == ap.ApklasseIndex).text
                };
                LOG.LogDebug("ApTag add apklasse");
                v5dbContext.ApTag.Add(k);
            }

            foreach (var ap in aps) {
                var s = new ApTag {
                    ApId = ap.ApIndex,
                    TagtypId = statistik.First(st => st.oldId == ap.ApstatistikIndex).newId,
                    Text = statistik.First(st => st.oldId == ap.ApstatistikIndex).text
                };
                LOG.LogDebug("ApTag add apstatistik");
                v5dbContext.ApTag.Add(s);
            }

            LOG.LogDebug("ApTag for Klasse/Statistik saving ...");
            v5dbContext.SaveChanges();
        }

        // TODO die fn nach Common auslagern (ggf. zusammen mit IP-Adressen-Umrechnungen)
        /*
         * MAC-Adresse aus einem String extrahieren und in einen 
         * Hex-String (friendly == false) oder einen
         * MAC-String getrennt durch "-" (friendly == true)
         * umwandeln.
         * 
         * Sofern keine MAC-Adresse gefunden werden kann, wird "000000000000"
         * zurueckgegeben.    
         * 
         * Eine SMBIOSGUID wuerde hier ebenfalls als MAC-Adresse erkannt werden!    
         */
        private string NormalizeMac(string mac, bool friendly) {
            var spacer = "";
            var m = Regex.Match(mac, macRegex);
            if (m.Success) {
                if (friendly) spacer = "-";
                return (m.Groups[1].Value + spacer +
                        m.Groups[2].Value + spacer +
                        m.Groups[3].Value + spacer +
                        m.Groups[4].Value + spacer +
                        m.Groups[5].Value + spacer +
                        m.Groups[6].Value).ToUpper();
            }

            return "000000000000";
        }

        private class conv {
            public long oldId { get; set; }
            public long newId { get; set; }
            public string text { get; set; }
        }
    }
}

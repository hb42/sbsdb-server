using hb.SbsdbServer.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace hb.SbsdbServer.Model {
  public class SbsdbContext_tmp: DbContext {
    /* TODO Abhaengigkeiten, die bei der DB-Erstellung beruecksichtigt werden muessen
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

    /* Anpassungen in automatisch erstellten Entites/Context
     * 
     * SbsdbContext
     *   - leeren c'tor raus
     *   
     *   - UserSettings -> Settings
               .HasConversion(
                 v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                 v => JsonConvert.DeserializeObject<ViewModel.UserSession>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
     * 
     *   - .ValueGeneratedOnAdd() muss fuer die IDs angeschaltet werden, sonst
     *     wird ein vorhandener Index "0" nicht eingefuegt.
     *     betrifft Aptyp, 
     *              Apklasse, 
     *              Oe
     *            
     *   - Sequences koennen raus         
     *   
     * UserSettings
     *    public UserSession Settings { get; set; }
     * 
     * 
     */

    // tables
    public virtual DbSet<Adresse> Adresse { get; set; }
    public virtual DbSet<UserSettings> UserSettings { get; set; }
    public virtual DbSet<ProgramSettings> ProgramSettings { get; set; }

    public SbsdbContext_tmp(DbContextOptions<SbsdbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<Adresse>(entity => {
        entity.ToTable("ADRESSE");

        entity.Property(e => e.Id)
          .HasColumnName("ID")
          .HasColumnType("number(19)")
          .UseOracleIdentityColumn();
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Hausnr)
          .HasColumnName("HAUSNR")
          .HasColumnType("varchar(50)");
        entity.Property(e => e.Ort)
          .HasColumnName("ORT")
          .HasColumnType("varchar(100)");
        entity.Property(e => e.Plz)
          .HasColumnName("PLZ")
          .HasColumnType("varchar(50)");
        entity.Property(e => e.Strasse)
          .HasColumnName("STRASSE")
          .HasColumnType("varchar(100)");
      });

      modelBuilder.Entity<UserSettings>(entity => {
        entity.ToTable("USER_SETTINGS");

        entity.Property(e => e.Id)
          .HasColumnName("ID")
          .HasColumnType("number(19)")
          .UseOracleIdentityColumn();
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Userid)
          .HasColumnName("UID")
          .HasColumnType("varchar(20)");
        entity.HasIndex(e => e.Userid).IsUnique();

        // Objekt-Typ UserSession als JSON im Feld ablegen
/*        entity.Property(e => e.Settings)
          .HasColumnName("SETTINGS")
          .HasColumnType("clob")
          .HasConversion(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            v => JsonConvert.DeserializeObject<UserSession>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
*/
      });

      modelBuilder.Entity<ProgramSettings>(entity => {
        entity.ToTable("PROGRAM_SETTINGS");

        entity.Property(e => e.Id)
          .HasColumnName("ID")
          .HasColumnType("number(19)")
          .UseOracleIdentityColumn();
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Key)
          .HasColumnName("KEY")
          .HasColumnType("varchar(100)");
        entity.HasIndex(e => e.Key).IsUnique();

        entity.Property(e => e.Value)
          .HasColumnName("VALUE")
          .HasColumnType("varchar(2000)");
      });

    }
  }
}

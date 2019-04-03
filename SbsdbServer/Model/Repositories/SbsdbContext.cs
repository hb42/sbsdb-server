using hb.SbsdbServer.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace hb.SbsdbServer.Model.Repositories {
  public class SbsdbContext: DbContext {
    // tables
    public virtual DbSet<UserSettings> UserSettings { get; set; }

    public SbsdbContext(DbContextOptions<SbsdbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      modelBuilder.Entity<UserSettings>(entity => {
        entity.ToTable("USER_SETTINGS");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id)
          .HasColumnName("ID")
          .HasColumnType("number(19)")
          .UseOracleIdentityColumn();  

        entity.Property(e => e.Uid)
          .HasColumnName("UID")
          .HasColumnType("varchar(20)");

        // Objekt-Typ User als JSON im Feld ablegen
        entity.Property(e => e.Settings)
          .HasColumnName("SETTINGS")
          .HasColumnType("clob")
          .HasConversion(
            v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
            v => JsonConvert.DeserializeObject<User>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

      });

    }
  }
}

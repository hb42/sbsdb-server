using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class SbsdbContext : DbContext
    {
        public SbsdbContext()
        {
        }

        public SbsdbContext(DbContextOptions<SbsdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adresse> Adresse { get; set; }
        public virtual DbSet<Ap> Ap { get; set; }
        public virtual DbSet<ApIssue> ApIssue { get; set; }
        public virtual DbSet<ApTag> ApTag { get; set; }
        public virtual DbSet<Apklasse> Apklasse { get; set; }
        public virtual DbSet<Aptyp> Aptyp { get; set; }
        public virtual DbSet<Aussond> Aussond { get; set; }
        public virtual DbSet<Extprog> Extprog { get; set; }
        public virtual DbSet<Hw> Hw { get; set; }
        public virtual DbSet<Hwhistory> Hwhistory { get; set; }
        public virtual DbSet<Hwkonfig> Hwkonfig { get; set; }
        public virtual DbSet<Hwtyp> Hwtyp { get; set; }
        public virtual DbSet<Issuetyp> Issuetyp { get; set; }
        public virtual DbSet<Mac> Mac { get; set; }
        public virtual DbSet<Oe> Oe { get; set; }
        public virtual DbSet<ProgramSettings> ProgramSettings { get; set; }
        public virtual DbSet<Tagtyp> Tagtyp { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<Vlan> Vlan { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=nathan)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=XEPDB1)));User ID=SBSDB_MASTER;Password=sbsdbpw;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:DefaultSchema", "SBSDB_MASTER");

            modelBuilder.Entity<Adresse>(entity =>
            {
                entity.ToTable("ADRESSE");

                entity.HasIndex(e => e.Id)
                    .HasName("ADRESSE_PK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Hausnr)
                    .HasColumnName("HAUSNR")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Ort)
                    .HasColumnName("ORT")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Plz)
                    .HasColumnName("PLZ")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Strasse)
                    .HasColumnName("STRASSE")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<Ap>(entity =>
            {
                entity.ToTable("AP");

                entity.HasIndex(e => e.ApklasseId)
                    .HasName("AP_APTYP_ID_IDX");

                entity.HasIndex(e => e.Apname)
                    .HasName("AP_APNAME_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("AP_PK")
                    .IsUnique();

                entity.HasIndex(e => e.OeId)
                    .HasName("AP_OE_ID_IDX");

                entity.HasIndex(e => e.OeIdVerOe)
                    .HasName("AP_OE_ID1_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApklasseId)
                    .HasColumnName("APKLASSE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Apname)
                    .IsRequired()
                    .HasColumnName("APNAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Bemerkung)
                    .HasColumnName("BEMERKUNG")
                    .HasColumnType("CLOB");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasColumnName("BEZEICHNUNG")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.OeId)
                    .HasColumnName("OE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.OeIdVerOe)
                    .HasColumnName("OE_ID_VER_OE")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.Apklasse)
                    .WithMany(p => p.Ap)
                    .HasForeignKey(d => d.ApklasseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AP_APKLASSE_FK");

                entity.HasOne(d => d.Oe)
                    .WithMany(p => p.ApOe)
                    .HasForeignKey(d => d.OeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AP_OE_FK");

                entity.HasOne(d => d.OeIdVerOeNavigation)
                    .WithMany(p => p.ApOeIdVerOeNavigation)
                    .HasForeignKey(d => d.OeIdVerOe)
                    .HasConstraintName("AP_OE_FK_VER_OE");
            });

            modelBuilder.Entity<ApIssue>(entity =>
            {
                entity.ToTable("AP_ISSUE");

                entity.HasIndex(e => e.ApId)
                    .HasName("AP_ISSUE_AP_ID_IDX");

                entity.HasIndex(e => e.Close)
                    .HasName("AP_ISSUE_CLOSE_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("AP_ISSUE_PK")
                    .IsUnique();

                entity.HasIndex(e => e.IssuetypId)
                    .HasName("AP_ISSUE_ISSUETYP_ID_IDX");

                entity.HasIndex(e => e.Open)
                    .HasName("AP_ISSUE_OPEN_IDX");

                entity.HasIndex(e => e.Prio)
                    .HasName("AP_ISSUE_PRIO_IDX");

                entity.HasIndex(e => e.Userid)
                    .HasName("AP_ISSUE_USERID_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApId)
                    .HasColumnName("AP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Close)
                    .HasColumnName("CLOSE")
                    .HasColumnType("DATE");

                entity.Property(e => e.Issue)
                    .IsRequired()
                    .HasColumnName("ISSUE")
                    .HasColumnType("CLOB");

                entity.Property(e => e.IssuetypId)
                    .HasColumnName("ISSUETYP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Open)
                    .HasColumnName("OPEN")
                    .HasColumnType("DATE");

                entity.Property(e => e.Prio)
                    .HasColumnName("PRIO")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(20)");

                entity.HasOne(d => d.Ap)
                    .WithMany(p => p.ApIssue)
                    .HasForeignKey(d => d.ApId)
                    .HasConstraintName("AP_ISSUE_AP_FK");

                entity.HasOne(d => d.Issuetyp)
                    .WithMany(p => p.ApIssue)
                    .HasForeignKey(d => d.IssuetypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AP_ISSUE_ISSUETYP_FK");
            });

            modelBuilder.Entity<ApTag>(entity =>
            {
                entity.ToTable("AP_TAG");

                entity.HasIndex(e => e.ApId)
                    .HasName("AP_TAG_AP_ID_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("AP_TAG_PK")
                    .IsUnique();

                entity.HasIndex(e => e.TagText)
                    .HasName("AP_TAG_TAG_TEXT_IDX");

                entity.HasIndex(e => e.TagtypId)
                    .HasName("AP_TAG_TAGTYP_ID_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApId)
                    .HasColumnName("AP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.TagText)
                    .IsRequired()
                    .HasColumnName("TAG_TEXT")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.TagtypId)
                    .HasColumnName("TAGTYP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.Ap)
                    .WithMany(p => p.ApTag)
                    .HasForeignKey(d => d.ApId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AP_TAG_AP_FK");

                entity.HasOne(d => d.Tagtyp)
                    .WithMany(p => p.ApTag)
                    .HasForeignKey(d => d.TagtypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AP_TAG_TAGTYP_FK");
            });

            modelBuilder.Entity<Apklasse>(entity =>
            {
                entity.ToTable("APKLASSE");

                entity.HasIndex(e => e.AptypId)
                    .HasName("APKLASSE_APTYP_ID_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("APKLASSE_PK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Apklasse1)
                    .IsRequired()
                    .HasColumnName("APKLASSE")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.AptypId)
                    .HasColumnName("APTYP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.Aptyp)
                    .WithMany(p => p.Apklasse)
                    .HasForeignKey(d => d.AptypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("APKLASSE_APTYP_FK");
            });

            modelBuilder.Entity<Aptyp>(entity =>
            {
                entity.ToTable("APTYP");

                entity.HasIndex(e => e.Aptyp1)
                    .HasName("APTYP_APTYP_UN")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("APTYP_PK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Aptyp1)
                    .IsRequired()
                    .HasColumnName("APTYP")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("NUMBER(38)");
            });

            modelBuilder.Entity<Aussond>(entity =>
            {
                entity.ToTable("AUSSOND");

                entity.HasIndex(e => e.AussDat)
                    .HasName("AUSSOND_AUSS_DAT_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("AUSSOND_PK")
                    .IsUnique();

                entity.HasIndex(e => e.SerNr)
                    .HasName("AUSSOND_SER_NR_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AnschDat)
                    .HasColumnName("ANSCH_DAT")
                    .HasColumnType("DATE");

                entity.Property(e => e.AnschWert)
                    .HasColumnName("ANSCH_WERT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.AussDat)
                    .HasColumnName("AUSS_DAT")
                    .HasColumnType("DATE");

                entity.Property(e => e.AussGrund)
                    .HasColumnName("AUSS_GRUND")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Bemerkung)
                    .HasColumnName("BEMERKUNG")
                    .HasColumnType("CLOB");

                entity.Property(e => e.HwkonfigId)
                    .HasColumnName("HWKONFIG_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.InvNr)
                    .HasColumnName("INV_NR")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Mac)
                    .HasColumnName("MAC")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Rewe)
                    .HasColumnName("REWE")
                    .HasColumnType("DATE");

                entity.Property(e => e.SerNr)
                    .IsRequired()
                    .HasColumnName("SER_NR")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Smbiosguid)
                    .HasColumnName("SMBIOSGUID")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.WartungBem)
                    .HasColumnName("WARTUNG_BEM")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.WartungFa)
                    .HasColumnName("WARTUNG_FA")
                    .HasColumnType("VARCHAR2(50)");

                entity.HasOne(d => d.Hwkonfig)
                    .WithMany(p => p.Aussond)
                    .HasForeignKey(d => d.HwkonfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AUSSOND_HWKONFIG_FK");
            });

            modelBuilder.Entity<Extprog>(entity =>
            {
                entity.ToTable("EXTPROG");

                entity.HasIndex(e => e.Id)
                    .HasName("EXTPROG_PK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApklasseId)
                    .HasColumnName("APKLASSE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Extprog1)
                    .IsRequired()
                    .HasColumnName("EXTPROG")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.ExtprogName)
                    .IsRequired()
                    .HasColumnName("EXTPROG_NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.ExtprogPar)
                    .HasColumnName("EXTPROG_PAR")
                    .HasColumnType("VARCHAR2(255)");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.Apklasse)
                    .WithMany(p => p.Extprog)
                    .HasForeignKey(d => d.ApklasseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("EXTPROG_APKLASSE_FK");
            });

            modelBuilder.Entity<Hw>(entity =>
            {
                entity.ToTable("HW");

                entity.HasIndex(e => e.ApId)
                    .HasName("HW_AP_ID_IDX");

                entity.HasIndex(e => e.HwkonfigId)
                    .HasName("HW_HWKONFIG_ID_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("HW_PK")
                    .IsUnique();

                entity.HasIndex(e => e.SerNr)
                    .HasName("HW_SER_NR_IDX");

                entity.HasIndex(e => e.Smbiosguid)
                    .HasName("HW_SMBIOSGUID_UN")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AnschDat)
                    .HasColumnName("ANSCH_DAT")
                    .HasColumnType("DATE");

                entity.Property(e => e.AnschWert)
                    .HasColumnName("ANSCH_WERT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ApId)
                    .HasColumnName("AP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Bemerkung)
                    .HasColumnName("BEMERKUNG")
                    .HasColumnType("CLOB");

                entity.Property(e => e.HwkonfigId)
                    .HasColumnName("HWKONFIG_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.InvNr)
                    .HasColumnName("INV_NR")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Pri).HasColumnName("PRI");

                entity.Property(e => e.SerNr)
                    .IsRequired()
                    .HasColumnName("SER_NR")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Smbiosguid)
                    .HasColumnName("SMBIOSGUID")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.WartungBem)
                    .HasColumnName("WARTUNG_BEM")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.WartungFa)
                    .HasColumnName("WARTUNG_FA")
                    .HasColumnType("VARCHAR2(50)");

                entity.HasOne(d => d.Ap)
                    .WithMany(p => p.Hw)
                    .HasForeignKey(d => d.ApId)
                    .HasConstraintName("HW_AP_FK");

                entity.HasOne(d => d.Hwkonfig)
                    .WithMany(p => p.Hw)
                    .HasForeignKey(d => d.HwkonfigId)
                    .HasConstraintName("HW_HWKONFIG_FK");
            });

            modelBuilder.Entity<Hwhistory>(entity =>
            {
                entity.ToTable("HWHISTORY");

                entity.HasIndex(e => e.ApName)
                    .HasName("HWHISTORY_AP_NAME_IDX");

                entity.HasIndex(e => e.Direction)
                    .HasName("HWHISTORY_DIRECTION_IDX");

                entity.HasIndex(e => e.HwId)
                    .HasName("HWHISTORY_HW_ID_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("HWHISTORY_PK")
                    .IsUnique();

                entity.HasIndex(e => e.Shiftdate)
                    .HasName("HWHISTORY_SHIFTDATE_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApBezeichnung)
                    .HasColumnName("AP_BEZEICHNUNG")
                    .HasColumnType("VARCHAR2(55)");

                entity.Property(e => e.ApId)
                    .HasColumnName("AP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.ApName)
                    .HasColumnName("AP_NAME")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Betriebsstelle)
                    .HasColumnName("BETRIEBSSTELLE")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Direction)
                    .IsRequired()
                    .HasColumnName("DIRECTION")
                    .HasColumnType("VARCHAR2(2)");

                entity.Property(e => e.HwId)
                    .HasColumnName("HW_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Shiftdate)
                    .HasColumnName("SHIFTDATE")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql(@"localtimestamp(6)
        ");

                entity.HasOne(d => d.Hw)
                    .WithMany(p => p.Hwhistory)
                    .HasForeignKey(d => d.HwId)
                    .HasConstraintName("HWHISTORY_HW_FK");
            });

            modelBuilder.Entity<Hwkonfig>(entity =>
            {
                entity.ToTable("HWKONFIG");

                entity.HasIndex(e => e.Bezeichnung)
                    .HasName("HWKONFIG_BEZEICHNUNG_IDX");

                entity.HasIndex(e => e.Hersteller)
                    .HasName("HWKONFIG_HERSTELLER_IDX");

                entity.HasIndex(e => e.HwtypId)
                    .HasName("HWKONFIG_HWTYP_ID_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("HWKONFIG_PK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasColumnName("BEZEICHNUNG")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Hd)
                    .HasColumnName("HD")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Hersteller)
                    .IsRequired()
                    .HasColumnName("HERSTELLER")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.HwtypId)
                    .HasColumnName("HWTYP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Prozessor)
                    .HasColumnName("PROZESSOR")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Ram)
                    .HasColumnName("RAM")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.Sonst)
                    .HasColumnName("SONST")
                    .HasColumnType("CLOB");

                entity.Property(e => e.Video)
                    .HasColumnName("VIDEO")
                    .HasColumnType("VARCHAR2(50)");

                entity.HasOne(d => d.Hwtyp)
                    .WithMany(p => p.Hwkonfig)
                    .HasForeignKey(d => d.HwtypId)
                    .HasConstraintName("HWKONFIG_HWTYP_FK");
            });

            modelBuilder.Entity<Hwtyp>(entity =>
            {
                entity.ToTable("HWTYP");

                entity.HasIndex(e => e.AptypId)
                    .HasName("HWTYP_APTYP_ID_IDX");

                entity.HasIndex(e => e.Flag)
                    .HasName("HWTYP_FLAG_IDX");

                entity.HasIndex(e => e.Hwtyp1)
                    .HasName("HWTYP_HWTYP_UN")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("HWTYP_PK")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AptypId)
                    .HasColumnName("APTYP_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Hwtyp1)
                    .IsRequired()
                    .HasColumnName("HWTYP")
                    .HasColumnType("VARCHAR2(50)");

                entity.HasOne(d => d.Aptyp)
                    .WithMany(p => p.Hwtyp)
                    .HasForeignKey(d => d.AptypId)
                    .HasConstraintName("HWTYP_APTYP_FK");
            });

            modelBuilder.Entity<Issuetyp>(entity =>
            {
                entity.ToTable("ISSUETYP");

                entity.HasIndex(e => e.Flag)
                    .HasName("ISSUETYP_FLAG_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("ISSUETYP_PK")
                    .IsUnique();

                entity.HasIndex(e => e.Issuetyp1)
                    .HasName("ISSUETYP_ISSUETYP_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Issuetyp1)
                    .IsRequired()
                    .HasColumnName("ISSUETYP")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.Entity<Mac>(entity =>
            {
                entity.ToTable("MAC");

                entity.HasIndex(e => e.Id)
                    .HasName("MAC_PK")
                    .IsUnique();

                entity.HasIndex(e => e.Mac1)
                    .HasName("MAC_MAC_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.HwId)
                    .HasColumnName("HW_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Mac1)
                    .IsRequired()
                    .HasColumnName("MAC")
                    .HasColumnType("VARCHAR2(12)");

                entity.Property(e => e.VlanVlanId)
                    .HasColumnName("VLAN_VLAN_ID")
                    .HasColumnType("NUMBER(38)");

                entity.HasOne(d => d.Hw)
                    .WithMany(p => p.Mac)
                    .HasForeignKey(d => d.HwId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAC_HW_FK");

                entity.HasOne(d => d.VlanVlan)
                    .WithMany(p => p.Mac)
                    .HasForeignKey(d => d.VlanVlanId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAC_VLAN_FK");
            });

            modelBuilder.Entity<Oe>(entity =>
            {
                entity.ToTable("OE");

                entity.HasIndex(e => e.AdresseId)
                    .HasName("OE_ADRESSE_ID_IDX");

                entity.HasIndex(e => e.Betriebsstelle)
                    .HasName("OE_BETRIEBSSTELLE_IDX");

                entity.HasIndex(e => e.Bst)
                    .HasName("OE_BST_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("OE_PK")
                    .IsUnique();

                entity.HasIndex(e => e.OeId)
                    .HasName("OE_OE_ID_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AdresseId)
                    .HasColumnName("ADRESSE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Ap)
                    .HasColumnName("AP")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Betriebsstelle)
                    .IsRequired()
                    .HasColumnName("BETRIEBSSTELLE")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Bst)
                    .HasColumnName("BST")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Fax)
                    .HasColumnName("FAX")
                    .HasColumnType("VARCHAR2(50)");

                entity.Property(e => e.OeId)
                    .HasColumnName("OE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Oeff)
                    .HasColumnName("OEFF")
                    .HasColumnType("CLOB");

                entity.Property(e => e.Tel)
                    .HasColumnName("TEL")
                    .HasColumnType("VARCHAR2(50)");

                entity.HasOne(d => d.Adresse)
                    .WithMany(p => p.Oe)
                    .HasForeignKey(d => d.AdresseId)
                    .HasConstraintName("OE_ADRESSE_FK");

                entity.HasOne(d => d.OeNavigation)
                    .WithMany(p => p.InverseOeNavigation)
                    .HasForeignKey(d => d.OeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OE_OE_FK");
            });

            modelBuilder.Entity<ProgramSettings>(entity =>
            {
                entity.ToTable("PROGRAM_SETTINGS");

                entity.HasIndex(e => e.Id)
                    .HasName("PROGRAM_SETTINGS_PK")
                    .IsUnique();

                entity.HasIndex(e => e.Key)
                    .HasName("PROGRAM_SETTINGS_KEY_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasColumnName("KEY")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Value)
                    .HasColumnName("VALUE")
                    .HasColumnType("VARCHAR2(2000)");
            });

            modelBuilder.Entity<Tagtyp>(entity =>
            {
                entity.ToTable("TAGTYP");

                entity.HasIndex(e => e.ApklasseId)
                    .HasName("TAGTYP_APTYP_ID_IDX");

                entity.HasIndex(e => e.Id)
                    .HasName("TAGTYP_PK")
                    .IsUnique();

                entity.HasIndex(e => e.TagTyp1)
                    .HasName("TAGTYP_TAG_TYP_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ApklasseId)
                    .HasColumnName("APKLASSE_ID")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Flag)
                    .HasColumnName("FLAG")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Param)
                    .HasColumnName("PARAM")
                    .HasColumnType("VARCHAR2(200)");

                entity.Property(e => e.TagTyp1)
                    .IsRequired()
                    .HasColumnName("TAG_TYP")
                    .HasColumnType("VARCHAR2(50)");

                entity.HasOne(d => d.Apklasse)
                    .WithMany(p => p.Tagtyp)
                    .HasForeignKey(d => d.ApklasseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("TAGTYP_APKLASSE_FK");
            });

            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.ToTable("USER_SETTINGS");

                entity.HasIndex(e => e.Id)
                    .HasName("USER_SETTINGS_PK")
                    .IsUnique();

                entity.HasIndex(e => e.Userid)
                    .HasName("USER_SETTINGS_USERID_IDX");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Settings)
                    .HasColumnName("SETTINGS")
                    .HasColumnType("CLOB");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("USERID")
                    .HasColumnType("VARCHAR2(20)");
            });

            modelBuilder.Entity<Vlan>(entity =>
            {
                entity.ToTable("VLAN");

                entity.HasIndex(e => e.Id)
                    .HasName("VLAN_PK")
                    .IsUnique();

                entity.HasIndex(e => e.Ip)
                    .HasName("VLAN_IP_IDX");

                entity.HasIndex(e => e.Vlan1)
                    .HasName("VLAN_VLAN_UN")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Netmask)
                    .HasColumnName("NETMASK")
                    .HasColumnType("NUMBER(38)");

                entity.Property(e => e.Vlan1)
                    .IsRequired()
                    .HasColumnName("VLAN")
                    .HasColumnType("VARCHAR2(100)");
            });

            modelBuilder.HasSequence("ISEQ$$_78938");

            modelBuilder.HasSequence("ISEQ$$_79168");

            modelBuilder.HasSequence("ISEQ$$_79296");

            modelBuilder.HasSequence("ISEQ$$_84823");

            modelBuilder.HasSequence("ISEQ$$_85881");

            modelBuilder.HasSequence("ISEQ$$_85884");

            modelBuilder.HasSequence("ISEQ$$_85893");

            modelBuilder.HasSequence("ISEQ$$_85904");

            modelBuilder.HasSequence("ISEQ$$_85910");

            modelBuilder.HasSequence("ISEQ$$_85914");

            modelBuilder.HasSequence("ISEQ$$_85918");

            modelBuilder.HasSequence("ISEQ$$_85925");

            modelBuilder.HasSequence("ISEQ$$_85928");

            modelBuilder.HasSequence("ISEQ$$_85937");

            modelBuilder.HasSequence("ISEQ$$_85945");

            modelBuilder.HasSequence("ISEQ$$_85951");

            modelBuilder.HasSequence("ISEQ$$_85956");

            modelBuilder.HasSequence("ISEQ$$_85960");

            modelBuilder.HasSequence("ISEQ$$_85969");

            modelBuilder.HasSequence("ISEQ$$_85973");

            modelBuilder.HasSequence("ISEQ$$_85978");

            modelBuilder.HasSequence("ISEQ$$_85984");

            modelBuilder.HasSequence("ISEQ$$_85993");

            modelBuilder.HasSequence("ISEQ$$_85996");

            modelBuilder.HasSequence("ISEQ$$_86005");

            modelBuilder.HasSequence("ISEQ$$_86016");

            modelBuilder.HasSequence("ISEQ$$_86022");

            modelBuilder.HasSequence("ISEQ$$_86026");

            modelBuilder.HasSequence("ISEQ$$_86030");

            modelBuilder.HasSequence("ISEQ$$_86037");

            modelBuilder.HasSequence("ISEQ$$_86040");

            modelBuilder.HasSequence("ISEQ$$_86049");

            modelBuilder.HasSequence("ISEQ$$_86057");

            modelBuilder.HasSequence("ISEQ$$_86063");

            modelBuilder.HasSequence("ISEQ$$_86068");

            modelBuilder.HasSequence("ISEQ$$_86072");

            modelBuilder.HasSequence("ISEQ$$_86081");

            modelBuilder.HasSequence("ISEQ$$_86085");

            modelBuilder.HasSequence("ISEQ$$_86090");

            modelBuilder.HasSequence("ISEQ$$_86096");

            modelBuilder.HasSequence("ISEQ$$_86132");

            modelBuilder.HasSequence("ISEQ$$_86138");

            modelBuilder.HasSequence("ISEQ$$_86141");

            modelBuilder.HasSequence("ISEQ$$_86150");

            modelBuilder.HasSequence("ISEQ$$_86161");

            modelBuilder.HasSequence("ISEQ$$_86167");

            modelBuilder.HasSequence("ISEQ$$_86171");

            modelBuilder.HasSequence("ISEQ$$_86175");

            modelBuilder.HasSequence("ISEQ$$_86182");

            modelBuilder.HasSequence("ISEQ$$_86185");

            modelBuilder.HasSequence("ISEQ$$_86194");

            modelBuilder.HasSequence("ISEQ$$_86201");

            modelBuilder.HasSequence("ISEQ$$_86209");

            modelBuilder.HasSequence("ISEQ$$_86215");

            modelBuilder.HasSequence("ISEQ$$_86220");

            modelBuilder.HasSequence("ISEQ$$_86224");

            modelBuilder.HasSequence("ISEQ$$_86233");

            modelBuilder.HasSequence("ISEQ$$_86237");

            modelBuilder.HasSequence("ISEQ$$_86242");

            modelBuilder.HasSequence("ISEQ$$_86248");

            modelBuilder.HasSequence("ISEQ$$_86423");

            modelBuilder.HasSequence("ISEQ$$_86426");

            modelBuilder.HasSequence("ISEQ$$_86435");

            modelBuilder.HasSequence("ISEQ$$_86446");

            modelBuilder.HasSequence("ISEQ$$_86452");

            modelBuilder.HasSequence("ISEQ$$_86456");

            modelBuilder.HasSequence("ISEQ$$_86460");

            modelBuilder.HasSequence("ISEQ$$_86467");

            modelBuilder.HasSequence("ISEQ$$_86470");

            modelBuilder.HasSequence("ISEQ$$_86479");

            modelBuilder.HasSequence("ISEQ$$_86486");

            modelBuilder.HasSequence("ISEQ$$_86494");

            modelBuilder.HasSequence("ISEQ$$_86500");

            modelBuilder.HasSequence("ISEQ$$_86505");

            modelBuilder.HasSequence("ISEQ$$_86509");

            modelBuilder.HasSequence("ISEQ$$_86518");

            modelBuilder.HasSequence("ISEQ$$_86522");

            modelBuilder.HasSequence("ISEQ$$_86527");

            modelBuilder.HasSequence("ISEQ$$_86533");
        }
    }
}

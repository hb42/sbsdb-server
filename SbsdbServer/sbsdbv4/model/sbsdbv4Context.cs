﻿using Microsoft.EntityFrameworkCore;

namespace hb.SbsdbServer.sbsdbv4.model {
    public class Sbsdbv4Context : DbContext {
        public Sbsdbv4Context(DbContextOptions<Sbsdbv4Context> options)
            : base(options) {
        }

        public virtual DbSet<SbsAdrtyp> SbsAdrtyp { get; set; }
        public virtual DbSet<SbsAp> SbsAp { get; set; }
        public virtual DbSet<SbsApAdr> SbsApAdr { get; set; }
        public virtual DbSet<SbsApSw> SbsApSw { get; set; }
        public virtual DbSet<SbsApklasse> SbsApklasse { get; set; }
        public virtual DbSet<SbsApstatistik> SbsApstatistik { get; set; }
        public virtual DbSet<SbsAptyp> SbsAptyp { get; set; }
        public virtual DbSet<SbsAussond> SbsAussond { get; set; }
        public virtual DbSet<SbsCron> SbsCron { get; set; }
        public virtual DbSet<SbsExtprog> SbsExtprog { get; set; }
        public virtual DbSet<SbsFiliale> SbsFiliale { get; set; }
        public virtual DbSet<SbsHw> SbsHw { get; set; }
        public virtual DbSet<SbsHwshift> SbsHwshift { get; set; }
        public virtual DbSet<SbsHwtyp> SbsHwtyp { get; set; }
        public virtual DbSet<SbsKonfig> SbsKonfig { get; set; }
        public virtual DbSet<SbsLiztyp> SbsLiztyp { get; set; }
        public virtual DbSet<SbsOe> SbsOe { get; set; }
        public virtual DbSet<SbsPrefs> SbsPrefs { get; set; }
        public virtual DbSet<SbsSegment> SbsSegment { get; set; }
        public virtual DbSet<SbsSw> SbsSw { get; set; }
        public virtual DbSet<SbsSwBestand> SbsSwBestand { get; set; }
        public virtual DbSet<SbsSwOpdv> SbsSwOpdv { get; set; }
        public virtual DbSet<SbsSwVv> SbsSwVv { get; set; }
        public virtual DbSet<SbsTtIssue> SbsTtIssue { get; set; }
        public virtual DbSet<SbsTtKategorie> SbsTtKategorie { get; set; }
        public virtual DbSet<SbsUser> SbsUser { get; set; }
        public virtual DbSet<SbsViews> SbsViews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<SbsAdrtyp>(entity => {
                entity.HasKey(e => e.AdrIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AdrTyp)
                    .HasName("sbsadrtyp_index1");

                entity.HasIndex(e => e.AptypIndex)
                    .HasName("FK2FB5A0379E420B01");

                entity.HasOne(d => d.AptypIndexNavigation)
                    .WithMany(p => p.SbsAdrtyp)
                    .HasForeignKey(d => d.AptypIndex)
                    .HasConstraintName("FK2FB5A0379E420B01");
            });

            modelBuilder.Entity<SbsAp>(entity => {
                entity.HasKey(e => e.ApIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ApName)
                    .HasName("sbsap_index1");

                entity.HasIndex(e => e.ApklasseIndex)
                    .HasName("FK916B726A31F69485");

                entity.HasIndex(e => e.ApstatistikIndex)
                    .HasName("FK916B726A1B5248DB");

                entity.HasIndex(e => e.OeIndex)
                    .HasName("FK916B726AE6978689");

                entity.HasIndex(e => e.SegmentIndex)
                    .HasName("FK916B726ABF666AF");

                entity.HasIndex(e => e.StandortIndex)
                    .HasName("FK916B726A5CA9D44E");

                entity.HasIndex(e => e.Tcp)
                    .HasName("sbsap_index2");

                entity.HasOne(d => d.ApklasseIndexNavigation)
                    .WithMany(p => p.SbsAp)
                    .HasForeignKey(d => d.ApklasseIndex)
                    .HasConstraintName("FK916B726A31F69485");

                entity.HasOne(d => d.ApstatistikIndexNavigation)
                    .WithMany(p => p.SbsAp)
                    .HasForeignKey(d => d.ApstatistikIndex)
                    .HasConstraintName("FK916B726A1B5248DB");

                entity.HasOne(d => d.OeIndexNavigation)
                    .WithMany(p => p.SbsApOeIndexNavigation)
                    .HasForeignKey(d => d.OeIndex)
                    .HasConstraintName("FK916B726AE6978689");

                entity.HasOne(d => d.SegmentIndexNavigation)
                    .WithMany(p => p.SbsAp)
                    .HasForeignKey(d => d.SegmentIndex)
                    .HasConstraintName("FK916B726ABF666AF");

                entity.HasOne(d => d.StandortIndexNavigation)
                    .WithMany(p => p.SbsApStandortIndexNavigation)
                    .HasForeignKey(d => d.StandortIndex)
                    .HasConstraintName("FK916B726A5CA9D44E");
            });

            modelBuilder.Entity<SbsApAdr>(entity => {
                entity.HasKey(e => e.ApadrIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AdrIndex)
                    .HasName("FK3064593A807B9F48");

                entity.HasIndex(e => e.AdrText)
                    .HasName("sbsapadr_index1");

                entity.HasIndex(e => e.ApIndex)
                    .HasName("FK3064593A7E2B9C7B");

                entity.HasOne(d => d.AdrIndexNavigation)
                    .WithMany(p => p.SbsApAdr)
                    .HasForeignKey(d => d.AdrIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK3064593A807B9F48");

                entity.HasOne(d => d.ApIndexNavigation)
                    .WithMany(p => p.SbsApAdr)
                    .HasForeignKey(d => d.ApIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK3064593A7E2B9C7B");
            });

            modelBuilder.Entity<SbsApSw>(entity => {
                entity.HasKey(e => e.ApswIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ApIndex)
                    .HasName("FKA6B8EC597E2B9C7B");

                entity.HasIndex(e => e.SwIndex)
                    .HasName("FKA6B8EC593E4C6525");

                entity.HasOne(d => d.ApIndexNavigation)
                    .WithMany(p => p.SbsApSw)
                    .HasForeignKey(d => d.ApIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKA6B8EC597E2B9C7B");

                entity.HasOne(d => d.SwIndexNavigation)
                    .WithMany(p => p.SbsApSw)
                    .HasForeignKey(d => d.SwIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKA6B8EC593E4C6525");
            });

            modelBuilder.Entity<SbsApklasse>(entity => {
                entity.HasKey(e => e.ApklasseIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AptypIndex)
                    .HasName("FK872BB9CF9E420B01");

                entity.HasOne(d => d.AptypIndexNavigation)
                    .WithMany(p => p.SbsApklasse)
                    .HasForeignKey(d => d.AptypIndex)
                    .HasConstraintName("FK872BB9CF9E420B01");
            });

            modelBuilder.Entity<SbsApstatistik>(entity => {
                entity.HasKey(e => e.ApstatistikIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AptypIndex)
                    .HasName("FKDBC82E0E9E420B01");

                entity.HasIndex(e => e.Flag)
                    .HasName("sbsapstatistik_index2");

                entity.HasIndex(e => e.Sort)
                    .HasName("sbsapstatistik_index1");

                entity.HasOne(d => d.AptypIndexNavigation)
                    .WithMany(p => p.SbsApstatistik)
                    .HasForeignKey(d => d.AptypIndex)
                    .HasConstraintName("FKDBC82E0E9E420B01");
            });

            modelBuilder.Entity<SbsAptyp>(entity => {
                entity.HasKey(e => e.AptypIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Aptyp)
                    .HasName("APTYP")
                    .IsUnique();

                entity.HasIndex(e => e.LfdNr)
                    .HasName("sbsaptyp_index1");
            });

            modelBuilder.Entity<SbsAussond>(entity => {
                entity.HasKey(e => e.HwIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AussDat)
                    .HasName("sbsaussond_index2");

                entity.HasIndex(e => e.SerNr)
                    .HasName("sbsaussond_index1");
            });

            modelBuilder.Entity<SbsCron>(entity => {
                entity.HasKey(e => e.CronIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Cron)
                    .HasName("sbscron_index1");
            });

            modelBuilder.Entity<SbsExtprog>(entity => {
                entity.HasKey(e => e.ExtprogIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ApklasseIndex)
                    .HasName("FKBCD2838031F69485");

                entity.HasOne(d => d.ApklasseIndexNavigation)
                    .WithMany(p => p.SbsExtprog)
                    .HasForeignKey(d => d.ApklasseIndex)
                    .HasConstraintName("FKBCD2838031F69485");
            });

            modelBuilder.Entity<SbsFiliale>(entity => {
                entity.HasKey(e => e.FilialeIndex)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<SbsHw>(entity => {
                entity.HasKey(e => e.HwIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ApIndex)
                    .HasName("FK916B734A7E2B9C7B");

                entity.HasIndex(e => e.KonfigIndex)
                    .HasName("FK916B734AF6F4CB51");

                entity.HasIndex(e => e.SerNr)
                    .HasName("sbshw_index1");

                entity.HasOne(d => d.ApIndexNavigation)
                    .WithMany(p => p.SbsHw)
                    .HasForeignKey(d => d.ApIndex)
                    .HasConstraintName("FK916B734A7E2B9C7B");

                entity.HasOne(d => d.KonfigIndexNavigation)
                    .WithMany(p => p.SbsHw)
                    .HasForeignKey(d => d.KonfigIndex)
                    .HasConstraintName("FK916B734AF6F4CB51");
            });

            modelBuilder.Entity<SbsHwshift>(entity => {
                entity.HasKey(e => e.HwshiftIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Direction)
                    .HasName("sbshwshift_index3");

                entity.HasIndex(e => e.Host)
                    .HasName("sbshwshift_index2");

                entity.HasIndex(e => e.HwIndex)
                    .HasName("FK59BE7F58C79F763B");

                entity.HasIndex(e => e.Shiftdate)
                    .HasName("sbshwshift_index1");

                entity.Property(e => e.Shiftdate)
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                    .ValueGeneratedOnAddOrUpdate();

                entity.HasOne(d => d.HwIndexNavigation)
                    .WithMany(p => p.SbsHwshift)
                    .HasForeignKey(d => d.HwIndex)
                    .HasConstraintName("FK59BE7F58C79F763B");
            });

            modelBuilder.Entity<SbsHwtyp>(entity => {
                entity.HasKey(e => e.HwtypIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.AptypIndex)
                    .HasName("FKA71E96E19E420B01");

                entity.HasOne(d => d.AptypIndexNavigation)
                    .WithMany(p => p.SbsHwtyp)
                    .HasForeignKey(d => d.AptypIndex)
                    .HasConstraintName("FKA71E96E19E420B01");
            });

            modelBuilder.Entity<SbsKonfig>(entity => {
                entity.HasKey(e => e.KonfigIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Bezeichnung)
                    .HasName("sbskonfig_index2");

                entity.HasIndex(e => e.Hersteller)
                    .HasName("sbskonfig_index1");

                entity.HasIndex(e => e.HwtypIndex)
                    .HasName("FK415F10F55EEAD941");

                entity.HasOne(d => d.HwtypIndexNavigation)
                    .WithMany(p => p.SbsKonfig)
                    .HasForeignKey(d => d.HwtypIndex)
                    .HasConstraintName("FK415F10F55EEAD941");
            });

            modelBuilder.Entity<SbsLiztyp>(entity => {
                entity.HasKey(e => e.LiztypIndex)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<SbsOe>(entity => {
                entity.HasKey(e => e.OeIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Betriebsstelle)
                    .HasName("sbsoe_index2");

                entity.HasIndex(e => e.Bst)
                    .HasName("sbsoe_index1");

                entity.HasIndex(e => e.FilialeIndex)
                    .HasName("FK916B7411427CC73D");

                entity.HasIndex(e => e.ParentOe)
                    .HasName("FK916B7411D06ED6EB");

                entity.HasOne(d => d.FilialeIndexNavigation)
                    .WithMany(p => p.SbsOe)
                    .HasForeignKey(d => d.FilialeIndex)
                    .HasConstraintName("FK916B7411427CC73D");

                entity.HasOne(d => d.ParentOeNavigation)
                    .WithMany(p => p.InverseParentOeNavigation)
                    .HasForeignKey(d => d.ParentOe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK916B7411D06ED6EB");
            });

            modelBuilder.Entity<SbsPrefs>(entity => {
                entity.HasKey(e => e.PrefsIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Preference)
                    .HasName("sbsprefs_index1");

                entity.HasIndex(e => e.UserIndex)
                    .HasName("FKA78CD275A65A7C33");

                entity.HasOne(d => d.UserIndexNavigation)
                    .WithMany(p => p.SbsPrefs)
                    .HasForeignKey(d => d.UserIndex)
                    .HasConstraintName("FKA78CD275A65A7C33");
            });

            modelBuilder.Entity<SbsSegment>(entity => {
                entity.HasKey(e => e.SegmentIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.FilialeIndex)
                    .HasName("FK8044EDB8427CC73D");

                //entity.HasOne(d => d.FilialeIndexNavigation)
                //.WithMany(p => p.SbsSegment)
                //.HasForeignKey(d => d.FilialeIndex)
                //.HasConstraintName("FK8044EDB8427CC73D");
            });

            modelBuilder.Entity<SbsSw>(entity => {
                entity.HasKey(e => e.SwIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Bezeichnung)
                    .HasName("sbssw_index2");

                entity.HasIndex(e => e.Einsatz)
                    .HasName("sbssw_index4");

                entity.HasIndex(e => e.Hersteller)
                    .HasName("sbssw_index1");

                entity.HasIndex(e => e.LiztypIndex)
                    .HasName("FK916B749F6C0A7979");

                entity.HasIndex(e => e.OeIndex)
                    .HasName("FK916B749FE6978689");

                entity.HasIndex(e => e.SmsPaket)
                    .HasName("sbssw_index3");

                entity.HasIndex(e => e.VollzErkl)
                    .HasName("sbssw_index5");

                entity.HasOne(d => d.LiztypIndexNavigation)
                    .WithMany(p => p.SbsSw)
                    .HasForeignKey(d => d.LiztypIndex)
                    .HasConstraintName("FK916B749F6C0A7979");

                entity.HasOne(d => d.OeIndexNavigation)
                    .WithMany(p => p.SbsSw)
                    .HasForeignKey(d => d.OeIndex)
                    .HasConstraintName("FK916B749FE6978689");
            });

            modelBuilder.Entity<SbsSwBestand>(entity => {
                entity.HasKey(e => e.SwbestandIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.SwIndex)
                    .HasName("FKB99A81B33E4C6525");

                entity.HasOne(d => d.SwIndexNavigation)
                    .WithMany(p => p.SbsSwBestand)
                    .HasForeignKey(d => d.SwIndex)
                    .HasConstraintName("FKB99A81B33E4C6525");
            });

            modelBuilder.Entity<SbsSwOpdv>(entity => {
                entity.HasKey(e => e.SwopdvIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Datum)
                    .HasName("sbsswopdv_index1");

                entity.HasIndex(e => e.SwIndex)
                    .HasName("FKA04F6DD33E4C6525");

                entity.HasOne(d => d.SwIndexNavigation)
                    .WithMany(p => p.SbsSwOpdv)
                    .HasForeignKey(d => d.SwIndex)
                    .HasConstraintName("FKA04F6DD33E4C6525");
            });

            modelBuilder.Entity<SbsSwVv>(entity => {
                entity.HasKey(e => e.SwvvIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Datum)
                    .HasName("sbsswvv_index1");

                entity.HasIndex(e => e.SwIndex)
                    .HasName("FKA7B9C2603E4C6525");

                entity.HasOne(d => d.SwIndexNavigation)
                    .WithMany(p => p.SbsSwVv)
                    .HasForeignKey(d => d.SwIndex)
                    .HasConstraintName("FKA7B9C2603E4C6525");
            });

            modelBuilder.Entity<SbsTtIssue>(entity => {
                entity.HasKey(e => e.TtissueIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.ApIndex)
                    .HasName("FK3279D0357E2B9C7B");

                entity.HasIndex(e => e.Close)
                    .HasName("sbsttissue_index3");

                entity.HasIndex(e => e.KategorieIndex)
                    .HasName("FK3279D035F95D3827");

                entity.HasIndex(e => e.Open)
                    .HasName("sbsttissue_index1");

                entity.HasIndex(e => e.Prio)
                    .HasName("sbsttissue_index2");

                entity.HasIndex(e => e.UserIndex)
                    .HasName("FK3279D035A65A7C33");

                entity.HasOne(d => d.ApIndexNavigation)
                    .WithMany(p => p.SbsTtIssue)
                    .HasForeignKey(d => d.ApIndex)
                    .HasConstraintName("FK3279D0357E2B9C7B");

                entity.HasOne(d => d.KategorieIndexNavigation)
                    .WithMany(p => p.SbsTtIssue)
                    .HasForeignKey(d => d.KategorieIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK3279D035F95D3827");

                entity.HasOne(d => d.UserIndexNavigation)
                    .WithMany(p => p.SbsTtIssue)
                    .HasForeignKey(d => d.UserIndex)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK3279D035A65A7C33");
            });

            modelBuilder.Entity<SbsTtKategorie>(entity => {
                entity.HasKey(e => e.KategorieIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Flag)
                    .HasName("sbsttkategorie_index2");

                entity.HasIndex(e => e.Kategorie)
                    .HasName("sbsttkategorie_index1");
            });

            modelBuilder.Entity<SbsUser>(entity => {
                entity.HasKey(e => e.UserIndex)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.Rolle)
                    .HasName("sbsuser_index1");

                entity.HasIndex(e => e.UserId)
                    .HasName("USER_ID")
                    .IsUnique();
            });

            modelBuilder.Entity<SbsViews>(entity => {
                entity.HasKey(e => e.ViewIndex)
                    .HasName("PRIMARY");
            });
        }
    }
}

﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MKB.Models;
using MKB_API.Models;
using WebApplication1.Models;

namespace MKB.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<KbBaranjaWeb> KbBaranjaWeb { get; set; }

    public virtual DbSet<KbIzvestaiWeb> KbIzvestaiWeb { get; set; }

    public virtual DbSet<KbNacinPlakanje> KbNacinPlakanje { get; set; }

    public virtual DbSet<KbStatusBaranjeWeb> KbStatusBaranjeWeb { get; set; }

    public virtual DbSet<KbWebKorisnikAktivnost> KbWebKorisnikAktivnosti { get; set; }

    public virtual DbSet<KbWebLogPrebaraniKompanii> KbWebLogPrebaraniKompanii { get; set; }

    public virtual DbSet<KbWebModuli> KbWebModuli { get; set; }

    public virtual DbSet<KbWebPaketiM> KbWebPaketiM { get; set; }

    public virtual DbSet<KbWebPravniLica> KbWebPravniLica { get; set; }

    public virtual DbSet<KbWebTipPrebaruvanje> KbWebTipPrebaruvanja { get; set; }

    public virtual DbSet<KbWebTipUsluga> KbWebTipUslugi { get; set; }

    public virtual DbSet<KbWebLogKompaniiSporedba> KbWebLogKompaniiSporedbi { get; set; }

    public virtual DbSet<KbWebKorisnikPaket> KbWebKorisnikPaketi { get; set; }

    public virtual DbSet<KbWebLogFilterKriterium> KbWebLogFilterKriteriumi { get; set; }

    public virtual DbSet<KbWebLogIskoristeniPromoKodovi> KbWebLogIskoristeniPromoKodovi { get; set; }

    public virtual DbSet<KbWebPromoKodovi> KbWebPromoKodovi { get; set; }

    public virtual DbSet<KbStatusPretplataWeb> KbStatusPretplataWeb { get; set; }

    public virtual DbSet<KbVidKorIzv> KbVidKorIzvestai { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
         => optionsBuilder.UseSqlServer(
             "Server=DESKTOP-JTIRNBH\\SQLEXPRESS;Database=MKB;Trusted_Connection=True;TrustServerCertificate=True"
             //"Server=DESKTOP-MCSAB02\\SQLEXPRESS;Database=MKB;Trusted_Connection=True;TrustServerCertificate=True"
             );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.UserWebId, "AK_AspNetUsers_UserWebID").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.DateIns)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DateUpd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Embg)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("EMBG");
            entity.Property(e => e.JobTitle).HasMaxLength(50);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LegalEntityId).HasColumnName("LegalEntityID");
            entity.Property(e => e.Mkbadmin).HasColumnName("MKBAdmin");
            entity.Property(e => e.MkbnewsInd).HasColumnName("MKBNewsInd");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.PaketId).HasColumnName("PaketID");
            entity.Property(e => e.PasswordUpd)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TimeIns)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TimeUpd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.UserWebId)
                .IsRequired()
                .HasColumnName("UserWebID");

            entity.HasOne(d => d.LegalEntity).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.LegalEntityId)
                .HasConstraintName("FK_AspNetUsers_KB_WebPravniLica");
        });

        modelBuilder.Entity<KbBaranjaWeb>(entity =>
        {
            entity.HasKey(e => e.BaranjeWebId);

            entity.ToTable("KB_BaranjaWeb");

            entity.Property(e => e.BaranjeWebId)
                .ValueGeneratedNever()
                .HasColumnName("BaranjeWebID");
            entity.Property(e => e.DatumBaranje)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Iznos).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.IzvestajWebId).HasColumnName("IzvestajWebID");
            entity.Property(e => e.KorisnikWebId).HasColumnName("KorisnikWebID");
            entity.Property(e => e.PretplataId).HasColumnName("PretplataID");
            entity.Property(e => e.VremeBaranje)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IzvestajWeb).WithMany(p => p.KbBaranjaWebs)
                .HasForeignKey(d => d.IzvestajWebId)
                .HasConstraintName("FK_KB_BaranjaWeb_KB_IzvestaiWeb");

            entity.HasOne(d => d.NacinPlakanjeNavigation).WithMany(p => p.KbBaranjaWebs)
                .HasForeignKey(d => d.NacinPlakanje)
                .HasConstraintName("FK_KB_BaranjaWeb_KB_TipBaranjeWeb");

            entity.HasOne(d => d.StatusBaranjeWebNavigation).WithMany(p => p.KbBaranjaWebs)
                .HasForeignKey(d => d.StatusBaranjeWeb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KB_BaranjaWeb_KB_StatusBaranjeWeb");
        });

        modelBuilder.Entity<KbIzvestaiWeb>(entity =>
        {
            entity.HasKey(e => e.IzvestajWebId).IsClustered(false);

            entity.ToTable("KB_IzvestaiWeb");

            entity.Property(e => e.IzvestajWebId)
                .ValueGeneratedNever()
                .HasColumnName("IzvestajWebID");
            entity.Property(e => e.DatumIzv)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IzvestajXml)
                .HasColumnType("xml")
                .HasColumnName("IzvestajXML");
            entity.Property(e => e.KorisnikWebId).HasColumnName("KorisnikWebID");
            entity.Property(e => e.LegalEntityId).HasColumnName("LegalEntityID");
            entity.Property(e => e.VremeIzv)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<KbNacinPlakanje>(entity =>
        {
            entity.HasKey(e => e.NacinPlakanje);

            entity.ToTable("KB_NacinPlakanje");
        });

        modelBuilder.Entity<KbStatusBaranjeWeb>(entity =>
        {
            entity.HasKey(e => e.StatusBaranjeWeb);

            entity.ToTable("KB_StatusBaranjeWeb");

            entity.Property(e => e.OpisStatusBaranjeWeb).HasMaxLength(64);
        });

        modelBuilder.Entity<KbStatusPretplataWeb>(entity =>
        {
            entity.HasKey(e => e.StatusPretplata);

            entity.ToTable("KB_StatusPretplataWeb");

            entity.Property(e => e.OpisStatusPretplata).HasMaxLength(64);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
        });

        modelBuilder.Entity<KbVidKorIzv>(entity =>
        {
            entity.HasKey(e => e.VidKorIzv);

            entity.ToTable("KB_VidKorIzv");

            entity.Property(e => e.IndAcdizv).HasColumnName("IndACDIzv");
            entity.Property(e => e.IndPrikazMkbadmin).HasColumnName("IndPrikazMKBAdmin");
            entity.Property(e => e.OpisVidKorIzv).HasMaxLength(50);
        });

        modelBuilder.Entity<KbWebKorisnikAktivnost>(entity =>
        {
            entity.ToTable("KB_WebKorisnikAktivnost");

            entity.Property(e => e.BaranjeWebId).HasColumnName("BaranjeWebID");
            entity.Property(e => e.Cena).HasColumnType("numeric(9, 2)");
            entity.Property(e => e.CenaDopMonitoringSubjekti).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.DatumVnes).HasColumnType("datetime");
            entity.Property(e => e.KorisnikWebId).HasColumnName("KorisnikWebID");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.UserName)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.HasOne(d => d.BaranjeWeb).WithMany(p => p.KbWebKorisnikAktivnosts)
                .HasForeignKey(d => d.BaranjeWebId)
                .HasConstraintName("FK_KB_WebKorisnikAktivnost_KB_BaranjaWeb");

            entity.HasOne(d => d.KorisnikWeb).WithMany(p => p.KbWebKorisnikAktivnosts)
                .HasPrincipalKey(p => p.UserWebId)
                .HasForeignKey(d => d.KorisnikWebId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KB_WebKorisnikAktivnost_AspNetUsers");

            entity.HasOne(d => d.NacinPlakanjeNavigation).WithMany(p => p.KbWebKorisnikAktivnosts)
                .HasForeignKey(d => d.NacinPlakanje)
                .HasConstraintName("FK_KB_WebKorisnikAktivnost_KB_NacinPlakanje");

            entity.HasOne(d => d.StatusPretplataNavigation).WithMany(p => p.KbWebKorisnikAktivnosts)
                .HasForeignKey(d => d.StatusPretplata)
                .HasConstraintName("FK_KB_WebKorisnikAktivnost_KB_StatusPretplataWeb");
        });

        modelBuilder.Entity<KbWebKorisnikPaket>(entity =>
        {
            entity.HasKey(e => e.KorisnikWebId);

            entity.ToTable("KB_WebKorisnikPaket");

            entity.Property(e => e.KorisnikWebId)
                .ValueGeneratedNever()
                .HasColumnName("KorisnikWebID");
            entity.Property(e => e.BrDopSubjektiMonitoring).HasDefaultValue((short)0);
            entity.Property(e => e.LegalEntityId).HasColumnName("LegalEntityID");
            entity.Property(e => e.UserName)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.HasOne(d => d.KorisnikWeb).WithOne(p => p.KbWebKorisnikPaket)
                .HasPrincipalKey<AspNetUser>(p => p.UserWebId)
                .HasForeignKey<KbWebKorisnikPaket>(d => d.KorisnikWebId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KB_WebKorisnikPaket_AspNetUsers");

            entity.HasOne(d => d.LegalEntity).WithMany(p => p.KbWebKorisnikPakets)
                .HasForeignKey(d => d.LegalEntityId)
                .HasConstraintName("FK_KB_WebKorisnikPaket_KB_WebPravniLica");

            entity.HasOne(d => d.Paket).WithMany(p => p.KbWebKorisnikPakets)
                .HasForeignKey(d => d.PaketId)
                .HasConstraintName("FK_KB_WebKorisnikPaket_KB_WebPaketiM");
        });

        modelBuilder.Entity<KbWebLogFilterKriterium>(entity =>
        {
            entity.ToTable("KB_WebLogFilterKriterium");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BrVrab).IsUnicode(false);
            entity.Property(e => e.Datum)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GodinaOsnovanje).IsUnicode(false);
            entity.Property(e => e.Izlozenost).IsUnicode(false);
            entity.Property(e => e.Konkurencija).IsUnicode(false);
            entity.Property(e => e.Prihod).IsUnicode(false);
            entity.Property(e => e.Profit).IsUnicode(false);
            entity.Property(e => e.Sediste).IsUnicode(false);
            entity.Property(e => e.Sektor).IsUnicode(false);
            entity.Property(e => e.TipKompanija).IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Vreme)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<KbWebLogIskoristeniPromoKodovi>(entity =>
        {
            entity.ToTable("KB_WebLogIskoristeniPromoKodovi");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AktivnostId).HasColumnName("AktivnostID");
            entity.Property(e => e.Embs)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("EMBS");
            entity.Property(e => e.KorisnikWebId).HasColumnName("KorisnikWebID");
            entity.Property(e => e.LegalEntityId).HasColumnName("LegalEntityID");
            entity.Property(e => e.PromoKod).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(32);
        });

        modelBuilder.Entity<KbWebLogKompaniiSporedba>(entity =>
        {
            entity.ToTable("KB_WebLogKompaniiSporedba");

            entity.Property(e => e.AktivnostId)
                .HasMaxLength(50)
                .HasColumnName("AktivnostID");
            entity.Property(e => e.Datum)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Edb)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EDB");
            entity.Property(e => e.Embs)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EMBS");
            entity.Property(e => e.KorisnikWebId).HasColumnName("KorisnikWebID");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.Vreme)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<KbWebLogPrebaraniKompanii>(entity =>
        {
            entity.ToTable("KB_WebLogPrebaraniKompanii");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AktivnostId).HasColumnName("AktivnostID");
            entity.Property(e => e.Datum)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Edb)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("EDB");
            entity.Property(e => e.Embs)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("EMBS");
            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Vreme)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<KbWebModuli>(entity =>
        {
            entity.HasKey(e => new { e.ModulId, e.PodModulId });

            entity.ToTable("KB_WebModuli");

            entity.Property(e => e.Cena)
                .HasDefaultValue(0.00m)
                .HasColumnType("numeric(9, 2)");
            entity.Property(e => e.CenaFl)
                .HasColumnType("numeric(9, 2)")
                .HasColumnName("CenaFL");
            entity.Property(e => e.DatumVnes).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idpovrzanost).HasColumnName("IDPovrzanost");
            entity.Property(e => e.NazivModul)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.NazivPodModul)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.UserName)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.HasOne(d => d.TipUslugaNavigation).WithMany(p => p.KbWebModulis)
                .HasForeignKey(d => d.TipUsluga)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KB_WebModuli_KB_WebTipUsluga");
        });

        modelBuilder.Entity<KbWebPaketiM>(entity =>
        {
            entity.HasKey(e => e.PaketId).HasName("PK__KB_WebPa__2F326F99E12451F2");

            entity.ToTable("KB_WebPaketiM");

            entity.Property(e => e.PaketId).ValueGeneratedNever();
            entity.Property(e => e.CenaDopKorisnik).HasColumnType("numeric(9, 2)");
            entity.Property(e => e.CenaPaket).HasColumnType("numeric(9, 2)");
            entity.Property(e => e.CenaPoen).HasColumnType("numeric(9, 2)");
            entity.Property(e => e.CenaSubjektMonitoring).HasColumnType("numeric(9, 2)");
            entity.Property(e => e.DatumVnes).HasColumnType("datetime");
            entity.Property(e => e.NazivPaket)
                .HasMaxLength(64)
                .IsFixedLength();
            entity.Property(e => e.OpisNazivPaket).HasMaxLength(64);
            entity.Property(e => e.UserName)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<KbWebPravniLica>(entity =>
        {
            entity.HasKey(e => e.LegalEntityId).HasName("PK__KB_WebPr__5266B18248CAD9E6");

            entity.ToTable("KB_WebPravniLica");

            entity.Property(e => e.City).HasMaxLength(30);
            entity.Property(e => e.CompanyAddress).HasMaxLength(128);
            entity.Property(e => e.ContractDate)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ContractPath).IsUnicode(false);
            entity.Property(e => e.ContractTime)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DateIns)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DateUpd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.DogovorBrKorisnik)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DogovorBrMkb)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DogovorBrMKB");
            entity.Property(e => e.Edb)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("EDB");
            entity.Property(e => e.Embs)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("EMBS");
            entity.Property(e => e.TimeIns)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TimeUpd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<KbWebPromoKodovi>(entity =>
        {
            entity.HasKey(e => e.PromoKod);

            entity.ToTable("KB_WebPromoKodovi");

            entity.Property(e => e.PromoKod).HasMaxLength(20);
            entity.Property(e => e.Cena).HasColumnType("numeric(9, 2)");
            entity.Property(e => e.DatumKrajVaznost).HasColumnType("datetime");
            entity.Property(e => e.DatumPocVaznost).HasColumnType("datetime");
            entity.Property(e => e.DatumVnes).HasColumnType("datetime");
            entity.Property(e => e.ImePromocija).HasMaxLength(64);
            entity.Property(e => e.ProcentPopust).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PrviNkompanii).HasColumnName("PrviNKompanii");
            entity.Property(e => e.UserName)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<KbWebTipPrebaruvanje>(entity =>
        {
            entity.HasKey(e => e.TipPrebaruvanje);

            entity.ToTable("KB_WebTipPrebaruvanje");

            entity.Property(e => e.OpisPrebaruvanje)
                .HasMaxLength(32)
                .IsFixedLength();
        });

        modelBuilder.Entity<KbWebTipUsluga>(entity =>
        {
            entity.HasKey(e => e.TipUsluga).HasName("PK__KB_WebTi__1589B91DF58B1249");

            entity.ToTable("KB_WebTipUsluga");

            entity.Property(e => e.TipUsluga).ValueGeneratedNever();
            entity.Property(e => e.OpisGrupaUsluga).HasMaxLength(20);
            entity.Property(e => e.OpisTipUsluga).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebServer.Models;

public partial class Team157LibraryContext : DbContext
{
    public Team157LibraryContext()
    {
    }

    public Team157LibraryContext(DbContextOptions<Team157LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CheckedOut> CheckedOut { get; set; }

    public virtual DbSet<Inventory> Inventory { get; set; }

    public virtual DbSet<Patrons> Patrons { get; set; }

    public virtual DbSet<Phones> Phones { get; set; }

    public virtual DbSet<Titles> Titles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=atr.eng.utah.edu;user id=u1429425;password=pwd;database=Team157Library", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.11.8-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<CheckedOut>(entity =>
        {
            entity.HasKey(e => e.Serial).HasName("PRIMARY");

            entity.HasIndex(e => e.CardNum, "CardNum");

            entity.Property(e => e.Serial)
                .ValueGeneratedNever()
                .HasColumnType("int(10) unsigned");
            entity.Property(e => e.CardNum).HasColumnType("int(10) unsigned");

            entity.HasOne(d => d.CardNumNavigation).WithMany(p => p.CheckedOut)
                .HasForeignKey(d => d.CardNum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CheckedOut_ibfk_1");

            entity.HasOne(d => d.SerialNavigation).WithOne(p => p.CheckedOut)
                .HasForeignKey<CheckedOut>(d => d.Serial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CheckedOut_ibfk_2");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Serial).HasName("PRIMARY");

            entity.HasIndex(e => e.Isbn, "ISBN");

            entity.Property(e => e.Serial).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Isbn)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.Inventory)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Inventory_ibfk_1");
        });

        modelBuilder.Entity<Patrons>(entity =>
        {
            entity.HasKey(e => e.CardNum).HasName("PRIMARY");

            entity.Property(e => e.CardNum).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Phones>(entity =>
        {
            entity.HasKey(e => new { e.CardNum, e.Phone })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.Property(e => e.CardNum).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Phone)
                .HasMaxLength(8)
                .IsFixedLength();

            entity.HasOne(d => d.CardNumNavigation).WithMany(p => p.Phones)
                .HasForeignKey(d => d.CardNum)
                .HasConstraintName("Phones_ibfk_1");
        });

        modelBuilder.Entity<Titles>(entity =>
        {
            entity.HasKey(e => e.Isbn).HasName("PRIMARY");

            entity.Property(e => e.Isbn)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practica_2.Models;

public partial class ListasDbContext : DbContext
{
    public ListasDbContext()
    {
    }

    public ListasDbContext(DbContextOptions<ListasDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cancion> Cancions { get; set; }

    public virtual DbSet<ListasDeReproduccion> ListasDeReproduccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=localhost\\SQLEXPRESS;database=ListasDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cancion>(entity =>
        {
            entity.HasKey(e => e.CancionId).HasName("PK__Cancion__EDA6B1AF01340006");

            entity.ToTable("Cancion");

            entity.Property(e => e.CancionId).HasColumnName("CancionID");
            entity.Property(e => e.Album).HasMaxLength(100);
            entity.Property(e => e.Artista).HasMaxLength(100);
            entity.Property(e => e.FechaAgregada)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ListaId).HasColumnName("ListaID");
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.Lista).WithMany(p => p.Cancions)
                .HasForeignKey(d => d.ListaId)
                .HasConstraintName("FK__Cancion__ListaID__403A8C7D");
        });

        modelBuilder.Entity<ListasDeReproduccion>(entity =>
        {
            entity.HasKey(e => e.ListaId).HasName("PK__ListasDe__2B0A743F2DA3310D");

            entity.ToTable("ListasDeReproduccion");

            entity.Property(e => e.ListaId).HasColumnName("ListaID");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ListasDeReproduccions)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ListasDeR__Usuar__3C69FB99");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798836FF18A");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A19206A1D62").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Contraseña).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

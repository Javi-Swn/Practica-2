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

    public virtual DbSet<Cancione> Canciones { get; set; }

    public virtual DbSet<ListasDeReproduccion> ListasDeReproduccions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost\\SQLEXPRESS;database=ListasDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cancione>(entity =>
        {
            entity.HasKey(e => e.CancionId).HasName("PK__Cancione__EDA6B1AF5A26FD65");

            entity.Property(e => e.CancionId).HasColumnName("CancionID");
            entity.Property(e => e.Album).HasMaxLength(100);
            entity.Property(e => e.Artista).HasMaxLength(100);
            entity.Property(e => e.FechaAgregada)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ListaId).HasColumnName("ListaID");
            entity.Property(e => e.Titulo).HasMaxLength(100);

            entity.HasOne(d => d.Lista).WithMany(p => p.Canciones)
                .HasForeignKey(d => d.ListaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Canciones__Lista__403A8C7D");
        });

        modelBuilder.Entity<ListasDeReproduccion>(entity =>
        {
            entity.HasKey(e => e.ListaId).HasName("PK__ListasDe__2B0A743FCFC85AC1");

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
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798D07F39D9");

            entity.HasIndex(e => e.Correo, "UQ__Usuarios__60695A1981BF9F72").IsUnique();

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

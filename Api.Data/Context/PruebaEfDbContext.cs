using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context;

public partial class PruebaEfDbContext : DbContext
{
    public PruebaEfDbContext()
    {
    }

    public PruebaEfDbContext(DbContextOptions<PruebaEfDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadoCivil> EstadoCivils { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadoCivil>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCivil);

            entity.ToTable("Estado_Civil");

            entity.Property(e => e.IdEstadoCivil).HasColumnName("Id_EstadoCivil");
            entity.Property(e => e.EstadoCivil1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EstadoCivil");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Documento);

            entity.ToTable("Persona");

            entity.Property(e => e.Documento)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IdDocumento).HasColumnName("Id_Documento");
            entity.Property(e => e.IdEstadoCivil).HasColumnName("Id_EstadoCivil");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ValorGanar).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.IdDocumentoNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Persona_Tipo_Documento");

            entity.HasOne(d => d.IdEstadoCivilNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdEstadoCivil)
                .HasConstraintName("FK_Persona_Estado_Civil");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdDocumento);

            entity.ToTable("Tipo_Documento");

            entity.Property(e => e.IdDocumento).HasColumnName("Id_Documento");
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

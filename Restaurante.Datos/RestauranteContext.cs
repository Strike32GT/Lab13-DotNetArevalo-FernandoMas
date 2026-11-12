using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Restaurante.Entidades;

namespace Restaurante.Datos;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Plato> Platos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasIndex(e => e.Nombre, "UX_Categorias_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Categorias_Activo");
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<Plato>(entity =>
        {
            entity.HasIndex(e => new { e.CategoriaId, e.Nombre }, "UX_Platos_Categoria_Nombre").IsUnique();

            entity.Property(e => e.Descripcion).HasMaxLength(250);
            entity.Property(e => e.Disponible).HasDefaultValue(true, "DF_Platos_Disponible");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Platos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Platos_Categorias");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

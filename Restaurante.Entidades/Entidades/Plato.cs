using System;
using System.Collections.Generic;

namespace Restaurante.Entidades;

public partial class Plato
{
    public int PlatoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public bool Disponible { get; set; }

    public int CategoriaId { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;
}

namespace Restaurante.Entidades.DTOs;

/// <summary>
/// DTO de lectura de un plato. Incluye el nombre de su categoría para no
/// exponer la entidad relacionada ni generar ciclos en el JSON.
/// </summary>
public class PlatoDto
{
    public int PlatoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public bool Disponible { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;
}

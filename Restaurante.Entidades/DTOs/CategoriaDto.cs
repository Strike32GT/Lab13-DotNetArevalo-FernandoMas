namespace Restaurante.Entidades.DTOs;

/// <summary>
/// DTO de lectura de una categoría. Es lo único que la API devuelve al cliente.
/// </summary>
public class CategoriaDto
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    public int CantidadPlatos { get; set; }
}

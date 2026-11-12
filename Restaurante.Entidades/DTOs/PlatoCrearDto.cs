using System.ComponentModel.DataAnnotations;

namespace Restaurante.Entidades.DTOs;

/// <summary>
/// DTO de entrada para registrar un plato nuevo.
/// </summary>
public class PlatoCrearDto
{
    [Required(ErrorMessage = "El nombre del plato es obligatorio.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(250, ErrorMessage = "La descripción no puede superar los 250 caracteres.")]
    public string? Descripcion { get; set; }

    [Range(1.00, 500.00, ErrorMessage = "El precio debe estar entre 1.00 y 500.00 soles.")]
    public decimal Precio { get; set; }

    public bool Disponible { get; set; } = true;

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar una categoría válida.")]
    public int CategoriaId { get; set; }
}

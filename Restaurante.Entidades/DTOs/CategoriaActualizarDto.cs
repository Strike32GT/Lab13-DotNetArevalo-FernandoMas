using System.ComponentModel.DataAnnotations;

namespace Restaurante.Entidades.DTOs;

/// <summary>
/// DTO de entrada para actualizar una categoría existente.
/// </summary>
public class CategoriaActualizarDto
{
    [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;
}

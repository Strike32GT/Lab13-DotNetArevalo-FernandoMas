using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Interfaces;

public interface IPlatoServicio
{
    Task<IEnumerable<PlatoDto>> ObtenerTodosAsync(
        string? nombre,
        int? categoriaId,
        decimal? precioMin,
        decimal? precioMax,
        bool? disponible);

    Task<PlatoDto?> ObtenerPorIdAsync(int platoId);

    /// <summary>
    /// Devuelve null cuando la categoría no existe, para que el controlador
    /// responda 404 en la ruta anidada.
    /// </summary>
    Task<IEnumerable<PlatoDto>?> ObtenerPorCategoriaAsync(int categoriaId);

    Task<PlatoDto> CrearAsync(PlatoCrearDto dto);
    Task<bool> ActualizarAsync(int platoId, PlatoActualizarDto dto);
    Task<bool> EliminarAsync(int platoId);
}

using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Interfaces;

public interface ICategoriaServicio
{
    Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync(string? nombre, bool? activo);
    Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId);
    Task<CategoriaDto> CrearAsync(CategoriaCrearDto dto);
    Task<bool> ActualizarAsync(int categoriaId, CategoriaActualizarDto dto);
    Task<bool> EliminarAsync(int categoriaId);
}

using Restaurante.Entidades;

namespace Restaurante.Datos.Interfaces;

/// <summary>
/// Contrato de acceso a datos para las categorías.
/// </summary>
public interface ICategoriaRepositorio
{
    Task<IEnumerable<Categoria>> ObtenerTodasAsync(string? nombre, bool? activo);
    Task<Categoria?> ObtenerPorIdAsync(int categoriaId);
    Task<bool> ExisteAsync(int categoriaId);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirCategoriaId = null);
    Task<int> ContarPlatosAsync(int categoriaId);
    Task<int> ContarPlatosDisponiblesAsync(int categoriaId);
    Task<Categoria> CrearAsync(Categoria categoria);
    Task ActualizarAsync(Categoria categoria);
    Task EliminarAsync(int categoriaId);
}

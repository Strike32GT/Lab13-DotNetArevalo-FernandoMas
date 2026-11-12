using Restaurante.Entidades;

namespace Restaurante.Datos.Interfaces;

/// <summary>
/// Contrato de acceso a datos para los platos.
/// </summary>
public interface IPlatoRepositorio
{
    Task<IEnumerable<Plato>> ObtenerTodosAsync(
        string? nombre,
        int? categoriaId,
        decimal? precioMin,
        decimal? precioMax,
        bool? disponible);

    Task<Plato?> ObtenerPorIdAsync(int platoId);
    Task<IEnumerable<Plato>> ObtenerPorCategoriaAsync(int categoriaId);
    Task<bool> ExisteAsync(int platoId);
    Task<bool> ExisteNombreEnCategoriaAsync(string nombre, int categoriaId, int? excluirPlatoId = null);
    Task<Plato> CrearAsync(Plato plato);
    Task ActualizarAsync(Plato plato);
    Task EliminarAsync(int platoId);
}

using Microsoft.EntityFrameworkCore;
using Restaurante.Datos.Interfaces;
using Restaurante.Entidades;

namespace Restaurante.Datos.Repositorios;

public class PlatoRepositorio : IPlatoRepositorio
{
    private readonly RestauranteContext _contexto;

    public PlatoRepositorio(RestauranteContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IEnumerable<Plato>> ObtenerTodosAsync(
        string? nombre,
        int? categoriaId,
        decimal? precioMin,
        decimal? precioMax,
        bool? disponible)
    {
        var consulta = _contexto.Platos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            consulta = consulta.Where(p => p.Nombre.Contains(nombre.Trim()));
        }

        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);
        }

        if (precioMin.HasValue)
        {
            consulta = consulta.Where(p => p.Precio >= precioMin.Value);
        }

        if (precioMax.HasValue)
        {
            consulta = consulta.Where(p => p.Precio <= precioMax.Value);
        }

        if (disponible.HasValue)
        {
            consulta = consulta.Where(p => p.Disponible == disponible.Value);
        }

        return await consulta
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Plato?> ObtenerPorIdAsync(int platoId)
    {
        return await _contexto.Platos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.PlatoId == platoId);
    }

    public async Task<IEnumerable<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        return await _contexto.Platos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .Where(p => p.CategoriaId == categoriaId)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<bool> ExisteAsync(int platoId)
    {
        return await _contexto.Platos
            .AsNoTracking()
            .AnyAsync(p => p.PlatoId == platoId);
    }

    public async Task<bool> ExisteNombreEnCategoriaAsync(
        string nombre,
        int categoriaId,
        int? excluirPlatoId = null)
    {
        var consulta = _contexto.Platos
            .AsNoTracking()
            .Where(p => p.Nombre == nombre && p.CategoriaId == categoriaId);

        if (excluirPlatoId.HasValue)
        {
            consulta = consulta.Where(p => p.PlatoId != excluirPlatoId.Value);
        }

        return await consulta.AnyAsync();
    }

    public async Task<Plato> CrearAsync(Plato plato)
    {
        await _contexto.Platos.AddAsync(plato);
        await _contexto.SaveChangesAsync();
        return plato;
    }

    public async Task ActualizarAsync(Plato plato)
    {
        _contexto.Entry(plato).State = EntityState.Modified;
        await _contexto.SaveChangesAsync();
    }

    public async Task EliminarAsync(int platoId)
    {
        var plato = await _contexto.Platos.FindAsync(platoId);

        if (plato is not null)
        {
            _contexto.Platos.Remove(plato);
            await _contexto.SaveChangesAsync();
        }
    }
}

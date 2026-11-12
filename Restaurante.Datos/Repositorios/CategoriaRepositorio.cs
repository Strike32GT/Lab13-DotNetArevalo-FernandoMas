using Microsoft.EntityFrameworkCore;
using Restaurante.Datos.Interfaces;
using Restaurante.Entidades;

namespace Restaurante.Datos.Repositorios;

public class CategoriaRepositorio : ICategoriaRepositorio
{
    private readonly RestauranteContext _contexto;

    public CategoriaRepositorio(RestauranteContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IEnumerable<Categoria>> ObtenerTodasAsync(string? nombre, bool? activo)
    {
        var consulta = _contexto.Categorias
            .AsNoTracking()
            .Include(c => c.Platos)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            consulta = consulta.Where(c => c.Nombre.Contains(nombre.Trim()));
        }

        if (activo.HasValue)
        {
            consulta = consulta.Where(c => c.Activo == activo.Value);
        }

        return await consulta
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<Categoria?> ObtenerPorIdAsync(int categoriaId)
    {
        return await _contexto.Categorias
            .AsNoTracking()
            .Include(c => c.Platos)
            .FirstOrDefaultAsync(c => c.CategoriaId == categoriaId);
    }

    public async Task<bool> ExisteAsync(int categoriaId)
    {
        return await _contexto.Categorias
            .AsNoTracking()
            .AnyAsync(c => c.CategoriaId == categoriaId);
    }

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirCategoriaId = null)
    {
        var consulta = _contexto.Categorias
            .AsNoTracking()
            .Where(c => c.Nombre == nombre);

        if (excluirCategoriaId.HasValue)
        {
            consulta = consulta.Where(c => c.CategoriaId != excluirCategoriaId.Value);
        }

        return await consulta.AnyAsync();
    }

    public async Task<int> ContarPlatosAsync(int categoriaId)
    {
        return await _contexto.Platos
            .AsNoTracking()
            .CountAsync(p => p.CategoriaId == categoriaId);
    }

    public async Task<int> ContarPlatosDisponiblesAsync(int categoriaId)
    {
        return await _contexto.Platos
            .AsNoTracking()
            .CountAsync(p => p.CategoriaId == categoriaId && p.Disponible);
    }

    public async Task<Categoria> CrearAsync(Categoria categoria)
    {
        await _contexto.Categorias.AddAsync(categoria);
        await _contexto.SaveChangesAsync();
        return categoria;
    }

    public async Task ActualizarAsync(Categoria categoria)
    {
        // La entidad llega sin seguimiento; se marca solo la categoría como modificada,
        // sin arrastrar la colección de platos.
        _contexto.Entry(categoria).State = EntityState.Modified;
        await _contexto.SaveChangesAsync();
    }

    public async Task EliminarAsync(int categoriaId)
    {
        var categoria = await _contexto.Categorias.FindAsync(categoriaId);

        if (categoria is not null)
        {
            _contexto.Categorias.Remove(categoria);
            await _contexto.SaveChangesAsync();
        }
    }
}

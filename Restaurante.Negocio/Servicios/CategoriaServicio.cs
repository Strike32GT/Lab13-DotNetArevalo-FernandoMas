using AutoMapper;
using Restaurante.Datos.Interfaces;
using Restaurante.Entidades;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Excepciones;
using Restaurante.Negocio.Interfaces;

namespace Restaurante.Negocio.Servicios;

public class CategoriaServicio : ICategoriaServicio
{
    private readonly ICategoriaRepositorio _repositorio;
    private readonly IMapper _mapper;

    public CategoriaServicio(ICategoriaRepositorio repositorio, IMapper mapper)
    {
        _repositorio = repositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync(string? nombre, bool? activo)
    {
        var categorias = await _repositorio.ObtenerTodasAsync(nombre, activo);
        return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int categoriaId)
    {
        var categoria = await _repositorio.ObtenerPorIdAsync(categoriaId);

        return categoria is null
            ? null
            : _mapper.Map<CategoriaDto>(categoria);
    }

    public async Task<CategoriaDto> CrearAsync(CategoriaCrearDto dto)
    {
        var nombre = dto.Nombre.Trim();

        if (await _repositorio.ExisteNombreAsync(nombre))
        {
            throw new ReglaNegocioException(
                $"Ya existe una categoría registrada con el nombre '{nombre}'.");
        }

        var categoria = _mapper.Map<Categoria>(dto);
        categoria.Nombre = nombre;
        categoria.Activo = true;

        var creada = await _repositorio.CrearAsync(categoria);

        return _mapper.Map<CategoriaDto>(creada);
    }

    public async Task<bool> ActualizarAsync(int categoriaId, CategoriaActualizarDto dto)
    {
        var categoria = await _repositorio.ObtenerPorIdAsync(categoriaId);

        if (categoria is null)
        {
            return false;
        }

        var nombre = dto.Nombre.Trim();

        if (await _repositorio.ExisteNombreAsync(nombre, categoriaId))
        {
            throw new ReglaNegocioException(
                $"Ya existe otra categoría registrada con el nombre '{nombre}'.");
        }

        // Regla de negocio: no se puede desactivar una categoría que todavía
        // tiene platos disponibles para la venta.
        if (categoria.Activo && !dto.Activo)
        {
            var disponibles = await _repositorio.ContarPlatosDisponiblesAsync(categoriaId);

            if (disponibles > 0)
            {
                throw new ReglaNegocioException(
                    $"No se puede desactivar la categoría '{categoria.Nombre}' porque tiene {disponibles} plato(s) disponible(s).");
            }
        }

        _mapper.Map(dto, categoria);
        categoria.Nombre = nombre;

        await _repositorio.ActualizarAsync(categoria);
        return true;
    }

    public async Task<bool> EliminarAsync(int categoriaId)
    {
        if (!await _repositorio.ExisteAsync(categoriaId))
        {
            return false;
        }

        // Regla de negocio: una categoría con platos no se puede eliminar,
        // primero hay que reubicar o eliminar sus platos.
        var cantidadPlatos = await _repositorio.ContarPlatosAsync(categoriaId);

        if (cantidadPlatos > 0)
        {
            throw new ReglaNegocioException(
                $"No se puede eliminar la categoría porque tiene {cantidadPlatos} plato(s) registrado(s).");
        }

        await _repositorio.EliminarAsync(categoriaId);
        return true;
    }
}

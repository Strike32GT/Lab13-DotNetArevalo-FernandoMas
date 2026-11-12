using AutoMapper;
using Restaurante.Datos.Interfaces;
using Restaurante.Entidades;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Excepciones;
using Restaurante.Negocio.Interfaces;

namespace Restaurante.Negocio.Servicios;

public class PlatoServicio : IPlatoServicio
{
    private readonly IPlatoRepositorio _platoRepositorio;
    private readonly ICategoriaRepositorio _categoriaRepositorio;
    private readonly IMapper _mapper;

    public PlatoServicio(
        IPlatoRepositorio platoRepositorio,
        ICategoriaRepositorio categoriaRepositorio,
        IMapper mapper)
    {
        _platoRepositorio = platoRepositorio;
        _categoriaRepositorio = categoriaRepositorio;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlatoDto>> ObtenerTodosAsync(
        string? nombre,
        int? categoriaId,
        decimal? precioMin,
        decimal? precioMax,
        bool? disponible)
    {
        var platos = await _platoRepositorio.ObtenerTodosAsync(
            nombre, categoriaId, precioMin, precioMax, disponible);

        return _mapper.Map<IEnumerable<PlatoDto>>(platos);
    }

    public async Task<PlatoDto?> ObtenerPorIdAsync(int platoId)
    {
        var plato = await _platoRepositorio.ObtenerPorIdAsync(platoId);

        return plato is null
            ? null
            : _mapper.Map<PlatoDto>(plato);
    }

    public async Task<IEnumerable<PlatoDto>?> ObtenerPorCategoriaAsync(int categoriaId)
    {
        if (!await _categoriaRepositorio.ExisteAsync(categoriaId))
        {
            return null;
        }

        var platos = await _platoRepositorio.ObtenerPorCategoriaAsync(categoriaId);
        return _mapper.Map<IEnumerable<PlatoDto>>(platos);
    }

    public async Task<PlatoDto> CrearAsync(PlatoCrearDto dto)
    {
        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(dto.CategoriaId);

        if (categoria is null)
        {
            throw new ReglaNegocioException(
                $"La categoría con id {dto.CategoriaId} no existe.");
        }

        // Regla de negocio: solo se ofrecen platos de categorías activas.
        if (!categoria.Activo)
        {
            throw new ReglaNegocioException(
                $"No se puede registrar un plato en la categoría '{categoria.Nombre}' porque está inactiva.");
        }

        var nombre = dto.Nombre.Trim();

        // Regla de negocio: no se repite el nombre de un plato dentro de la misma categoría.
        if (await _platoRepositorio.ExisteNombreEnCategoriaAsync(nombre, dto.CategoriaId))
        {
            throw new ReglaNegocioException(
                $"Ya existe un plato llamado '{nombre}' en la categoría '{categoria.Nombre}'.");
        }

        var plato = _mapper.Map<Plato>(dto);
        plato.Nombre = nombre;

        var creado = await _platoRepositorio.CrearAsync(plato);

        // Se vuelve a leer para traer la categoría y poder devolver CategoriaNombre.
        var platoCompleto = await _platoRepositorio.ObtenerPorIdAsync(creado.PlatoId);

        return _mapper.Map<PlatoDto>(platoCompleto!);
    }

    public async Task<bool> ActualizarAsync(int platoId, PlatoActualizarDto dto)
    {
        var plato = await _platoRepositorio.ObtenerPorIdAsync(platoId);

        if (plato is null)
        {
            return false;
        }

        var categoria = await _categoriaRepositorio.ObtenerPorIdAsync(dto.CategoriaId);

        if (categoria is null)
        {
            throw new ReglaNegocioException(
                $"La categoría con id {dto.CategoriaId} no existe.");
        }

        // Solo se exige categoría activa cuando el plato cambia de categoría.
        // Así se puede corregir un plato que ya pertenece a una categoría inactiva.
        if (plato.CategoriaId != dto.CategoriaId && !categoria.Activo)
        {
            throw new ReglaNegocioException(
                $"No se puede mover el plato a la categoría '{categoria.Nombre}' porque está inactiva.");
        }

        var nombre = dto.Nombre.Trim();

        if (await _platoRepositorio.ExisteNombreEnCategoriaAsync(nombre, dto.CategoriaId, platoId))
        {
            throw new ReglaNegocioException(
                $"Ya existe otro plato llamado '{nombre}' en la categoría '{categoria.Nombre}'.");
        }

        _mapper.Map(dto, plato);
        plato.Nombre = nombre;

        await _platoRepositorio.ActualizarAsync(plato);
        return true;
    }

    public async Task<bool> EliminarAsync(int platoId)
    {
        if (!await _platoRepositorio.ExisteAsync(platoId))
        {
            return false;
        }

        await _platoRepositorio.EliminarAsync(platoId);
        return true;
    }
}

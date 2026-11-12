using AutoMapper;
using Restaurante.Entidades;
using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Mapeos;

/// <summary>
/// Perfil único de AutoMapper con todos los mapeos de la aplicación.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entidad -> DTO de lectura.
        // CantidadPlatos se calcula con la colección de platos ya cargada por el repositorio.
        CreateMap<Categoria, CategoriaDto>()
            .ForMember(destino => destino.CantidadPlatos,
                opcion => opcion.MapFrom(origen => origen.Platos.Count));

        // DTO de entrada -> Entidad, para crear y para actualizar.
        CreateMap<CategoriaCrearDto, Categoria>();
        CreateMap<CategoriaActualizarDto, Categoria>();

        // Entidad -> DTO de lectura.
        // En lugar de devolver el objeto Categoria completo, se devuelve solo su nombre.
        CreateMap<Plato, PlatoDto>()
            .ForMember(destino => destino.CategoriaNombre,
                opcion => opcion.MapFrom(origen =>
                    origen.Categoria != null ? origen.Categoria.Nombre : string.Empty));

        // DTO de entrada -> Entidad, para crear y para actualizar.
        CreateMap<PlatoCrearDto, Plato>();
        CreateMap<PlatoActualizarDto, Plato>();
    }
}

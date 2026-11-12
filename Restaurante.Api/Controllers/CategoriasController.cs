using Microsoft.AspNetCore.Mvc;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Excepciones;
using Restaurante.Negocio.Interfaces;

namespace Restaurante.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaServicio _categoriaServicio;
    private readonly IPlatoServicio _platoServicio;

    public CategoriasController(
        ICategoriaServicio categoriaServicio,
        IPlatoServicio platoServicio)
    {
        _categoriaServicio = categoriaServicio;
        _platoServicio = platoServicio;
    }

    /// <summary>
    /// Lista las categorías. Permite filtrar por nombre y por estado.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> ObtenerTodas(
        [FromQuery] string? nombre,
        [FromQuery] bool? activo)
    {
        var categorias = await _categoriaServicio.ObtenerTodasAsync(nombre, activo);
        return Ok(categorias);
    }

    /// <summary>
    /// Obtiene una categoría por su identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoriaDto>> ObtenerPorId(int id)
    {
        var categoria = await _categoriaServicio.ObtenerPorIdAsync(id);

        if (categoria is null)
        {
            return NotFound(new { mensaje = $"No existe la categoría con id {id}." });
        }

        return Ok(categoria);
    }

    /// <summary>
    /// Registra una categoría nueva.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoriaDto>> Crear([FromBody] CategoriaCrearDto dto)
    {
        try
        {
            var creada = await _categoriaServicio.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creada.CategoriaId },
                creada);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una categoría existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] CategoriaActualizarDto dto)
    {
        try
        {
            var actualizada = await _categoriaServicio.ActualizarAsync(id, dto);

            if (!actualizada)
            {
                return NotFound(new { mensaje = $"No existe la categoría con id {id}." });
            }

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Elimina una categoría que no tenga platos registrados.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminada = await _categoriaServicio.EliminarAsync(id);

            if (!eliminada)
            {
                return NotFound(new { mensaje = $"No existe la categoría con id {id}." });
            }

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Ruta anidada: devuelve los platos de una categoría.
    /// </summary>
    [HttpGet("{categoriaId:int}/platos")]
    [ProducesResponseType(typeof(IEnumerable<PlatoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PlatoDto>>> ObtenerPlatos(int categoriaId)
    {
        var platos = await _platoServicio.ObtenerPorCategoriaAsync(categoriaId);

        if (platos is null)
        {
            return NotFound(new { mensaje = $"No existe la categoría con id {categoriaId}." });
        }

        return Ok(platos);
    }
}

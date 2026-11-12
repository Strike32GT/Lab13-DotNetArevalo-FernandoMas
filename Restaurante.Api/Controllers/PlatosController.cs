using Microsoft.AspNetCore.Mvc;
using Restaurante.Entidades.DTOs;
using Restaurante.Negocio.Excepciones;
using Restaurante.Negocio.Interfaces;

namespace Restaurante.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlatosController : ControllerBase
{
    private readonly IPlatoServicio _platoServicio;

    public PlatosController(IPlatoServicio platoServicio)
    {
        _platoServicio = platoServicio;
    }

    /// <summary>
    /// Lista los platos. Filtros opcionales por nombre, categoría, rango de precio
    /// y disponibilidad.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PlatoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PlatoDto>>> ObtenerTodos(
        [FromQuery] string? nombre,
        [FromQuery] int? categoriaId,
        [FromQuery] decimal? precioMin,
        [FromQuery] decimal? precioMax,
        [FromQuery] bool? disponible)
    {
        var platos = await _platoServicio.ObtenerTodosAsync(
            nombre, categoriaId, precioMin, precioMax, disponible);

        return Ok(platos);
    }

    /// <summary>
    /// Obtiene un plato por su identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PlatoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlatoDto>> ObtenerPorId(int id)
    {
        var plato = await _platoServicio.ObtenerPorIdAsync(id);

        if (plato is null)
        {
            return NotFound(new { mensaje = $"No existe el plato con id {id}." });
        }

        return Ok(plato);
    }

    /// <summary>
    /// Registra un plato nuevo.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PlatoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PlatoDto>> Crear([FromBody] PlatoCrearDto dto)
    {
        try
        {
            var creado = await _platoServicio.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = creado.PlatoId },
                creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un plato existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] PlatoActualizarDto dto)
    {
        try
        {
            var actualizado = await _platoServicio.ActualizarAsync(id, dto);

            if (!actualizado)
            {
                return NotFound(new { mensaje = $"No existe el plato con id {id}." });
            }

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un plato existente.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _platoServicio.EliminarAsync(id);

        if (!eliminado)
        {
            return NotFound(new { mensaje = $"No existe el plato con id {id}." });
        }

        return NoContent();
    }
}

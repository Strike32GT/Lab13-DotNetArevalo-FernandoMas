namespace Restaurante.Negocio.Excepciones;

/// <summary>
/// Excepción propia para incumplimientos de reglas de negocio.
/// El controlador la traduce a un código HTTP 409 Conflict.
/// </summary>
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje)
        : base(mensaje)
    {
    }
}

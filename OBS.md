# Observaciones del laboratorio

El scaffold generó las entidades con sus propiedades de navegación y sin [JsonIgnore]. Al principio parece un riesgo, pero deja de serlo cuando todas las respuestas usan DTOs. La API nunca serializa Categoria ni Plato, así que el ciclo entre ambas no llega a producirse.

La cadena de referencias entre proyectos se nota al compilar. El proyecto Api solo referencia a Negocio, y aun así puede usar los DTOs porque la referencia llega de forma transitiva desde Entidades. En cambio, no tiene acceso al DbContext ni a los repositorios, lo que obliga a mantener el controlador delgado.

Separar CategoriaDto de CategoriaCrearDto y CategoriaActualizarDto parecía repetitivo al inicio, pero evita mezclar campos de salida con campos de entrada. Por ejemplo, CategoriaDto muestra CantidadPlatos y CategoriaActualizarDto permite cambiar Activo; ninguna de esas cosas debería estar en el otro DTO.

El ForMember de CategoriaDto funciona bien porque el repositorio carga la colección de platos con Include. Si no la cargara, CantidadPlatos saldría en cero aunque la categoría tuviera platos. Es una dependencia silenciosa que conviene tener presente al cambiar las consultas.

El ForMember de PlatoDto es el que más se nota en las respuestas. En lugar de devolver un objeto Categoria anidado, el JSON muestra categoriaNombre. Eso hace la respuesta más liviana y elimina el ciclo de serialización sin necesidad de ignorar propiedades.

Al crear un plato, la entidad recién insertada no trae la categoría cargada, así que el servicio vuelve a consultar el plato por id antes de mapearlo. Sin esa segunda consulta, CategoriaNombre saldría vacío en el 201. Es un detalle práctico que no aparece en la teoría del mapeo.

No usé ReverseMap porque los DTOs de lectura tienen campos calculados que no existen en la entidad. Un ReverseMap de PlatoDto a Plato intentaría mapear CategoriaNombre y dejaría un mapeo confuso. Declarar los seis CreateMap por separado es más código, pero es más claro y evita sorpresas.

Las reglas de negocio se probaron con casos reales: intentar borrar la categoría Platos de fondo devuelve 409, intentar desactivarla también devuelve 409 porque tiene platos disponibles, y crear un plato en una categoría inactiva devuelve 409. Los mensajes explican el motivo y eso ayuda a quien consume la API.

El código 400 no se programó en los controladores. Lo genera automáticamente [ApiController] cuando falla una Data Annotation. Eso reduce código repetido, pero obliga a revisar que los DTOs de entrada tengan las validaciones correctas, porque de lo contrario el error llega hasta el servicio.

Los repositorios usan AsNoTracking en las lecturas y EntityState.Modified para las actualizaciones. Esa combinación funciona porque las entidades se cargan sin seguimiento y luego se marcan explícitamente. En los DELETE se busca la entidad con FindAsync para eliminar solo el registro y no arrastrar la colección de platos.

El script SQL debe ejecutarse con sqlcmd y la opción -f 65001. Sin esa opción, los acentos se guardan mal y textos como día o clásico terminan con caracteres extraños. Lo detecté al revisar los datos de ejemplo y lo corregí volviendo a ejecutar el script con la codificación correcta.

El SDK de .NET 10 creó un archivo Restaurante.slnx en lugar del clásico .sln. Visual Studio y la CLI lo abren sin problema, pero conviene saberlo si alguien busca el archivo .sln de versiones anteriores.

Al instalar Swashbuckle apareció un conflicto de versiones con Microsoft.OpenApi. La plantilla de .NET 10 traía Microsoft.AspNetCore.OpenApi, que exige una versión anterior a 3.0, y Swashbuckle 11 exige 3.11. La solución fue quitar el paquete Microsoft.AspNetCore.OpenApi y dejar Swashbuckle, que es el que se usa para generar Swagger y su interfaz.

AutoMapper 16 muestra una advertencia de licencia en el log. Es normal en desarrollo y no impide que la API funcione. La advertencia desaparece con una licencia, pero para este laboratorio no es necesaria.

La herramienta dotnet-ef instalada era 10.0.13 y el runtime de EF Core era 10.0.15. El scaffold avisó de esa diferencia de versión, aunque generó el modelo sin errores. Si se regenera el modelo, conviene actualizar la herramienta para evitar futuros avisos.

El certificado de desarrollo de HTTPS no estaba confiado en el equipo, por eso las pruebas con curl usan -k. En el navegador se puede ejecutar dotnet dev-certs https --trust una vez para evitar la advertencia.

Las pruebas se ejecutaron con un script de PowerShell que guarda la respuesta completa de cada solicitud. Fueron 34 casos y todos pasaron. La tabla de docs/PRUEBAS.md y los archivos de docs/evidencias quedaron como respaldo de las capturas.

El script de pruebas usa los identificadores 5 y 10 para la categoría y el plato creados. Eso funciona porque la base se restaura antes de ejecutarlo. Si se corre sobre una base con otros registros, los identificadores cambian y las solicitudes que los usan pueden fallar. Es una limitación del script, no de la API.

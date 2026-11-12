# Conclusiones del laboratorio

Trabajar una API en cuatro capas ayuda a separar responsabilidades de verdad. El controlador se dedica a recibir la solicitud y devolver el código correcto; el servicio concentra las reglas del restaurante; el repositorio se encarga de las consultas; y las entidades quedan aisladas en su propio proyecto.

El uso de DTOs resolvió dos problemas a la vez. Por un lado, la API no expone el modelo de la base de datos y puede cambiar sin romper el contrato. Por otro lado, desaparece el ciclo de JSON entre Categoria y Plato sin necesidad de [JsonIgnore], porque las respuestas solo contienen los campos que el cliente necesita.

AutoMapper ahorra código repetitivo, pero no es magia. Los mapeos con nombres distintos o con valores calculados necesitan ForMember y MapFrom. Configurar CantidadPlatos y CategoriaNombre de forma explícita hizo la diferencia entre una respuesta útil y una respuesta incompleta.

Fue más claro mantener DTOs separados para crear y para actualizar. Cada uno tiene sus propias validaciones y su propia intención. Esa separación también permitió que el DTO de actualización de categoría incluyera Activo sin afectar la creación.

Las reglas de negocio se entienden mejor cuando viven en el servicio. El controlador no necesita saber por qué no se puede borrar una categoría con platos; solo captura la excepción y responde 409. Eso mantiene el código del controlador corto y permite reutilizar la misma regla desde otro cliente o desde una prueba.

Los códigos HTTP dejan de ser un detalle y pasan a ser parte del contrato. Un 400 indica que el JSON no cumple las validaciones, un 404 indica que el recurso no existe y un 409 indica que la solicitud es válida pero choca con una regla del negocio. La diferencia entre 400 y 409 fue una de las cosas más útiles de practicar.

El uso de async y LINQ en todas las capas mantiene el código uniforme. Los métodos del repositorio, del servicio y del controlador siguen el mismo patrón de Task, y las consultas se arman con filtros opcionales que se aplican solo cuando el cliente los envía.

La inyección de dependencias hizo que el Program.cs fuera el único lugar donde se decide qué implementación se usa. Si mañana se cambia el repositorio por uno que consulte otra fuente, el controlador y el servicio no tendrían que modificarse.

Swagger fue útil no solo para probar, sino también para comprobar el diseño. Ver que los esquemas mostraban solo DTOs confirmó que la API estaba exponiendo lo correcto y que las entidades quedaban fuera del contrato público.

Probar cada código de respuesta con casos concretos dio más seguridad que revisar el código a simple vista. Los 34 casos ejecutados cubrieron los once endpoints y los seis códigos pedidos, y quedaron documentados con sus respuestas completas.

La base de datos primero con scaffold funcionó bien para este laboratorio. Generó el DbContext y las entidades a partir del script, y luego bastó mover las entidades a su proyecto y ajustar el namespace. Es un camino práctico cuando el esquema ya está definido.

Como mejora futura, agregaría paginación en los listados, autenticación para las operaciones de escritura y pruebas unitarias de las reglas de negocio. También reemplazaría el script manual por migraciones de Entity Framework para tener un historial de cambios del esquema.

En resumen, el laboratorio mostró que una API pequeña también se beneficia de una arquitectura ordenada. La separación en capas, los DTOs, los mapeos explícitos y las reglas en el servicio hacen que el proyecto sea más fácil de entender, de probar y de mantener.

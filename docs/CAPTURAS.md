# Guía para las capturas del laboratorio

Las capturas son evidencia visual que debes tomar en tu equipo. Esta guía indica exactamente qué abrir y qué debe verse en cada imagen, para que coincida con el código entregado.

## 1. Explorador de soluciones con los cuatro proyectos

Abre Restaurante.slnx en Visual Studio y despliega la solución completa. La captura debe mostrar:

- Restaurante.Api, con las carpetas Controllers, Properties y los archivos Program.cs y appsettings.json.
- Restaurante.Negocio, con Excepciones, Interfaces, Mapeos y Servicios.
- Restaurante.Datos, con Interfaces, Repositorios y RestauranteContext.cs.
- Restaurante.Entidades, con Entidades y DTOs.

Si usas VS Code, puedes mostrar el panel del explorador con las cuatro carpetas y, en la terminal, la salida de:

```powershell
dotnet sln Restaurante.slnx list
```

## 2. Código completo del MappingProfile

Abre Restaurante.Negocio/Mapeos/MappingProfile.cs y captura el archivo completo, desde using AutoMapper hasta la última llave. Deben verse los seis CreateMap y los dos ForMember con MapFrom.

## 3. Swagger con solo DTOs

Ejecuta la API y abre https://localhost:7266/swagger. Captura la lista de endpoints y, en la sección Schemas del final de la página, los esquemas CategoriaDto, CategoriaCrearDto, CategoriaActualizarDto, PlatoDto, PlatoCrearDto y PlatoActualizarDto. No deben aparecer Categoria ni Plato.

También puedes capturar la respuesta de https://localhost:7266/swagger/v1/swagger.json en la parte de components.schemas.

## 4. Pruebas de todos los endpoints y códigos

Puedes usar Swagger, Postman, Thunder Client o el archivo Restaurante.Api/Restaurante.Api.http. Las capturas deben mostrar el método, la URL, el cuerpo enviado cuando corresponda y el código de estado de la respuesta.

| Código | Solicitud sugerida | Qué debe verse |
| :-- | :-- | :-- |
| 200 | GET /api/categorias y GET /api/platos | Lista de DTOs con cantidadPlatos y categoriaNombre |
| 200 | GET /api/categorias/1/platos | Ruta anidada funcionando |
| 201 | POST /api/categorias con datos válidos | Código 201 y cabecera Location |
| 201 | POST /api/platos con datos válidos | Código 201 y el plato con categoriaNombre |
| 204 | PUT /api/categorias/5 y DELETE /api/categorias/5 | Sin cuerpo de respuesta |
| 204 | PUT /api/platos/10 y DELETE /api/platos/10 | Sin cuerpo de respuesta |
| 400 | POST /api/categorias con nombre "A" | Errores de validación del DTO |
| 400 | POST /api/platos con precio 0 | Error de [Range] en Precio |
| 404 | GET /api/categorias/999 y GET /api/platos/999 | Mensaje de recurso inexistente |
| 404 | PUT /api/categorias/999 y DELETE /api/platos/999 | Mensaje de recurso inexistente |
| 409 | POST /api/categorias con nombre repetido | Mensaje de regla de negocio |
| 409 | DELETE /api/categorias/2 | No se puede eliminar una categoría con platos |
| 409 | PUT /api/categorias/2 con activo false | No se puede desactivar con platos disponibles |
| 409 | POST /api/platos en una categoría inactiva | No se puede crear en categoría inactiva |
| 409 | POST /api/platos con nombre repetido en la misma categoría | Mensaje de plato duplicado |

La secuencia completa, con los cuerpos exactos y el orden recomendado, está en Restaurante.Api/Restaurante.Api.http. Después de restaurar la base, la categoría creada recibe el id 5 y el plato creado el id 10.

## 5. Evidencia ya generada

El archivo docs/PRUEBAS.md contiene la tabla de 34 pruebas con el código esperado, el código obtenido y la respuesta completa de cada solicitud. La carpeta docs/evidencias guarda un archivo de texto por prueba. Esa documentación sirve como respaldo de las capturas, no las reemplaza.

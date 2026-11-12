# API REST en capas para la gestión de un restaurante

Proyecto de la Semana 13 del curso Desarrollo de Aplicaciones Empresariales Avanzadas. Implementa una Web API REST con ASP.NET Core 10 y Entity Framework Core 10, organizada en cuatro capas y usando DTOs con AutoMapper.

Fernando Mas

## 1. Tema y problema que resuelve

El tema elegido es la gestión del menú de un restaurante. Muchos restaurantes pequeños administran sus cartas en cuadernos, hojas de cálculo o mensajes de WhatsApp, lo que provoca nombres repetidos, precios desactualizados y platos que se siguen ofreciendo aunque ya no estén disponibles.

La API resuelve ese problema con dos entidades relacionadas:

- Categoria: agrupa los platos del menú (entradas, platos de fondo, postres, bebidas). Tiene nombre, descripción y un estado activo.
- Plato: es un producto del menú. Tiene nombre, descripción, precio, disponibilidad y pertenece a una sola categoría.

La relación es uno a muchos: una categoría tiene muchos platos y cada plato pertenece a una sola categoría. En la base de datos se representa con la clave foránea Platos.CategoriaId y en el modelo con las propiedades de navegación Categoria.Platos y Plato.Categoria.

Las entidades no llevan [JsonIgnore] porque la API nunca las devuelve: todas las respuestas usan DTOs. De esa forma tampoco aparece el ciclo de serialización entre Categoria y Plato.

## 2. Estructura de la solución

```
Restaurante.slnx
├── Restaurante.Entidades
│   ├── Entidades
│   │   ├── Categoria.cs
│   │   └── Plato.cs
│   └── DTOs
│       ├── CategoriaDto.cs
│       ├── CategoriaCrearDto.cs
│       ├── CategoriaActualizarDto.cs
│       ├── PlatoDto.cs
│       ├── PlatoCrearDto.cs
│       └── PlatoActualizarDto.cs
├── Restaurante.Datos
│   ├── RestauranteContext.cs
│   ├── Interfaces
│   │   ├── ICategoriaRepositorio.cs
│   │   └── IPlatoRepositorio.cs
│   └── Repositorios
│       ├── CategoriaRepositorio.cs
│       └── PlatoRepositorio.cs
├── Restaurante.Negocio
│   ├── Excepciones
│   │   └── ReglaNegocioException.cs
│   ├── Interfaces
│   │   ├── ICategoriaServicio.cs
│   │   └── IPlatoServicio.cs
│   ├── Mapeos
│   │   └── MappingProfile.cs
│   └── Servicios
│       ├── CategoriaServicio.cs
│       └── PlatoServicio.cs
├── Restaurante.Api
│   ├── Controllers
│   │   ├── CategoriasController.cs
│   │   └── PlatosController.cs
│   ├── Program.cs
│   └── Restaurante.Api.http
├── database
│   └── 01_restaurante.sql
├── docs
│   ├── PRUEBAS.md
│   ├── CAPTURAS.md
│   └── evidencias
├── scripts
│   └── probar-api.ps1
├── CONCL.md
├── OBS.md
└── EXPL.md
```

Las referencias van en cadena: Restaurante.Api referencia a Restaurante.Negocio, Restaurante.Negocio referencia a Restaurante.Datos y Restaurante.Datos referencia a Restaurante.Entidades. El controlador solo conoce servicios por su interfaz y DTOs; no usa el DbContext ni las entidades.

## 3. Base de datos y scaffold

El script database/01_restaurante.sql crea la base RestauranteDB, las tablas Categorias y Platos, sus índices, la clave foránea y datos de ejemplo (4 categorías y 9 platos).

Para ejecutarlo con sqlcmd se usa la opción -f 65001 para que los acentos se lean como UTF-8:

```powershell
sqlcmd -S ".\SQLEXPRESS" -E -f 65001 -i "database\01_restaurante.sql"
```

Cambia .\SQLEXPRESS por la instancia local que tengas. También se puede abrir el script en SQL Server Management Studio y ejecutarlo completo.

El modelo se generó con scaffold, base de datos primero:

```powershell
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=RestauranteDB;Trusted_Connection=True;TrustServerCertificate=True" `
  Microsoft.EntityFrameworkCore.SqlServer `
  --project Restaurante.Datos --startup-project Restaurante.Api `
  --output-dir Models --context RestauranteContext --no-onconfiguring --force
```

El scaffold creó Categoria.cs y Plato.cs en Restaurante.Datos/Models. Después se movieron a Restaurante.Entidades/Entidades y se cambió su namespace a Restaurante.Entidades. El RestauranteContext se movió a la raíz de Restaurante.Datos y ahora importa Restaurante.Entidades.

## 4. DTOs y AutoMapper

Hay un DTO de lectura por entidad y dos DTOs de entrada por entidad. Los DTOs de entrada tienen Data Annotations:

| DTO | Uso | Validaciones principales |
| :-- | :-- | :-- |
| CategoriaDto | Lectura | Ninguna, es de salida |
| CategoriaCrearDto | Crear categoría | [Required], [StringLength(80, MinimumLength = 3)], [StringLength(200)] |
| CategoriaActualizarDto | Actualizar categoría | [Required], [StringLength(80, MinimumLength = 3)], [StringLength(200)] |
| PlatoDto | Lectura | Ninguna, es de salida |
| PlatoCrearDto | Crear plato | [Required], [StringLength(100, MinimumLength = 2)], [StringLength(250)], [Range(1.00, 500.00)], [Range(1, int.MaxValue)] en CategoriaId |
| PlatoActualizarDto | Actualizar plato | Las mismas validaciones que PlatoCrearDto |

El único MappingProfile está en Restaurante.Negocio/Mapeos/MappingProfile.cs:

```csharp
using AutoMapper;
using Restaurante.Entidades;
using Restaurante.Entidades.DTOs;

namespace Restaurante.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Categoria, CategoriaDto>()
            .ForMember(destino => destino.CantidadPlatos,
                opcion => opcion.MapFrom(origen => origen.Platos.Count));

        CreateMap<CategoriaCrearDto, Categoria>();
        CreateMap<CategoriaActualizarDto, Categoria>();

        CreateMap<Plato, PlatoDto>()
            .ForMember(destino => destino.CategoriaNombre,
                opcion => opcion.MapFrom(origen =>
                    origen.Categoria != null ? origen.Categoria.Nombre : string.Empty));

        CreateMap<PlatoCrearDto, Plato>();
        CreateMap<PlatoActualizarDto, Plato>();
    }
}
```

### Sustentación de cada mapeo

| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
| :-- | :-- | :-- | :-- |
| Categoria → CategoriaDto | Entidad → DTO de lectura | Es lo que la API devuelve en los GET de categorías; la entidad no debe salir de la API ni arrastrar su colección de platos | ForMember: CantidadPlatos toma Platos.Count |
| CategoriaCrearDto → Categoria | DTO de entrada → Entidad | Es el JSON que llega del cliente y se convierte en una entidad nueva para guardarla; no se devuelve | Ninguna; el servicio fuerza Activo = true y recorta el nombre |
| CategoriaActualizarDto → Categoria | DTO de entrada → Entidad existente | Se usa Map(dto, entidad) para modificar la categoría que ya se cargó desde el repositorio | Ninguna; incluye Activo para poder desactivar y validar la regla de negocio |
| Plato → PlatoDto | Entidad → DTO de lectura | Es la respuesta de los GET de platos y evita devolver la navegación Categoria, que produciría el ciclo JSON | ForMember: CategoriaNombre toma Categoria.Nombre y usa cadena vacía si no hay categoría |
| PlatoCrearDto → Plato | DTO de entrada → Entidad | Es el JSON que llega del cliente y se convierte en entidad para insertarla; CategoriaId se copia tal cual | Ninguna; el servicio valida antes la categoría y el nombre duplicado |
| PlatoActualizarDto → Plato | DTO de entrada → Entidad existente | Se usa Map(dto, plato) para actualizar el plato cargado sin crear otra instancia | Ninguna; el servicio valida categoría, estado y nombre antes de mapear |

No se usa ReverseMap. Los DTOs de lectura tienen propiedades calculadas que no existen en la entidad (CantidadPlatos y CategoriaNombre), así que un mapeo inverso automático quedaría incompleto o ambiguo. Además, los DTOs de crear y actualizar tienen intenciones y validaciones distintas. Por eso cada dirección se declara de forma explícita.

## 5. Reglas de negocio

Todas las reglas viven en la capa de Negocio y lanzan ReglaNegocioException. Los controladores capturan esa excepción y devuelven 409 Conflict con un JSON de mensaje.

1. No se puede registrar una categoría con un nombre que ya existe.
2. No se puede eliminar una categoría que tenga platos registrados.
3. No se puede desactivar una categoría que tenga platos disponibles.
4. No se puede crear un plato en una categoría inexistente o inactiva.
5. No se puede mover un plato a una categoría inactiva.
6. No se puede repetir el nombre de un plato dentro de la misma categoría.

## 6. Endpoints

La API tiene 11 endpoints: CRUD completo de las dos entidades, filtros por query string y una ruta anidada.

| # | Método | Ruta | Códigos posibles | Descripción |
| :-- | :-- | :-- | :-- | :-- |
| 1 | GET | /api/categorias?nombre=&activo= | 200 | Lista categorías con filtros opcionales |
| 2 | GET | /api/categorias/{id} | 200, 404 | Obtiene una categoría por id |
| 3 | POST | /api/categorias | 201, 400, 409 | Crea una categoría y devuelve Location |
| 4 | PUT | /api/categorias/{id} | 204, 400, 404, 409 | Actualiza una categoría |
| 5 | DELETE | /api/categorias/{id} | 204, 404, 409 | Elimina una categoría sin platos |
| 6 | GET | /api/categorias/{categoriaId}/platos | 200, 404 | Ruta anidada: platos de una categoría |
| 7 | GET | /api/platos?nombre=&categoriaId=&precioMin=&precioMax=&disponible= | 200 | Lista platos con filtros opcionales |
| 8 | GET | /api/platos/{id} | 200, 404 | Obtiene un plato por id |
| 9 | POST | /api/platos | 201, 400, 409 | Crea un plato y devuelve Location |
| 10 | PUT | /api/platos/{id} | 204, 400, 404, 409 | Actualiza un plato |
| 11 | DELETE | /api/platos/{id} | 204, 404 | Elimina un plato |

Correspondencia de códigos usada en los controladores:

| Resultado | Código | Dónde aparece |
| :-- | :-- | :-- |
| Ok(datos) | 200 | GET de categorías y platos |
| CreatedAtAction(...) | 201 | POST correcto de categoría o plato |
| NoContent() | 204 | PUT y DELETE correctos |
| Validación automática de [ApiController] | 400 | DTO de entrada inválido |
| NotFound(...) | 404 | Recurso inexistente |
| Conflict(...) | 409 | Regla de negocio incumplida |

## 7. Inyección de dependencias

En Restaurante.Api/Program.cs se registran:

- El DbContext con UseSqlServer y la cadena de conexión RestauranteDB.
- ICategoriaRepositorio → CategoriaRepositorio y IPlatoRepositorio → PlatoRepositorio con AddScoped.
- ICategoriaServicio → CategoriaServicio e IPlatoServicio → PlatoServicio con AddScoped.
- AutoMapper con AddAutoMapper y el ensamblado donde está MappingProfile.
- Los controladores, Swagger y el pipeline HTTP.

## 8. Cómo ejecutar el proyecto

Requisitos: SDK de .NET 10, SQL Server (Express o LocalDB) y la herramienta dotnet-ef si se quiere regenerar el scaffold.

```powershell
# 1. Crear la base y cargar los datos de ejemplo
sqlcmd -S ".\SQLEXPRESS" -E -f 65001 -i "database\01_restaurante.sql"

# 2. Crear la configuración local a partir del ejemplo y ajustar la instancia
Copy-Item "Restaurante.Api\appsettings.Development.example.json" "Restaurante.Api\appsettings.Development.json"

# 3. Restaurar, compilar y ejecutar
dotnet restore Restaurante.slnx
dotnet build Restaurante.slnx
dotnet run --project Restaurante.Api --launch-profile https

# 4. Abrir Swagger
# https://localhost:7266/swagger
```

Si el navegador advierte por el certificado de desarrollo, ejecutar una vez:

```powershell
dotnet dev-certs https --trust
```

La cadena de conexión real se guarda en Restaurante.Api/appsettings.Development.json, que está excluido con .gitignore. En el repositorio solo queda la plantilla appsettings.Development.example.json y appsettings.json con un servidor de ejemplo, sin contraseñas.

## 9. Pruebas

Se probaron los 11 endpoints y los seis códigos pedidos: 200, 201, 204, 400, 404 y 409. El documento docs/PRUEBAS.md contiene la tabla de 34 casos ejecutados y la respuesta completa de cada uno; las respuestas individuales están en docs/evidencias.

Resultado de la última ejecución: 34 de 34 pruebas aprobadas.

Para repetir las pruebas, primero hay que restaurar la base con el script y luego ejecutar:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File ".\scripts\probar-api.ps1"
```

El script usa identificadores previsibles (categoría 5 y plato 10) después de restaurar la base. Si la base ya tiene otros registros, esos identificadores pueden cambiar.

También se puede usar el archivo Restaurante.Api/Restaurante.Api.http desde Visual Studio o VS Code con la extensión REST Client. Las solicitudes están numeradas y comentadas con el código esperado.

Swagger muestra únicamente los DTOs. El contrato OpenAPI revisado contiene los esquemas CategoriaDto, CategoriaCrearDto, CategoriaActualizarDto, PlatoDto, PlatoCrearDto y PlatoActualizarDto, además de ProblemDetails; no aparecen las entidades Categoria ni Plato.

## 10. Notas de implementación

La plantilla de .NET 10 incluye el paquete Microsoft.AspNetCore.OpenApi. Para usar Swagger con interfaz se instaló Swashbuckle.AspNetCore, que trae SwaggerUI, y se quitó Microsoft.AspNetCore.OpenApi porque exige Microsoft.OpenApi anterior a 3.0, mientras que Swashbuckle 11 exige 3.11. La API usa AddSwaggerGen, UseSwagger y UseSwaggerUI, y el contrato se ve en /swagger/v1/swagger.json.

AutoMapper 16 muestra una advertencia de licencia en el log de desarrollo. Es un comportamiento normal del paquete y no afecta el funcionamiento de la API.

El scaffold se generó con el proyecto Datos y el proyecto Api como startup. Las entidades se movieron después a Restaurante.Entidades y el namespace se ajustó a Restaurante.Entidades.

## 11. Entregables

- Explicación del tema: este README y EXPL.md.
- Repositorio: agregar aquí el enlace del repositorio remoto (pendiente de publicar). El repositorio no incluye contraseñas ni la cadena de conexión local, e incluye database/01_restaurante.sql.
- Captura del Explorador de soluciones con los cuatro proyectos: ver la guía docs/CAPTURAS.md.
- Código completo del MappingProfile: sección 4 de este README y Restaurante.Negocio/Mapeos/MappingProfile.cs.
- Sustentación de los mapeos: tabla de la sección 4.
- Capturas de las pruebas: docs/CAPTURAS.md indica qué capturar; docs/PRUEBAS.md y docs/evidencias contienen la evidencia ejecutada.
- Observaciones y conclusiones: OBS.md y CONCL.md.

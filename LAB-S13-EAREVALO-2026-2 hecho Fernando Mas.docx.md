

                    

                                                                                                                                                                                                  **13**

**Desarrollo de Aplicaciones Empresariales Avanzadas**

Arévalo Sermeño, Edwin William

Desarrollo de Aplicaciones Empresariales Avanzadas  
6 \- C24 \- Sección C \- D  
**API en capas con DTOs y AutoMapper SEMANA13**

En este desafío, se te pide que desarrolles una **Web API REST en capas** (Entidades, Datos, Negocio y API) con ASP.NET Core y Entity Framework Core sobre un **tema de tu elección** (por ejemplo, clínica, veterinaria, gimnasio, tickets o taller mecánico), distinto de la demostración de clase (productos y categorías). La API debe **exponer solo DTOs** y convertirlos con **AutoMapper**. En esta semana **no es necesario publicarla en la nube**.

**Tema y modelo de datos:**

1. Define las entidades de tu tema: **al menos dos** con una relación **1:N** (clave foránea y propiedad de navegación).

2. Crea la base de datos con un **script SQL** (con datos de ejemplo) y genera el modelo con **Scaffold** (base de datos primero). El script va en el repositorio.

3. Las entidades **no usan** \[JsonIgnore\]: la API nunca las devuelve, por eso el ciclo de JSON desaparece.

**Arquitectura en capas:**

1. Crea una solución con **cuatro proyectos**: **Entidades** (entidades y DTOs), **Datos** (DbContext y repositorios), **Negocio** (servicios, reglas de negocio y MappingProfile) y **API** (controladores y Program.cs).

2. Referencias en cadena: **API → Negocio → Datos → Entidades**. El controlador usa solo servicios (por su interfaz) y DTOs; **nunca el DbContext ni las entidades**.

3. Un **repositorio** y un **servicio** (cada uno con su interfaz) para cada entidad principal. Las reglas de negocio viven en el servicio.

4. **Inyección de dependencias:** registra el DbContext, los repositorios, los servicios y AutoMapper en Program.cs.

5. Todos los métodos de repositorios, servicios y controladores son async y usan LINQ.

**DTOs y AutoMapper:**

1. Define **al menos un DTO de lectura por entidad** y **al menos dos DTOs de entrada** (crear y actualizar). Los DTOs de entrada llevan Data Annotations (\[Required\], \[StringLength\], \[Range\]).

2. Un **único MappingProfile** en la capa de Negocio con todos los mapeos, entre ellos **al menos un** ForMember **con** MapFrom (por ejemplo, devolver el nombre de la entidad relacionada en lugar del objeto).

3. Los servicios usan AutoMapper para crear (Map\<T\>) y para actualizar un objeto existente (Map(origen, destino)). Usa ReverseMap() solo si puedes justificarlo.

**Sustentación de los mapeos:** por cada CreateMap de tu MappingProfile explica qué origen y qué destino tiene, **por qué se mapea en ese sentido** y qué se configura aparte. Preséntalo con este formato (ejemplo de la demostración de clase):

| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
| :---- | :---- | :---- | :---- |
| Producto → ProductoDto | Entidad → DTO de lectura | Es lo que la API devuelve al cliente: la entidad no debe salir de la API | ForMember: Categoria toma el Nombre de la categoría |
| ProductoCrearDto → Producto | DTO de entrada → Entidad | Es lo que llega del cliente y se convierte en entidad para guardarla; no se devuelve | Ninguna: los nombres coinciden |

**Web API (ASP.NET Core, .NET 10):**

1. Proyecto **ASP.NET Core Web API con controladores** (no minimal API), un controlador por entidad, \[ApiController\] y rutas api/\[controller\].

2. **CRUD completo** de las dos entidades (**mínimo 10 endpoints**), un GET con **filtros por query string** y una **ruta anidada**.

3. **Swagger** (OpenAPI \+ SwaggerUI) mostrando **solo los DTOs**, nunca las entidades.

4. Implementa **al menos dos reglas de negocio** en la capa de Negocio que lancen una excepción propia (por ejemplo, ReglaNegocioException); el controlador la convierte en **409 Conflict**.

| Resultado | Código | Dónde debe aparecer |
| :---- | :---- | :---- |
| Ok(datos) | 200 | GET que devuelve DTOs |
| CreatedAtAction(...) | 201 | POST correcto (con la ruta del nuevo recurso) |
| NoContent() | 204 | PUT y DELETE correctos |
| BadRequest() o ValidationProblem(...) | 400 | Datos inválidos del DTO de entrada |
| NotFound() | 404 | El recurso solicitado no existe |
| Conflict(...) | 409 | Regla de negocio incumplida |

**Según tu editor:**

1. **Visual Studio:** crea una **solución en blanco** y agrega tres proyectos **Biblioteca de clases** (\<Nombre\>.Entidades, \<Nombre\>.Datos, \<Nombre\>.Negocio) y uno **ASP.NET Core Web API** (\<Nombre\>.Api). En "Información adicional" del proyecto API marca:

| Opción | Valor |
| :---- | :---- |
| Framework | .NET 10 |
| Tipo de autenticación | Ninguno |
| Configurar para HTTPS | Marcado |
| **Usar controladores (Use controllers)** | **Marcado**: si lo dejas sin marcar se crea una minimal API |
| **Habilitar compatibilidad con OpenAPI** | **Marcado** |
| No usar instrucciones de nivel superior | Sin marcar |

Luego agrega las referencias con clic derecho en Dependencias → Agregar referencia de proyecto (Api → Negocio, Negocio → Datos, Datos → Entidades) e instala con Administrar paquetes NuGet: en **Datos**, Microsoft.EntityFrameworkCore.SqlServer; en **Api**, Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.Tools y Swashbuckle.AspNetCore.SwaggerUI; en **Negocio**, AutoMapper. Los paquetes de EF Core deben ser de la **versión 10**.

2. **VS Code:** en una terminal ejecuta lo siguiente (reemplaza \<Nombre\> por el nombre de tu solución):

**dotnet** new sln \-n \<Nombre\>  
**dotnet** new classlib \-n \<Nombre\>.Entidades  
**dotnet** new classlib \-n \<Nombre\>.Datos  
**dotnet** new classlib \-n \<Nombre\>.Negocio  
**dotnet** new webapi \-n \<Nombre\>.Api \--use-controllers  
**dotnet** sln add \<Nombre\>.Entidades \<Nombre\>.Datos  
    \<Nombre\>.Negocio \<Nombre\>.Api  
   
**dotnet** add \<Nombre\>.Api reference \<Nombre\>.Negocio  
**dotnet** add \<Nombre\>.Negocio reference \<Nombre\>.Datos  
**dotnet** add \<Nombre\>.Datos reference \<Nombre\>.Entidades  
   
**dotnet** add \<Nombre\>.Datos package Microsoft.EntityFrameworkCore.SqlServer \--version "10.\*"  
**dotnet** add \<Nombre\>.Api package Microsoft.EntityFrameworkCore.Design \--version "10.\*"  
**dotnet** add \<Nombre\>.Api package Swashbuckle.AspNetCore.SwaggerUI  
**dotnet** add \<Nombre\>.Negocio package AutoMapper

Escribe \--version "10.\*" en los paquetes de EF Core: sin ella se instala la versión 11, que no es compatible con .NET 10\. AutoMapper 16 muestra en el log una advertencia de licencia; es normal en desarrollo.

**Scaffold:** el modelo se genera en el proyecto Datos y después se **mueven las entidades al proyecto Entidades** (ajusta su namespace). Con la CLI (dotnet tool install \--global dotnet-ef una sola vez):

**dotnet** ef dbcontext scaffold "\<cadena de conexión\>" \\  
  Microsoft.EntityFrameworkCore.SqlServer \\  
  \--project \<Nombre\>.Datos \--startup-project \<Nombre\>.Api \\  
  \--output-dir Models \--context \<Nombre\>Context \--no-onconfiguring \--force

En Visual Studio usa la Consola del Administrador de paquetes con \-Project \<Nombre\>.Datos \-StartupProject \<Nombre\>.Api \-OutputDir Models \-Context \<Nombre\>Context \-NoOnConfiguring \-Force.

**Pruebas:**

Prueba **cada endpoint y cada código de respuesta (200, 201, 204, 400, 404 y 409\)** con Postman u otro cliente (Swagger, Thunder Client, archivo .http). Puedes hacerlo en tu equipo local.

**Entregables:**

* Explicación de tu tema (texto): qué problema resuelve, las entidades, su relación y las reglas de negocio:  
* Enlace del repositorio (sin contraseñas ni cadenas de conexión; incluye el script SQL):  
* Captura del Explorador de soluciones con los cuatro proyectos (Entidades, Datos, Negocio y API):  
* Captura del código completo del MappingProfile (todos los CreateMap deben verse):  
* Sustentación en texto de cada mapeo del MappingProfile: origen y destino, por qué se mapea en ese sentido y qué configura cada ForMember o ReverseMap (usa la tabla del formato indicado):  
* Capturas de las pruebas de todos los endpoints y de cada código de respuesta:  
* Observaciones y Conclusiones:
# Pruebas de la API Restaurante

Fecha de ejecución: 2026-11-11 19:01

URL base: https://localhost:7266

Resultado general: 34 de 34 pruebas aprobadas, 0 con fallo.

| # | Caso | Método | Ruta | Esperado | Obtenido | Resultado |
| :-- | :---- | :----- | :--- | :------- | :------- | :-------- |
| 01 | Listar categorías | GET | /api/categorias | 200 | 200 | OK |
| 02 | Filtrar categorías por nombre y activo | GET | /api/categorias?nombre=Ent&activo=true | 200 | 200 | OK |
| 03 | Obtener categoría existente | GET | /api/categorias/1 | 200 | 200 | OK |
| 04 | Categoría inexistente | GET | /api/categorias/999 | 404 | 404 | OK |
| 05 | Ruta anidada: platos de una categoría | GET | /api/categorias/1/platos | 200 | 200 | OK |
| 06 | Ruta anidada con categoría inexistente | GET | /api/categorias/999/platos | 404 | 404 | OK |
| 07 | Crear categoría | POST | /api/categorias | 201 | 201 | OK |
| 08 | Crear categoría duplicada | POST | /api/categorias | 409 | 409 | OK |
| 09 | Crear categoría con datos inválidos | POST | /api/categorias | 400 | 400 | OK |
| 10 | Desactivar categoría sin platos | PUT | /api/categorias/5 | 204 | 204 | OK |
| 11 | Crear plato en categoría inactiva | POST | /api/platos | 409 | 409 | OK |
| 12 | Mover plato a categoría inactiva | PUT | /api/platos/1 | 409 | 409 | OK |
| 13 | Desactivar categoría con platos disponibles | PUT | /api/categorias/2 | 409 | 409 | OK |
| 14 | Actualizar categoría inexistente | PUT | /api/categorias/999 | 404 | 404 | OK |
| 15 | Eliminar categoría con platos | DELETE | /api/categorias/2 | 409 | 409 | OK |
| 16 | Eliminar categoría inexistente | DELETE | /api/categorias/999 | 404 | 404 | OK |
| 17 | Consultar categoría desactivada | GET | /api/categorias/5 | 200 | 200 | OK |
| 18 | Eliminar categoría sin platos | DELETE | /api/categorias/5 | 204 | 204 | OK |
| 19 | Consultar categoría eliminada | GET | /api/categorias/5 | 404 | 404 | OK |
| 20 | Listar platos | GET | /api/platos | 200 | 200 | OK |
| 21 | Filtrar platos por nombre y disponibilidad | GET | /api/platos?nombre=arroz&disponible=true | 200 | 200 | OK |
| 22 | Filtrar platos por categoría y rango de precio | GET | /api/platos?categoriaId=2&precioMin=30&precioMax=50 | 200 | 200 | OK |
| 23 | Obtener plato existente | GET | /api/platos/1 | 200 | 200 | OK |
| 24 | Plato inexistente | GET | /api/platos/999 | 404 | 404 | OK |
| 25 | Crear plato | POST | /api/platos | 201 | 201 | OK |
| 26 | Crear plato duplicado en la misma categoría | POST | /api/platos | 409 | 409 | OK |
| 27 | Crear plato con precio inválido | POST | /api/platos | 400 | 400 | OK |
| 28 | Crear plato en categoría inexistente | POST | /api/platos | 409 | 409 | OK |
| 29 | Actualizar plato | PUT | /api/platos/10 | 204 | 204 | OK |
| 30 | Consultar plato actualizado | GET | /api/platos/10 | 200 | 200 | OK |
| 31 | Actualizar plato inexistente | PUT | /api/platos/999 | 404 | 404 | OK |
| 32 | Eliminar plato | DELETE | /api/platos/10 | 204 | 204 | OK |
| 33 | Consultar plato eliminado | GET | /api/platos/10 | 404 | 404 | OK |
| 34 | Eliminar plato inexistente | DELETE | /api/platos/999 | 404 | 404 | OK |

## Detalle de cada prueba

### 01. Listar categorías

Solicitud: GET /api/categorias

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:14 GMT
Server: Kestrel
Transfer-Encoding: chunked

[{"categoriaId":4,"nombre":"Bebidas","descripcion":"Bebidas frías y calientes","activo":true,"cantidadPlatos":2},{"categoriaId":1,"nombre":"Entradas","descripcion":"Platos ligeros para comenzar la comida","activo":true,"cantidadPlatos":2},{"categoriaId":2,"nombre":"Platos de fondo","descripcion":"Especialidades de la casa","activo":true,"cantidadPlatos":3},{"categoriaId":3,"nombre":"Postres","descripcion":"Dulces preparados en el día","activo":true,"cantidadPlatos":2}]
```

### 02. Filtrar categorías por nombre y activo

Solicitud: GET /api/categorias?nombre=Ent&activo=true

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:14 GMT
Server: Kestrel
Transfer-Encoding: chunked

[{"categoriaId":1,"nombre":"Entradas","descripcion":"Platos ligeros para comenzar la comida","activo":true,"cantidadPlatos":2}]
```

### 03. Obtener categoría existente

Solicitud: GET /api/categorias/1

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:14 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"categoriaId":1,"nombre":"Entradas","descripcion":"Platos ligeros para comenzar la comida","activo":true,"cantidadPlatos":2}
```

### 04. Categoría inexistente

Solicitud: GET /api/categorias/999

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:14 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe la categoría con id 999."}
```

### 05. Ruta anidada: platos de una categoría

Solicitud: GET /api/categorias/1/platos

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

[{"platoId":2,"nombre":"Causa limeña","descripcion":"Papa amarilla con pollo y mayonesa","precio":22.50,"disponible":true,"categoriaId":1,"categoriaNombre":"Entradas"},{"platoId":1,"nombre":"Ceviche clásico","descripcion":"Pescado fresco marinado en limón y ají","precio":28.90,"disponible":true,"categoriaId":1,"categoriaNombre":"Entradas"}]
```

### 06. Ruta anidada con categoría inexistente

Solicitud: GET /api/categorias/999/platos

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe la categoría con id 999."}
```

### 07. Crear categoría

Solicitud: POST /api/categorias

```json
{"nombre":"Temporada","descripcion":"Platos de temporada"}
```

Respuesta completa (código esperado 201, obtenido 201):

```text
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
Location: https://localhost:7266/api/Categorias/5
Transfer-Encoding: chunked

{"categoriaId":5,"nombre":"Temporada","descripcion":"Platos de temporada","activo":true,"cantidadPlatos":0}
```

### 08. Crear categoría duplicada

Solicitud: POST /api/categorias

```json
{"nombre":"Temporada","descripcion":"Platos de temporada"}
```

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"Ya existe una categoría registrada con el nombre 'Temporada'."}
```

### 09. Crear categoría con datos inválidos

Solicitud: POST /api/categorias

```json
{"nombre":"A","descripcion":"Nombre demasiado corto"}
```

Respuesta completa (código esperado 400, obtenido 400):

```text
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"Nombre":["El nombre debe tener entre 3 y 80 caracteres."]},"traceId":"00-d8dd0971e4d9e5150e96b31bbfaba4b5-1a9268a5467dd2ee-00"}
```

### 10. Desactivar categoría sin platos

Solicitud: PUT /api/categorias/5

```json
{"nombre":"Temporada","descripcion":"Platos de temporada","activo":false}
```

Respuesta completa (código esperado 204, obtenido 204):

```text
HTTP/1.1 204 No Content
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
```

### 11. Crear plato en categoría inactiva

Solicitud: POST /api/platos

```json
{"nombre":"Plato de prueba","descripcion":"No debe crearse","precio":15.00,"disponible":true,"categoriaId":5}
```

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:15 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No se puede registrar un plato en la categoría 'Temporada' porque está inactiva."}
```

### 12. Mover plato a categoría inactiva

Solicitud: PUT /api/platos/1

```json
{"nombre":"Ceviche clasico","descripcion":"Pescado fresco marinado en limon y aji","precio":28.90,"disponible":true,"categoriaId":5}
```

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No se puede mover el plato a la categoría 'Temporada' porque está inactiva."}
```

### 13. Desactivar categoría con platos disponibles

Solicitud: PUT /api/categorias/2

```json
{"nombre":"Platos de fondo","descripcion":"Especialidades de la casa","activo":false}
```

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No se puede desactivar la categoría 'Platos de fondo' porque tiene 3 plato(s) disponible(s)."}
```

### 14. Actualizar categoría inexistente

Solicitud: PUT /api/categorias/999

```json
{"nombre":"Categoría inexistente","descripcion":"Prueba","activo":true}
```

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe la categoría con id 999."}
```

### 15. Eliminar categoría con platos

Solicitud: DELETE /api/categorias/2

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No se puede eliminar la categoría porque tiene 3 plato(s) registrado(s)."}
```

### 16. Eliminar categoría inexistente

Solicitud: DELETE /api/categorias/999

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe la categoría con id 999."}
```

### 17. Consultar categoría desactivada

Solicitud: GET /api/categorias/5

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"categoriaId":5,"nombre":"Temporada","descripcion":"Platos de temporada","activo":false,"cantidadPlatos":0}
```

### 18. Eliminar categoría sin platos

Solicitud: DELETE /api/categorias/5

Respuesta completa (código esperado 204, obtenido 204):

```text
HTTP/1.1 204 No Content
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
```

### 19. Consultar categoría eliminada

Solicitud: GET /api/categorias/5

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe la categoría con id 5."}
```

### 20. Listar platos

Solicitud: GET /api/platos

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

[{"platoId":4,"nombre":"Ají de gallina","descripcion":"Pollo deshilachado en crema de ají amarillo","precio":34.00,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"},{"platoId":6,"nombre":"Arroz con leche","descripcion":"Postre tradicional con canela","precio":12.00,"disponible":true,"categoriaId":3,"categoriaNombre":"Postres"},{"platoId":5,"nombre":"Arroz con mariscos","descripcion":"Arroz meloso con mariscos y culantro","precio":46.50,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"},{"platoId":2,"nombre":"Causa limeña","descripcion":"Papa amarilla con pollo y mayonesa","precio":22.50,"disponible":true,"categoriaId":1,"categoriaNombre":"Entradas"},{"platoId":1,"nombre":"Ceviche clásico","descripcion":"Pescado fresco marinado en limón y ají","precio":28.90,"disponible":true,"categoriaId":1,"categoriaNombre":"Entradas"},{"platoId":8,"nombre":"Chicha morada","descripcion":"Bebida de maíz morado con frutas","precio":8.00,"disponible":true,"categoriaId":4,"categoriaNombre":"Bebidas"},{"platoId":7,"nombre":"Crema volteada","descripcion":"Flan de leche con caramelo","precio":13.50,"disponible":true,"categoriaId":3,"categoriaNombre":"Postres"},{"platoId":9,"nombre":"Limonada frozen","descripcion":"Limón, azúcar y hielo frappé","precio":9.50,"disponible":true,"categoriaId":4,"categoriaNombre":"Bebidas"},{"platoId":3,"nombre":"Lomo saltado","descripcion":"Res salteada con cebolla, tomate y papas fritas","precio":39.90,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"}]
```

### 21. Filtrar platos por nombre y disponibilidad

Solicitud: GET /api/platos?nombre=arroz&disponible=true

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

[{"platoId":6,"nombre":"Arroz con leche","descripcion":"Postre tradicional con canela","precio":12.00,"disponible":true,"categoriaId":3,"categoriaNombre":"Postres"},{"platoId":5,"nombre":"Arroz con mariscos","descripcion":"Arroz meloso con mariscos y culantro","precio":46.50,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"}]
```

### 22. Filtrar platos por categoría y rango de precio

Solicitud: GET /api/platos?categoriaId=2&precioMin=30&precioMax=50

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

[{"platoId":4,"nombre":"Ají de gallina","descripcion":"Pollo deshilachado en crema de ají amarillo","precio":34.00,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"},{"platoId":5,"nombre":"Arroz con mariscos","descripcion":"Arroz meloso con mariscos y culantro","precio":46.50,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"},{"platoId":3,"nombre":"Lomo saltado","descripcion":"Res salteada con cebolla, tomate y papas fritas","precio":39.90,"disponible":true,"categoriaId":2,"categoriaNombre":"Platos de fondo"}]
```

### 23. Obtener plato existente

Solicitud: GET /api/platos/1

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"platoId":1,"nombre":"Ceviche clásico","descripcion":"Pescado fresco marinado en limón y ají","precio":28.90,"disponible":true,"categoriaId":1,"categoriaNombre":"Entradas"}
```

### 24. Plato inexistente

Solicitud: GET /api/platos/999

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:16 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe el plato con id 999."}
```

### 25. Crear plato

Solicitud: POST /api/platos

```json
{"nombre":"Tiradito de prueba","descripcion":"Plato creado para las pruebas","precio":25.50,"disponible":true,"categoriaId":1}
```

Respuesta completa (código esperado 201, obtenido 201):

```text
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Location: https://localhost:7266/api/Platos/10
Transfer-Encoding: chunked

{"platoId":10,"nombre":"Tiradito de prueba","descripcion":"Plato creado para las pruebas","precio":25.50,"disponible":true,"categoriaId":1,"categoriaNombre":"Entradas"}
```

### 26. Crear plato duplicado en la misma categoría

Solicitud: POST /api/platos

```json
{"nombre":"Tiradito de prueba","descripcion":"Plato creado para las pruebas","precio":25.50,"disponible":true,"categoriaId":1}
```

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"Ya existe un plato llamado 'Tiradito de prueba' en la categoría 'Entradas'."}
```

### 27. Crear plato con precio inválido

Solicitud: POST /api/platos

```json
{"nombre":"Precio invalido","precio":0,"disponible":true,"categoriaId":1}
```

Respuesta completa (código esperado 400, obtenido 400):

```text
HTTP/1.1 400 Bad Request
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"Precio":["El precio debe estar entre 1.00 y 500.00 soles."]},"traceId":"00-90480bf4d0f22eb89554579a474ce7f2-9f286fa6f641ba08-00"}
```

### 28. Crear plato en categoría inexistente

Solicitud: POST /api/platos

```json
{"nombre":"Plato huerfano","precio":10.00,"disponible":true,"categoriaId":999}
```

Respuesta completa (código esperado 409, obtenido 409):

```text
HTTP/1.1 409 Conflict
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"La categoría con id 999 no existe."}
```

### 29. Actualizar plato

Solicitud: PUT /api/platos/10

```json
{"nombre":"Tiradito actualizado","descripcion":"Actualizado en la prueba","precio":27.00,"disponible":false,"categoriaId":1}
```

Respuesta completa (código esperado 204, obtenido 204):

```text
HTTP/1.1 204 No Content
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
```

### 30. Consultar plato actualizado

Solicitud: GET /api/platos/10

Respuesta completa (código esperado 200, obtenido 200):

```text
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"platoId":10,"nombre":"Tiradito actualizado","descripcion":"Actualizado en la prueba","precio":27.00,"disponible":false,"categoriaId":1,"categoriaNombre":"Entradas"}
```

### 31. Actualizar plato inexistente

Solicitud: PUT /api/platos/999

```json
{"nombre":"Tiradito actualizado","descripcion":"Actualizado en la prueba","precio":27.00,"disponible":false,"categoriaId":1}
```

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe el plato con id 999."}
```

### 32. Eliminar plato

Solicitud: DELETE /api/platos/10

Respuesta completa (código esperado 204, obtenido 204):

```text
HTTP/1.1 204 No Content
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
```

### 33. Consultar plato eliminado

Solicitud: GET /api/platos/10

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe el plato con id 10."}
```

### 34. Eliminar plato inexistente

Solicitud: DELETE /api/platos/999

Respuesta completa (código esperado 404, obtenido 404):

```text
HTTP/1.1 404 Not Found
Content-Type: application/json; charset=utf-8
Date: Thu, 12 Nov 2026 00:01:17 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"mensaje":"No existe el plato con id 999."}
```


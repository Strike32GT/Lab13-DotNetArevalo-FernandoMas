# ============================================================
# Pruebas automaticas de la API Restaurante (todos los codigos)
# Uso:  powershell -ExecutionPolicy Bypass -File .\scripts\probar-api.ps1
# Requiere la API en ejecucion en https://localhost:7266
# ============================================================
param(
    [string]$BaseUrl = "https://localhost:7266",
    [string]$CarpetaEvidencias = ""
)

$ErrorActionPreference = "Stop"

if ($CarpetaEvidencias -eq "") {
    $CarpetaEvidencias = Join-Path $PSScriptRoot "..\docs\evidencias"
}

$CarpetaEvidencias = [System.IO.Path]::GetFullPath($CarpetaEvidencias)
$CarpetaDocs = Split-Path $CarpetaEvidencias -Parent
New-Item -ItemType Directory -Force -Path $CarpetaEvidencias | Out-Null

$resultados = @()
$numero = 0

function Enviar-Pruba {
    param(
        [string]$Caso,
        [string]$Metodo,
        [string]$Ruta,
        [string]$Cuerpo = "",
        [int]$Esperado
    )

    $script:numero = $script:numero + 1
    $n = $script:numero.ToString("00")
    $archivo = Join-Path $CarpetaEvidencias ("{0}-{1}.txt" -f $n, ($Caso -replace "[^a-zA-Z0-9\-]", "-"))
    $url = "$BaseUrl$Ruta"

    $argumentos = @("-k", "-s", "-i", "-X", $Metodo, $url, "-o", $archivo)

    if ($Cuerpo -ne "") {
        $temporal = Join-Path $env:TEMP ("restaurante-cuerpo-{0}.json" -f $n)
        [System.IO.File]::WriteAllText($temporal, $Cuerpo, (New-Object System.Text.UTF8Encoding($false)))
        $argumentos += @("-H", "Content-Type: application/json", "--data-binary", "@$temporal")
    }

    & curl.exe @argumentos | Out-Null

    $contenido = Get-Content $archivo -Raw -Encoding UTF8
    $codigo = 0
    $coincidencias = [regex]::Matches($contenido, "(?m)^HTTP/\S+\s+(\d{3})")

    if ($coincidencias.Count -gt 0) {
        $codigo = [int]$coincidencias[$coincidencias.Count - 1].Groups[1].Value
    }

    $ok = ($codigo -eq $Esperado)

    $script:resultados += [pscustomobject]@{
        Numero    = $n
        Caso      = $Caso
        Metodo    = $Metodo
        Ruta      = $Ruta
        Cuerpo    = $Cuerpo
        Esperado  = $Esperado
        Obtenido  = $codigo
        Resultado = $(if ($ok) { "OK" } else { "FALLO" })
        Archivo   = $archivo
    }

    if ($ok) {
        Write-Host ("OK    {0}  {1} {2} -> {3}" -f $n, $Metodo, $Ruta, $codigo) -ForegroundColor Green
    }
    else {
        Write-Host ("FALLO {0}  {1} {2} -> {3} (esperado {4})" -f $n, $Metodo, $Ruta, $codigo, $Esperado) -ForegroundColor Red
    }
}

# ------------------------------------------------------------
# Categorias: GET 200, filtros y 404
# ------------------------------------------------------------
Enviar-Pruba -Caso "Listar categorías" -Metodo "GET" -Ruta "/api/categorias" -Esperado 200
Enviar-Pruba -Caso "Filtrar categorías por nombre y activo" -Metodo "GET" -Ruta "/api/categorias?nombre=Ent&activo=true" -Esperado 200
Enviar-Pruba -Caso "Obtener categoría existente" -Metodo "GET" -Ruta "/api/categorias/1" -Esperado 200
Enviar-Pruba -Caso "Categoría inexistente" -Metodo "GET" -Ruta "/api/categorias/999" -Esperado 404

# Ruta anidada
Enviar-Pruba -Caso "Ruta anidada: platos de una categoría" -Metodo "GET" -Ruta "/api/categorias/1/platos" -Esperado 200
Enviar-Pruba -Caso "Ruta anidada con categoría inexistente" -Metodo "GET" -Ruta "/api/categorias/999/platos" -Esperado 404

# ------------------------------------------------------------
# Categorias: POST 201, 400 y 409
# ------------------------------------------------------------
$categoriaNueva = '{"nombre":"Temporada","descripcion":"Platos de temporada"}'
Enviar-Pruba -Caso "Crear categoría" -Metodo "POST" -Ruta "/api/categorias" -Cuerpo $categoriaNueva -Esperado 201

Enviar-Pruba -Caso "Crear categoría duplicada" -Metodo "POST" -Ruta "/api/categorias" -Cuerpo $categoriaNueva -Esperado 409

$categoriaInvalida = '{"nombre":"A","descripcion":"Nombre demasiado corto"}'
Enviar-Pruba -Caso "Crear categoría con datos inválidos" -Metodo "POST" -Ruta "/api/categorias" -Cuerpo $categoriaInvalida -Esperado 400

# ------------------------------------------------------------
# Categorias: PUT 204, 404 y 409
# ------------------------------------------------------------
$categoriaDesactivada = '{"nombre":"Temporada","descripcion":"Platos de temporada","activo":false}'
Enviar-Pruba -Caso "Desactivar categoría sin platos" -Metodo "PUT" -Ruta "/api/categorias/5" -Cuerpo $categoriaDesactivada -Esperado 204

$platoEnCategoriaInactiva = '{"nombre":"Plato de prueba","descripcion":"No debe crearse","precio":15.00,"disponible":true,"categoriaId":5}'
Enviar-Pruba -Caso "Crear plato en categoría inactiva" -Metodo "POST" -Ruta "/api/platos" -Cuerpo $platoEnCategoriaInactiva -Esperado 409

$moverPlatoACategoriaInactiva = '{"nombre":"Ceviche clasico","descripcion":"Pescado fresco marinado en limon y aji","precio":28.90,"disponible":true,"categoriaId":5}'
Enviar-Pruba -Caso "Mover plato a categoría inactiva" -Metodo "PUT" -Ruta "/api/platos/1" -Cuerpo $moverPlatoACategoriaInactiva -Esperado 409

$categoriaConPlatos = '{"nombre":"Platos de fondo","descripcion":"Especialidades de la casa","activo":false}'
Enviar-Pruba -Caso "Desactivar categoría con platos disponibles" -Metodo "PUT" -Ruta "/api/categorias/2" -Cuerpo $categoriaConPlatos -Esperado 409

$categoriaInexistente = '{"nombre":"Categoría inexistente","descripcion":"Prueba","activo":true}'
Enviar-Pruba -Caso "Actualizar categoría inexistente" -Metodo "PUT" -Ruta "/api/categorias/999" -Cuerpo $categoriaInexistente -Esperado 404

# ------------------------------------------------------------
# Categorias: DELETE 204, 404 y 409
# ------------------------------------------------------------
Enviar-Pruba -Caso "Eliminar categoría con platos" -Metodo "DELETE" -Ruta "/api/categorias/2" -Esperado 409
Enviar-Pruba -Caso "Eliminar categoría inexistente" -Metodo "DELETE" -Ruta "/api/categorias/999" -Esperado 404
Enviar-Pruba -Caso "Consultar categoría desactivada" -Metodo "GET" -Ruta "/api/categorias/5" -Esperado 200
Enviar-Pruba -Caso "Eliminar categoría sin platos" -Metodo "DELETE" -Ruta "/api/categorias/5" -Esperado 204
Enviar-Pruba -Caso "Consultar categoría eliminada" -Metodo "GET" -Ruta "/api/categorias/5" -Esperado 404

# ------------------------------------------------------------
# Platos: GET 200, filtros y 404
# ------------------------------------------------------------
Enviar-Pruba -Caso "Listar platos" -Metodo "GET" -Ruta "/api/platos" -Esperado 200
Enviar-Pruba -Caso "Filtrar platos por nombre y disponibilidad" -Metodo "GET" -Ruta "/api/platos?nombre=arroz&disponible=true" -Esperado 200
Enviar-Pruba -Caso "Filtrar platos por categoría y rango de precio" -Metodo "GET" -Ruta "/api/platos?categoriaId=2&precioMin=30&precioMax=50" -Esperado 200
Enviar-Pruba -Caso "Obtener plato existente" -Metodo "GET" -Ruta "/api/platos/1" -Esperado 200
Enviar-Pruba -Caso "Plato inexistente" -Metodo "GET" -Ruta "/api/platos/999" -Esperado 404

# ------------------------------------------------------------
# Platos: POST 201, 400 y 409
# ------------------------------------------------------------
$platoNuevo = '{"nombre":"Tiradito de prueba","descripcion":"Plato creado para las pruebas","precio":25.50,"disponible":true,"categoriaId":1}'
Enviar-Pruba -Caso "Crear plato" -Metodo "POST" -Ruta "/api/platos" -Cuerpo $platoNuevo -Esperado 201

Enviar-Pruba -Caso "Crear plato duplicado en la misma categoría" -Metodo "POST" -Ruta "/api/platos" -Cuerpo $platoNuevo -Esperado 409

$platoInvalido = '{"nombre":"Precio invalido","precio":0,"disponible":true,"categoriaId":1}'
Enviar-Pruba -Caso "Crear plato con precio inválido" -Metodo "POST" -Ruta "/api/platos" -Cuerpo $platoInvalido -Esperado 400

$platoCategoriaInexistente = '{"nombre":"Plato huerfano","precio":10.00,"disponible":true,"categoriaId":999}'
Enviar-Pruba -Caso "Crear plato en categoría inexistente" -Metodo "POST" -Ruta "/api/platos" -Cuerpo $platoCategoriaInexistente -Esperado 409

# ------------------------------------------------------------
# Platos: PUT 204 y 404
# ------------------------------------------------------------
$platoActualizado = '{"nombre":"Tiradito actualizado","descripcion":"Actualizado en la prueba","precio":27.00,"disponible":false,"categoriaId":1}'
Enviar-Pruba -Caso "Actualizar plato" -Metodo "PUT" -Ruta "/api/platos/10" -Cuerpo $platoActualizado -Esperado 204
Enviar-Pruba -Caso "Consultar plato actualizado" -Metodo "GET" -Ruta "/api/platos/10" -Esperado 200
Enviar-Pruba -Caso "Actualizar plato inexistente" -Metodo "PUT" -Ruta "/api/platos/999" -Cuerpo $platoActualizado -Esperado 404

# ------------------------------------------------------------
# Platos: DELETE 204 y 404
# ------------------------------------------------------------
Enviar-Pruba -Caso "Eliminar plato" -Metodo "DELETE" -Ruta "/api/platos/10" -Esperado 204
Enviar-Pruba -Caso "Consultar plato eliminado" -Metodo "GET" -Ruta "/api/platos/10" -Esperado 404
Enviar-Pruba -Caso "Eliminar plato inexistente" -Metodo "DELETE" -Ruta "/api/platos/999" -Esperado 404

# ------------------------------------------------------------
# Resumen en Markdown
# ------------------------------------------------------------
$total = $resultados.Count
$aprobadas = ($resultados | Where-Object { $_.Resultado -eq "OK" }).Count
$fallidas = $total - $aprobadas
$fecha = Get-Date -Format "yyyy-MM-dd HH:mm"

$md = New-Object System.Text.StringBuilder
[void]$md.AppendLine("# Pruebas de la API Restaurante")
[void]$md.AppendLine("")
[void]$md.AppendLine("Fecha de ejecución: $fecha")
[void]$md.AppendLine("")
[void]$md.AppendLine("URL base: $BaseUrl")
[void]$md.AppendLine("")
[void]$md.AppendLine("Resultado general: $aprobadas de $total pruebas aprobadas, $fallidas con fallo.")
[void]$md.AppendLine("")
[void]$md.AppendLine("| # | Caso | Método | Ruta | Esperado | Obtenido | Resultado |")
[void]$md.AppendLine("| :-- | :---- | :----- | :--- | :------- | :------- | :-------- |")

foreach ($r in $resultados) {
    [void]$md.AppendLine("| $($r.Numero) | $($r.Caso) | $($r.Metodo) | $($r.Ruta) | $($r.Esperado) | $($r.Obtenido) | $($r.Resultado) |")
}

[void]$md.AppendLine("")
[void]$md.AppendLine("## Detalle de cada prueba")
[void]$md.AppendLine("")

foreach ($r in $resultados) {
    [void]$md.AppendLine("### $($r.Numero). $($r.Caso)")
    [void]$md.AppendLine("")

    if ($r.Cuerpo -ne "") {
        [void]$md.AppendLine("Solicitud: $($r.Metodo) $($r.Ruta)")
        [void]$md.AppendLine("")
        [void]$md.AppendLine('```json')
        [void]$md.AppendLine($r.Cuerpo)
        [void]$md.AppendLine('```')
    }
    else {
        [void]$md.AppendLine("Solicitud: $($r.Metodo) $($r.Ruta)")
    }

    [void]$md.AppendLine("")
    [void]$md.AppendLine("Respuesta completa (código esperado $($r.Esperado), obtenido $($r.Obtenido)):")
    [void]$md.AppendLine("")
    [void]$md.AppendLine('```text')

    $respuesta = Get-Content $r.Archivo -Raw -Encoding UTF8
    [void]$md.AppendLine($respuesta.TrimEnd())

    [void]$md.AppendLine('```')
    [void]$md.AppendLine("")
}

$archivoResumen = Join-Path $CarpetaDocs "PRUEBAS.md"
[System.IO.File]::WriteAllText($archivoResumen, $md.ToString(), (New-Object System.Text.UTF8Encoding($false)))

Write-Host ""
Write-Host "Resumen: $aprobadas/$total pruebas aprobadas."
Write-Host "Evidencias: $CarpetaEvidencias"
Write-Host "Documento: $archivoResumen"

if ($fallidas -gt 0) {
    exit 1
}

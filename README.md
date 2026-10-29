# DSW_3P_EJ01 — QuestLog API (etapa JSON)

Proyecto base para la primera hora del tercer parcial de Desarrollo de Servicios Web.

## Requisitos
- .NET SDK 10
- Visual Studio 2022 compatible con .NET 10 o VS Code con C# Dev Kit
- Postman o cliente HTTP compatible con archivos `.http`
- Git y cuenta GitHub para la práctica de control de versiones

## Ejecutar
```bash
dotnet restore
dotnet run --launch-profile http
```
La API estará en `http://localhost:5186`.

## Probar
Abrir `QuestLog.http` en un cliente HTTP compatible o utilizar Postman.

| Método | Ruta | Respuesta esperada |
|---|---|---|
| GET | `/api/misiones` | 200 |
| GET | `/api/misiones/1` | 200 o 404 |
| POST | `/api/misiones` | 201 o 400 |
| PUT | `/api/misiones/1` | 204, 400 o 404 |
| DELETE | `/api/misiones/1` | 204 o 404 |

**Ejemplo de POST:**
```json
{
  "nombre": "Rescatar al explorador",
  "dificultad": "Media",
  "recompensaXP": 250,
  "completada": true
}
```
La misión se registra con `completada = false`, aunque el cliente envíe `true`, porque es una regla de negocio del Service.

## Arquitectura
`MisionesController → IMisionService / MisionService → IMisionRepository / MisionJsonRepository → Data/misiones.json`

## Preparación para la segunda hora (NO implementada)
1. Instalar Entity Framework Core y proveedor de SQL Server.
2. Crear `AppDbContext` y su `DbSet<Mision>`.
3. Configurar la conexión a SQL Server.
4. Crear `MisionSqlRepository : IMisionRepository`.
5. Crear y aplicar la migración.
6. Cambiar en `Program.cs` el registro `IMisionRepository` de `MisionJsonRepository` a `MisionSqlRepository`.
7. Repetir las pruebas CRUD.

**No modificar** `MisionesController`, `MisionService` ni `IMisionRepository` durante el cambio de persistencia.

## Git
```bash
git init
git add .
git commit -m "feat: implementar QuestLog con persistencia JSON"
```
Después de completar SQL Server en clase, realizar un segundo commit.

## Nota
El proyecto incluye el documento OpenAPI en `/openapi/v1.json` cuando se ejecuta en Development. No incluye interfaz Swagger UI para evitar dependencias innecesarias. Las pruebas pueden realizarse desde Postman o `QuestLog.http`.

El almacenamiento JSON es deliberadamente simple para uso educativo y no maneja escrituras concurrentes; no está pensado para producción.

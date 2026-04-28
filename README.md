#  PoliRiwi

**Sistema de gestión de reservas de espacios deportivos** — Aplicación web construida con ASP.NET Core MVC y MySQL, orientada a la administración de usuarios, lugares y reservas en instalaciones polideportivas.

---

##  Tabla de Contenidos

- [Descripción](#descripción)
- [Tecnologías](#tecnologías)
- [Arquitectura del Proyecto](#arquitectura-del-proyecto)
- [Modelos de Datos](#modelos-de-datos)
- [Requisitos Previos](#requisitos-previos)
- [Instalación y Configuración](#instalación-y-configuración)
- [Endpoints / Rutas](#endpoints--rutas)
- [Validaciones](#validaciones)
- [Casos de Uso](#casos-de-uso)
- [Convenciones del Proyecto](#convenciones-del-proyecto)

---

## Descripción

PoliRiwi permite gestionar el ciclo completo de reservas en un complejo deportivo: registro de usuarios, alta y control de espacios (canchas, piscinas, etc.) y creación/seguimiento de reservas con control de disponibilidad por estados. La interfaz utiliza un diseño dark moderno con Bootstrap 5 e íconos Bootstrap Icons.

---

## Tecnologías

| Capa | Tecnología |
|---|---|
| **Framework** | ASP.NET Core MVC — .NET 10 |
| **ORM** | Entity Framework Core 9 + Pomelo (MySQL) |
| **Base de Datos** | MySQL |
| **Validaciones** | FluentValidation 12.1 |
| **Frontend** | Razor Views + Bootstrap 5 + Bootstrap Icons |
| **IDE recomendado** | JetBrains Rider / Visual Studio 2022 |

---

## Arquitectura del Proyecto

```
PoliRiwi/
├── Controllers/
│   ├── HomeController.cs          # Rutas de inicio y error
│   └── UserController.cs          # CRUD de usuarios, lugares y reservas
├── Models/
│   ├── BaseEntity.cs              # Clase base: Id, CreatedAt, UpdatedAt
│   ├── Users.cs                   # Modelo de usuario
│   ├── Places.cs                  # Modelo de lugar/espacio deportivo
│   └── Reservations.cs            # Modelo de reserva
├── Services/
│   ├── Interfaces/
│   │   ├── IUserService.cs
│   │   ├── IPlaceServices.cs
│   │   └── IReservationService.cs
│   ├── UserService.cs
│   ├── PlaceService.cs
│   └── ReservationService.cs
├── Validators/
│   ├── UserValidators.cs
│   ├── PlaceValidators.cs
│   └── ReservationValidatiors.cs
├── Data/
│   └── MySqlDbContext.cs          # DbContext con relaciones Fluent API
├── Enums/
│   ├── SpaceType.cs               # Tipos de espacio deportivo
│   └── Status.cs                  # Estados de lugares y reservas
├── Response/
│   └── ServiceResponse.cs         # Wrapper genérico de respuesta
├── Views/
│   ├── User/
│   │   ├── Users.cshtml           # IDENTITY MANAGEMENT
│   │   ├── Places.cshtml          # LOCATION MANAGEMENT
│   │   └── Reservations.cshtml    # Live Reservations
│   └── Shared/
│       └── _Layout.cshtml
├── Program.cs
├── appsettings.json
└── PoliRiwi.csproj
```

---

## Modelos de Datos

### BaseEntity
Clase abstracta heredada por todos los modelos principales.

| Propiedad | Tipo | Descripción |
|---|---|---|
| `Id` | `int` | Clave primaria |
| `CreatedAt` | `DateTime` | Fecha de creación |
| `UpdatedAt` | `DateTime` | Fecha de última modificación |

---

### Users
| Propiedad | Tipo | Descripción |
|---|---|---|
| `Name` | `string` | Nombre completo |
| `Email` | `string` | Correo electrónico |
| `Document` | `string` | Número de documento |
| `Phone` | `string` | Teléfono de contacto |
| `Reservations` | `ICollection<Reservations>` | Reservas asociadas |

---

### Places
| Propiedad | Tipo | Descripción |
|---|---|---|
| `Name` | `string` | Nombre del lugar |
| `SpaceType` | `SpaceType` | Tipo de espacio (enum) |
| `Capacity` | `int` | Capacidad máxima |
| `Status` | `Status` | Estado del lugar (enum) |
| `Reservations` | `ICollection<Reservations>` | Reservas asociadas |

---

### Reservations
| Propiedad | Tipo | Descripción |
|---|---|---|
| `UserId` | `int` | FK → Users |
| `PlaceId` | `int` | FK → Places |
| `Status` | `Status` | Estado de la reserva |
| `Date` | `DateTime` | Fecha de la reserva |
| `StartTime` | `TimeOnly` | Hora de inicio |
| `EndTime` | `TimeOnly` | Hora de fin |

---

### Enums

**SpaceType**
```csharp
Soccer | Basketball | Pool | Volleyball | Rugby | Tennis | Golf
```

**Status**
```csharp
Unavailable | Available | Reserved | Cancelled | Finished
```

---

## Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MySQL 8.0 o superior
- JetBrains Rider 2024+ o Visual Studio 2022

---

## Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/PoliRiwi.git
cd PoliRiwi
```

### 2. Configurar la cadena de conexión

Editar `appsettings.json` con los datos de tu servidor MySQL:

```json
{
  "ConnectionStrings": {
    "MySqlConnection": "Server=localhost;Database=poliriwi_db;User Id=root;Password=tu_password;"
  }
}
```

>  **No subas credenciales reales al repositorio.** Usa `appsettings.Development.json` o variables de entorno para datos sensibles.

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Aplicar migraciones (si aplica)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Ejecutar el proyecto

```bash
dotnet run
```

La aplicación estará disponible en `https://localhost:5001` / `http://localhost:5000`.

---

## Endpoints / Rutas

La ruta por defecto configurada en `Program.cs` es:

```
{controller=User}/{action=Users}/{id?}
```

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/User/Users` | Lista todos los usuarios |
| `POST` | `/User/CreateUser` | Crea un nuevo usuario |
| `POST` | `/User/UpdateUser` | Actualiza un usuario existente |
| `POST` | `/User/DeleteUser` | Elimina un usuario por Id |
| `GET` | `/User/Places` | Vista de lugares (Location Management) |
| `POST` | `/User/CreatePlace` | Crea un nuevo lugar deportivo |
| `GET` | `/User/GetAllPlaces` | Obtiene todos los lugares (JSON/View) |
| `GET` | `/User/Reservations` | Vista de reservas activas |

---

## Validaciones

Las validaciones están implementadas con **FluentValidation** e inyectadas en el contenedor de dependencias.

### UserValidators
- `Name` → No puede ser vacío
- `Email` → No puede ser vacío
- `Phone` → No puede ser vacío

### PlaceValidators
- `Name` → No puede ser vacío
- `Status` → Debe ser un valor válido del enum `Status`
- `SpaceType` → Debe ser un valor válido del enum `SpaceType`

### ReservationValidators
- `Date` → Debe ser mayor a hoy (no fechas pasadas)
- `EndTime` → Debe ser mayor a `StartTime`
- `Status` → Debe ser un valor válido del enum `Status`

---

## Casos de Uso

### CU-01 — Gestión de Usuarios

| Campo | Detalle |
|---|---|
| **Actor** | Administrador / Operador |
| **Descripción** | Permite registrar, consultar, actualizar y eliminar usuarios del sistema. |
| **Precondición** | El sistema debe tener conexión activa a la base de datos MySQL. |

**Flujo Principal**
1. El usuario accede a la vista *Identity Management* (`/User/Users`).
2. Consulta el listado paginado de usuarios registrados.
3. Puede crear un nuevo usuario mediante el modal *New Entry*.
4. Completa nombre, email, documento y teléfono.
5. FluentValidation verifica los campos antes de persistir.
6. El sistema guarda y redirige al listado actualizado.

**Flujos Alternativos**
- **FA-01:** Si la validación falla, se muestra el mensaje de error correspondiente sin guardar.
- **FA-02:** Si el usuario tiene reservas activas, el sistema puede impedir la eliminación.

**Postcondición** → El usuario queda registrado/actualizado en la tabla `Users`.

---

### CU-02 — Gestión de Lugares Deportivos

| Campo | Detalle |
|---|---|
| **Actor** | Administrador |
| **Descripción** | Permite registrar y administrar los espacios deportivos disponibles (canchas, piscinas, etc.) con su tipo, capacidad y estado. |
| **Precondición** | El administrador debe tener acceso al módulo *Location Management*. |

**Flujo Principal**
1. El administrador accede a `/User/Places`.
2. Visualiza el panel de métricas (zonas activas, carga del sistema).
3. Selecciona *New Place* para abrir el modal de creación.
4. Ingresa nombre, tipo de espacio (`SpaceType`), capacidad y estado (`Status`).
5. El sistema valida los datos con `PlaceValidators`.
6. El lugar queda registrado y visible en la tabla.

**Flujos Alternativos**
- **FA-01:** Si el `SpaceType` o `Status` no son valores válidos del enum, la validación rechaza la operación.

**Postcondición** → El lugar queda registrado en la tabla `Places` con su estado inicial.

---

### CU-03 — Gestión de Reservas

| Campo | Detalle |
|---|---|
| **Actor** | Usuario / Operador |
| **Descripción** | Permite crear y consultar reservas de espacios deportivos, asociando un usuario, un lugar, fecha y horario. |
| **Precondición** | Deben existir usuarios y lugares registrados en el sistema. |

**Flujo Principal**
1. El operador accede a la vista *Reservations* (`/User/Reservations`).
2. Consulta las reservas activas del día.
3. Selecciona crear una nueva reserva indicando: usuario, lugar, fecha, hora inicio y hora fin.
4. El sistema valida que la fecha no sea pasada y que `EndTime > StartTime`.
5. Se asigna el estado inicial `Reserved` y se persiste.
6. La reserva aparece en el panel de *Live Reservations*.

**Flujos Alternativos**
- **FA-01:** Si la fecha es anterior a hoy, la validación rechaza la operación con mensaje *"Can not do reservations for yesterday"*.
- **FA-02:** Si `EndTime <= StartTime`, se muestra *"Start time cannot be in the future"*.
- **FA-03:** Si el lugar tiene estado `Unavailable`, no puede recibir reservas.

**Postcondición** → La reserva queda registrada en `Reservations` con estado `Reserved` y las FKs de usuario y lugar vinculadas correctamente.

---

### CU-04 — Consulta de Disponibilidad

| Campo | Detalle |
|---|---|
| **Actor** | Usuario / Operador |
| **Descripción** | Permite verificar el estado actual de los lugares para determinar disponibilidad antes de generar una reserva. |
| **Precondición** | Deben existir lugares registrados con estado asignado. |

**Flujo Principal**
1. El operador consulta la vista de *Location Management*.
2. Filtra o busca el lugar de interés.
3. Verifica el campo `Status` del lugar (`Available`, `Unavailable`, `Reserved`).
4. Si está disponible, procede a crear la reserva (CU-03).

**Postcondición** → El operador obtiene información de disponibilidad en tiempo real para tomar decisiones.

---

## Convenciones del Proyecto

- **Arquitectura:** Separación por capas — Controllers → Services (con interfaces) → Data (EF Core).
- **Patrón de respuesta:** Todas las respuestas de servicio se envuelven en `ServiceResponse<T>` con `Data`, `Message` e `IsSuccess`.
- **Relaciones de BD:** Definidas con Fluent API en `MySqlDbContext.OnModelCreating` (no con Data Annotations).
- **Colecciones:** Se usa `ICollection<T>` en lugar de `IEnumerable<T>` en las propiedades de navegación para permitir lectura y modificación eficiente.
- **Timestamps:** `CreatedAt` y `UpdatedAt` se asignan en el Controller antes de persistir (`DateTime.Now`).
- **Enums:** Usados para `SpaceType` y `Status` para garantizar integridad de datos sin valores arbitrarios.

---

*PoliRiwi · ASP.NET Core 10 · MySQL · FluentValidation*

# Finanzas Personales .NET

## Descripción
Aplicación de consola para gestión de finanzas personales desarrollada en .NET 8.0 con Entity Framework Core y SQLite.

## Características
- ✅ **Gestión de Usuarios**: Registro e inicio de sesión con autenticación
- ✅ **Movimientos Financieros**: Registro de ingresos y gastos con categorización
- ✅ **Metas de Ahorro**: Crear y gestionar objetivos de ahorro
- ✅ **Inversiones**: Simulación de inversiones con cálculo de rendimientos
- ✅ **Estadísticas**: Resúmenes financieros y análisis por categorías
- ✅ **Base de Datos**: Persistencia con SQLite y Entity Framework Core
- ✅ **Interfaz Intuitiva**: Menús de consola coloridos y fáciles de usar

## Arquitectura
El proyecto sigue el patrón MVC (Model-View-Controller) con inyección de dependencias:

### Modelos (Models)
- `Usuario`: Gestión de usuarios y autenticación
- `Movimiento`: Clase base abstracta para transacciones financieras
- `Ingreso`: Ingresos de dinero
- `Gasto`: Gastos con categorización
- `Ahorro`: Metas de ahorro con seguimiento de progreso
- `Inversion`: Simulación de inversiones con cálculos de rendimiento

### Servicios (Services)
- `UsuarioService`: Autenticación y gestión de usuarios
- `MovimientoService`: Lógica de negocio para ingresos y gastos
- `AhorroService`: Gestión de metas de ahorro
- `InversionService`: Cálculos de inversiones
- `EstadisticaService`: Generación de reportes y estadísticas

### Controladores (Controllers)
- Actúan como intermediarios entre las vistas y los servicios
- Incluyen validación de datos y manejo de errores
- Implementación asíncrona para mejor rendimiento

### Vistas (Views)
- Interfaces de consola interactivas
- Menús coloridos con validación de entrada
- Manejo de errores amigable al usuario

### Persistencia (Data)
- `FinanzasDbContext`: Contexto de Entity Framework Core
- Configuración automática de base de datos SQLite
- Migraciones automáticas

## Requisitos del Sistema
- .NET 8.0 SDK
- Windows, Linux o macOS
- SQLite (incluido con .NET)

## Instalación

1. **Clonar o descargar el proyecto**

2. **Instalar .NET 8.0 SDK**
   ```bash
   # Windows: Descargar desde https://dotnet.microsoft.com/download
   # Linux (Ubuntu):
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-8.0

   # macOS:
   brew install dotnet
   ```

3. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

4. **Compilar el proyecto**
   ```bash
   dotnet build
   ```

## Uso

1. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```

2. **Primera vez**
   - Registrar un nuevo usuario
   - Iniciar sesión
   - Explorar las funcionalidades del menú

3. **Funcionalidades principales**
   - **Movimientos**: Agregar ingresos y gastos
   - **Ahorros**: Crear metas y hacer depósitos/retiros
   - **Inversiones**: Simular inversiones y ver rendimientos
   - **Estadísticas**: Ver resúmenes y análisis financieros

## Estructura del Proyecto
```
FinanzasPersonales.NET/
├── Controllers/         # Controladores MVC
├── Data/               # Contexto de base de datos
├── Models/             # Modelos de dominio
├── Services/           # Lógica de negocio
├── Views/              # Interfaces de usuario
├── appsettings.json    # Configuración
├── Program.cs          # Punto de entrada
└── FinanzasPersonales.NET.csproj
```

## Base de Datos
- **Motor**: SQLite
- **ORM**: Entity Framework Core 8.0
- **Archivo**: `finanzas.db` (creado automáticamente)
- **Tablas**: Usuarios, Ingresos, Gastos, Ahorros, Inversiones

## Características Técnicas
- **Async/Await**: Operaciones asíncronas para mejor rendimiento
- **Inyección de Dependencias**: Arquitectura desacoplada y testeable
- **Validación**: Validación robusta en todos los niveles
- **Seguridad**: Hash de contraseñas con SHA256
- **Error Handling**: Manejo completo de errores y excepciones

## Migración desde Java
Este proyecto es una migración completa de una aplicación Java Swing a .NET Console:
- ✅ Arquitectura MVC mantenida
- ✅ Funcionalidad completa preservada
- ✅ Mejorada con async/await y Entity Framework
- ✅ Base de datos integrada con SQLite

## Desarrollo Futuro
- [ ] Interfaz gráfica (WPF/MAUI)
- [ ] API REST
- [ ] Reportes en PDF
- [ ] Gráficos interactivos
- [ ] Importación/Exportación de datos
- [ ] Múltiples cuentas bancarias

## Autor
Migrado a .NET con GitHub Copilot

---
**¡Empieza a gestionar tus finanzas personales de manera efectiva!** 💰📊
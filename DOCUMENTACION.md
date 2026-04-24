# DOCUMENTACIÓN — Finanzas Personales

> **Proyecto universitario** de gestión de finanzas personales desarrollado en Java con interfaz gráfica Swing y base de datos MariaDB.

---

## Tabla de Contenidos

1. [Descripción General](#1-descripción-general)
2. [Requisitos y Configuración](#2-requisitos-y-configuración)
3. [Estructura del Proyecto](#3-estructura-del-proyecto)
4. [Arquitectura del Sistema](#4-arquitectura-del-sistema)
5. [Base de Datos](#5-base-de-datos)
6. [Capa de Modelo](#6-capa-de-modelo)
7. [Capa de Servicio](#7-capa-de-servicio)
8. [Capa de Controlador](#8-capa-de-controlador)
9. [Capa de Vista](#9-capa-de-vista)
10. [Conceptos de POO Aplicados](#10-conceptos-de-poo-aplicados)
11. [Patrones de Diseño](#11-patrones-de-diseño)
12. [Flujo de Navegación](#12-flujo-de-navegación)
13. [Compilación y Ejecución](#13-compilación-y-ejecución)

---

## 1. Descripción General

**Finanzas Personales** es una aplicación de escritorio que permite al usuario:

- **Registrarse e iniciar sesión** con credenciales almacenadas en base de datos.
- **Registrar ingresos y gastos** con monto, descripción y fecha.
- **Ahorrar y retirar dinero** con validación de fondos disponibles.
- **Simular inversiones** con cálculo de ganancia simple.
- **Consultar estadísticas** de ingresos vs gastos con gráfica de barras.
- **Ver un histórico mensual** agrupado por periodo con detalle y gráfica.

La interfaz utiliza un **tema oscuro profesional** con colores desaturados y tipografía Segoe UI.

---

## 2. Requisitos y Configuración

### Tecnologías

| Componente       | Tecnología                              |
|------------------|-----------------------------------------|
| Lenguaje         | Java (OpenJDK 25 — Eclipse Temurin)     |
| Interfaz gráfica | Java Swing                              |
| Base de datos    | MariaDB 10.x (via XAMPP)                |
| Driver JDBC      | mariadb-java-client-3.3.3.jar           |
| Sistema operativo| Windows                                 |

### Prerequisitos

1. **Java JDK 25** instalado y en el PATH del sistema.
2. **XAMPP** ejecutándose con Apache y MySQL activos.
3. Base de datos `finanzas_db` creada en MariaDB (puerto 3306).

### Configuración de Base de Datos

```sql
-- Crear la base de datos
CREATE DATABASE IF NOT EXISTS finanzas_db;
USE finanzas_db;

-- Tabla de usuarios
CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL
);

-- Tabla de movimientos
CREATE TABLE movimientos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo VARCHAR(10) NOT NULL,         -- 'Ingreso' o 'Gasto'
    monto DOUBLE NOT NULL,
    descripcion VARCHAR(200),
    fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### Conexión

La conexión se configura en `Conexion.java`:

| Parámetro | Valor                                      |
|-----------|--------------------------------------------|
| URL       | `jdbc:mariadb://localhost:3306/finanzas_db` |
| Usuario   | `root`                                      |
| Contraseña| *(vacía)*                                  |

---

## 3. Estructura del Proyecto

```
FinanzasPersonales/
├── src/
│   ├── Main.java                          # Punto de entrada
│   ├── conexion/
│   │   └── Conexion.java                  # Conexión JDBC a MariaDB
│   ├── modelo/
│   │   ├── Movimiento.java                # Clase abstracta base
│   │   ├── Ingreso.java                   # Hereda de Movimiento
│   │   ├── Gasto.java                     # Hereda de Movimiento
│   │   ├── Ahorro.java                    # Modelo de ahorro
│   │   ├── Inversion.java                 # Modelo de inversión
│   │   ├── Calculadora.java               # Operaciones matemáticas
│   │   └── Usuario.java                   # Autenticación y registro
│   ├── servicio/
│   │   ├── MovimientoService.java         # CRUD de movimientos
│   │   ├── AhorroService.java             # Lógica de ahorros
│   │   ├── EstadisticaService.java        # Cálculos estadísticos
│   │   └── InversionService.java          # Cálculos de inversión
│   ├── controlador/
│   │   ├── MovimientoController.java      # Controlador de movimientos
│   │   ├── AhorroController.java          # Controlador de ahorros
│   │   ├── EstadisticaController.java     # Controlador de estadísticas
│   │   └── InversionController.java       # Controlador de inversiones
│   └── vista/
│       ├── EstiloApp.java                 # Estilos centralizados (tema oscuro)
│       ├── ViewLogin.java                 # Pantalla de login/registro
│       ├── ViewMenuPrincipal.java         # Menú principal con tarjetas
│       ├── ViewMovimientos.java           # Registro de ingresos/gastos
│       ├── AhorroView.java                # Módulo de ahorros
│       ├── InversionView.java             # Simulador de inversiones
│       ├── EstadisticaView.java           # Estadísticas financieras
│       ├── ViewHistorico.java             # Histórico mensual
│       └── GraficaPanel.java              # Gráfica de barras personalizada
├── lib/
│   └── mariadb-java-client-3.3.3.jar      # Driver JDBC
└── README.md
```

**Total: 26 archivos Java** organizados en 5 paquetes.

---

## 4. Arquitectura del Sistema

El proyecto sigue el patrón **MVC (Modelo-Vista-Controlador)** con una capa de servicio intermedia:

```
┌─────────────────────────────────────────────────────┐
│                      VISTA                          │
│  ViewLogin · ViewMenuPrincipal · ViewMovimientos    │
│  AhorroView · InversionView · EstadisticaView       │
│  ViewHistorico · GraficaPanel · EstiloApp           │
└────────────────────┬────────────────────────────────┘
                     │ eventos de usuario
                     ▼
┌─────────────────────────────────────────────────────┐
│                   CONTROLADOR                       │
│  MovimientoController · AhorroController            │
│  EstadisticaController · InversionController        │
└────────────────────┬────────────────────────────────┘
                     │ delegación
                     ▼
┌─────────────────────────────────────────────────────┐
│                    SERVICIO                         │
│  MovimientoService · AhorroService                  │
│  EstadisticaService · InversionService              │
└────────────────────┬────────────────────────────────┘
                     │ acceso a datos
                     ▼
┌─────────────────────────────────────────────────────┐
│                     MODELO                          │
│  Movimiento (abstracta) → Ingreso / Gasto          │
│  Ahorro · Inversion · Calculadora · Usuario         │
└────────────────────┬────────────────────────────────┘
                     │ JDBC
                     ▼
┌─────────────────────────────────────────────────────┐
│               BASE DE DATOS (MariaDB)               │
│          conexion/Conexion.java                      │
│     Tablas: usuarios · movimientos                   │
└─────────────────────────────────────────────────────┘
```

### Flujo de datos

1. El **usuario** interactúa con la **Vista** (botones, formularios).
2. La Vista invoca métodos del **Controlador**.
3. El Controlador delega la lógica al **Servicio**.
4. El Servicio opera sobre los **Modelos** y consulta la **Base de Datos** vía JDBC.
5. Los resultados fluyen de regreso a la Vista para su presentación.

---

## 5. Base de Datos

### Diagrama de Tablas

```
┌──────────────────────────┐     ┌────────────────────────────────────┐
│        usuarios          │     │           movimientos              │
├──────────────────────────┤     ├────────────────────────────────────┤
│ id       INT PK AUTO_INC│     │ id          INT PK AUTO_INCREMENT  │
│ username VARCHAR(50) UNI │     │ tipo        VARCHAR(10)            │
│ password VARCHAR(255)    │     │ monto       DOUBLE                 │
└──────────────────────────┘     │ descripcion VARCHAR(200)           │
                                 │ fecha       TIMESTAMP DEFAULT NOW  │
                                 └────────────────────────────────────┘
```

### Consultas SQL utilizadas

| Ubicación               | Consulta                                                                                       | Propósito                              |
|--------------------------|-----------------------------------------------------------------------------------------------|----------------------------------------|
| `Usuario.autenticar()`   | `SELECT id FROM usuarios WHERE username = ? AND password = ?`                                 | Verificar credenciales de login        |
| `Usuario.registrar()`    | `SELECT id FROM usuarios WHERE username = ?`                                                  | Verificar si el usuario ya existe      |
| `Usuario.registrar()`    | `INSERT INTO usuarios (username, password) VALUES (?, ?)`                                     | Registrar nuevo usuario                |
| `MovimientoService.guardar()` | `INSERT INTO movimientos (tipo, monto, descripcion, fecha) VALUES (?, ?, ?, ?)`          | Guardar un ingreso o gasto             |
| `MovimientoService.listarMovimientos()` | `SELECT * FROM movimientos`                                                      | Obtener todos los movimientos          |
| `MovimientoService.listarPorMes()` | `SELECT DATE_FORMAT(fecha, '%Y-%m') as periodo, SUM(CASE WHEN tipo='Ingreso'...)`     | Agrupar totales por mes                |
| `MovimientoService.listarMovimientosPorMes()` | `SELECT * FROM movimientos WHERE DATE_FORMAT(fecha, '%Y-%m') = ?`              | Obtener movimientos de un mes          |

> Todas las consultas usan **PreparedStatement** para prevenir inyección SQL.

---

## 6. Capa de Modelo

### 6.1 Movimiento (clase abstracta)

```
modelo/Movimiento.java
```

Clase base que define la estructura común de todos los movimientos financieros.

| Atributo      | Tipo     | Visibilidad | Descripción                |
|---------------|----------|-------------|----------------------------|
| `id`          | `int`    | `private`   | ID del movimiento en BD    |
| `monto`       | `double` | `private`   | Cantidad del movimiento    |
| `descripcion` | `String` | `private`   | Descripción del movimiento |

| Método              | Retorno  | Descripción                                      |
|---------------------|----------|--------------------------------------------------|
| `Movimiento(monto, descripcion)` | —  | Constructor que inicializa monto y descripción |
| `getMonto()`        | `double` | Getter del monto                                 |
| `getDescripcion()`  | `String` | Getter de la descripción                         |
| `setId(int)`        | `void`   | Setter del ID                                    |
| `getTipo()`         | `String` | **Método abstracto** — retorna el tipo           |

**Conceptos POO:** Abstracción, Encapsulamiento, Herencia (clase base).

### 6.2 Ingreso (hereda de Movimiento)

```
modelo/Ingreso.java
```

Representa un movimiento de tipo ingreso. Implementa `getTipo()` retornando `"Ingreso"`.

**Conceptos POO:** Herencia, Polimorfismo (sobreescribe `getTipo()`).

### 6.3 Gasto (hereda de Movimiento)

```
modelo/Gasto.java
```

Representa un movimiento de tipo gasto. Implementa `getTipo()` retornando `"Gasto"`.

**Conceptos POO:** Herencia, Polimorfismo (sobreescribe `getTipo()`).

### 6.4 Ahorro

```
modelo/Ahorro.java
```

Maneja el saldo de ahorro del usuario. Objeto compartido entre `AhorroView` e `InversionView`.

| Atributo | Tipo     | Visibilidad | Descripción          |
|----------|----------|-------------|----------------------|
| `saldo`  | `double` | `private`   | Saldo actual ahorrado |

| Método               | Retorno   | Descripción                                          |
|----------------------|-----------|------------------------------------------------------|
| `Ahorro()`           | —         | Constructor, inicializa saldo en 0                   |
| `depositar(monto)`   | `void`    | Suma al saldo si monto > 0                           |
| `retirar(monto)`     | `boolean` | Resta del saldo si hay fondos suficientes             |
| `getSaldo()`         | `double`  | Retorna el saldo actual                              |

**Conceptos POO:** Encapsulamiento, Validación de datos.

### 6.5 Inversion

```
modelo/Inversion.java
```

Representa una simulación de inversión con cálculo de ganancia simple.

| Atributo      | Tipo     | Visibilidad | Descripción              |
|---------------|----------|-------------|--------------------------|
| `capital`     | `double` | `private`   | Capital invertido         |
| `tasaInteres` | `double` | `private`   | Tasa de interés decimal   |
| `tiempo`      | `int`    | `private`   | Tiempo en meses           |

| Método                       | Retorno  | Descripción                              |
|------------------------------|----------|------------------------------------------|
| `Inversion(capital, tasa, tiempo)` | — | Constructor                              |
| `calcularGananciaSimple()`   | `double` | Retorna `capital × tasa × tiempo`        |
| `calcularMontoTotal()`       | `double` | Retorna `capital + ganancia`             |

**Conceptos POO:** Encapsulamiento, Creación de objetos.

### 6.6 Calculadora

```
modelo/Calculadora.java
```

Clase utilitaria con operaciones matemáticas básicas.

| Método                    | Retorno  | Descripción                               |
|---------------------------|----------|-------------------------------------------|
| `sumar(a, b)`             | `double` | Retorna `a + b`                           |
| `restar(a, b)`            | `double` | Retorna `a - b`                           |
| `porcentaje(parte, total)` | `double` | Retorna `(parte/total) × 100`            |
| `promedio(total, cantidad)` | `double` | Retorna `total / cantidad`              |

### 6.7 Usuario

```
modelo/Usuario.java
```

Métodos estáticos para autenticación y registro contra la base de datos.

| Método                       | Retorno   | Descripción                                        |
|------------------------------|-----------|---------------------------------------------------|
| `autenticar(user, pass)`     | `boolean` | Verifica credenciales en BD (static)               |
| `registrar(user, pass)`      | `boolean` | Registra usuario nuevo, valida duplicados (static) |

**Conceptos POO:** Métodos estáticos, Try-with-resources (gestión automática de recursos).

---

## 7. Capa de Servicio

### 7.1 MovimientoService

```
servicio/MovimientoService.java
```

Gestiona la persistencia de movimientos (ingresos y gastos) en la base de datos.

| Método                              | Retorno                  | Descripción                                         |
|-------------------------------------|--------------------------|-----------------------------------------------------|
| `agregarIngreso(monto, desc, fecha)` | `boolean`               | Guarda un ingreso en BD                             |
| `agregarGasto(monto, desc, fecha)`   | `boolean`               | Guarda un gasto en BD                               |
| `guardar(tipo, monto, desc, fecha)`  | `boolean` *(private)*   | INSERT genérico con PreparedStatement               |
| `listarMovimientos()`                | `ArrayList<Movimiento>` | Obtiene todos los movimientos de la BD              |
| `listarPorMes()`                     | `ArrayList<Object[]>`   | Agrupa totales de ingresos/gastos por mes           |
| `listarMovimientosPorMes(periodo)`   | `ArrayList<Movimiento>` | Obtiene movimientos de un mes específico            |
| `crearMovimientoDesdeRS(rs)`         | `Movimiento` *(private)* | Convierte un ResultSet a objeto Ingreso o Gasto    |

**Conceptos POO:** Polimorfismo (crea `Ingreso` o `Gasto` según el campo `tipo`), Reutilización (método compartido `crearMovimientoDesdeRS`), Try-with-resources.

### 7.2 AhorroService

```
servicio/AhorroService.java
```

Lógica de negocio para el módulo de ahorros.

| Atributo           | Tipo                | Descripción                     |
|--------------------|---------------------|---------------------------------|
| `estadisticaService` | `EstadisticaService` | Para obtener ingresos/gastos  |

| Método                          | Retorno   | Descripción                                                  |
|---------------------------------|-----------|--------------------------------------------------------------|
| `calcularDisponible()`          | `double`  | Retorna ingresos − gastos desde la BD                        |
| `guardarDinero(ahorro, monto)`  | `boolean` | Deposita si `monto ≤ disponible − yaAhorrado`                |
| `retirarDinero(ahorro, monto)`  | `boolean` | Retira del ahorro (delega a `Ahorro.retirar()`)             |

**Conceptos POO:** Composición (usa `EstadisticaService`), Validación de reglas de negocio.

### 7.3 EstadisticaService

```
servicio/EstadisticaService.java
```

Cálculos estadísticos sobre los movimientos registrados.

| Atributo          | Tipo                 | Descripción                    |
|-------------------|----------------------|--------------------------------|
| `calculadora`     | `Calculadora`        | Para operaciones matemáticas   |
| `movimientoService` | `MovimientoService` | Para obtener movimientos       |

| Método                              | Retorno  | Descripción                                 |
|-------------------------------------|----------|---------------------------------------------|
| `calcularTotal(tipo, movimientos)`  | `double` | Suma montos filtrados por tipo *(private)*  |
| `calcularTotalIngresos()`           | `double` | Total de todos los ingresos                 |
| `calcularTotalGastos()`             | `double` | Total de todos los gastos                   |
| `balance(ingresos, gastos)`         | `double` | Calcula ingresos − gastos                   |
| `porcentaje(gastos, ingresos)`      | `double` | Porcentaje de gastos sobre ingresos         |
| `promedio(total, cantidad)`         | `double` | Promedio por movimiento                     |
| `contarMovimientos()`               | `int`    | Cantidad total de movimientos               |

**Conceptos POO:** Composición, Reutilización (método `calcularTotal` compartido).

### 7.4 InversionService

```
servicio/InversionService.java
```

Delegación de cálculos de inversión al modelo.

| Método                           | Retorno  | Descripción                                 |
|----------------------------------|----------|---------------------------------------------|
| `calcularGanancia(inversion)`    | `double` | Delega a `Inversion.calcularGananciaSimple()` |
| `calcularMontoTotal(inversion)`  | `double` | Delega a `Inversion.calcularMontoTotal()`    |

**Conceptos POO:** Delegación, Separación de capas.

---

## 8. Capa de Controlador

Cada controlador actúa como intermediario entre la Vista y el Servicio. Contiene una referencia privada al servicio correspondiente (Composición) y delega todas las operaciones.

### 8.1 MovimientoController

```
controlador/MovimientoController.java
```

| Método                                 | Retorno                  | Descripción                              |
|----------------------------------------|--------------------------|------------------------------------------|
| `agregarIngreso(monto, desc, fecha)`   | `boolean`                | Delega a `MovimientoService`             |
| `agregarGasto(monto, desc, fecha)`     | `boolean`                | Delega a `MovimientoService`             |
| `listarMovimientos()`                  | `ArrayList<Movimiento>`  | Delega a `MovimientoService`             |
| `listarPorMes()`                       | `ArrayList<Object[]>`    | Delega a `MovimientoService`             |
| `listarMovimientosPorMes(periodo)`     | `ArrayList<Movimiento>`  | Delega a `MovimientoService`             |

### 8.2 AhorroController

```
controlador/AhorroController.java
```

| Método                       | Retorno   | Descripción                                |
|------------------------------|-----------|--------------------------------------------|
| `depositar(ahorro, monto)`   | `boolean` | Delega a `AhorroService.guardarDinero()`   |
| `retirar(ahorro, monto)`     | `boolean` | Delega a `AhorroService.retirarDinero()`   |
| `getDisponible()`            | `double`  | Delega a `AhorroService.calcularDisponible()` |

### 8.3 EstadisticaController

```
controlador/EstadisticaController.java
```

| Método                           | Retorno  | Descripción                                     |
|----------------------------------|----------|-------------------------------------------------|
| `getTotalIngresos()`             | `double` | Delega a `EstadisticaService`                   |
| `getTotalGastos()`               | `double` | Delega a `EstadisticaService`                   |
| `balance(ingresos, gastos)`      | `double` | Delega a `EstadisticaService`                   |
| `porcentaje(gastos, ingresos)`   | `double` | Delega a `EstadisticaService`                   |
| `promedio(total, cantidad)`      | `double` | Delega a `EstadisticaService`                   |
| `contarMovimientos()`            | `int`    | Delega a `EstadisticaService`                   |

### 8.4 InversionController

```
controlador/InversionController.java
```

| Método                            | Retorno  | Descripción                          |
|-----------------------------------|----------|--------------------------------------|
| `calcularGanancia(inversion)`     | `double` | Delega a `InversionService`          |
| `calcularMontoTotal(inversion)`   | `double` | Delega a `InversionService`          |

---

## 9. Capa de Vista

Todas las vistas extienden `JFrame` (Herencia) y usan `EstiloApp` para mantener un diseño oscuro uniforme.

### 9.1 EstiloApp (Estilos centralizados)

```
vista/EstiloApp.java
```

Clase utilitaria que centraliza todos los colores, fuentes y métodos de creación de componentes.

**Paleta de colores del tema oscuro suave:**

| Constante    | RGB            | Uso                            |
|-------------|----------------|-------------------------------|
| `HEADER`    | (35, 40, 48)   | Encabezados oscuros           |
| `FONDO`     | (45, 50, 58)   | Fondo general de las vistas   |
| `TARJETA`   | (55, 62, 72)   | Paneles informativos          |
| `CAMPO`     | (62, 70, 82)   | Campos de texto               |
| `BORDE`     | (75, 82, 95)   | Bordes sutiles                |
| `TEXTO`     | (210, 215, 220)| Texto principal               |
| `TEXTO_SEC` | (145, 155, 165)| Texto secundario              |
| `GRIS`      | (120, 130, 140)| Elementos de apoyo            |
| `PRIMARIO`  | (70, 130, 180) | Acciones principales (azul)   |
| `EXITO`     | (75, 145, 95)  | Éxito/ingresos (verde)        |
| `PELIGRO`   | (175, 75, 70)  | Error/gastos (rojo)           |
| `MORADO`    | (110, 95, 160) | Estadísticas (morado)         |
| `TURQUESA`  | (65, 150, 140) | Inversiones (turquesa)        |
| `NARANJA`   | (180, 135, 70) | Alertas (naranja)             |

**Fuentes:**

| Constante         | Fuente               | Uso                    |
|-------------------|-----------------------|------------------------|
| `FUENTE_TITULO`   | Segoe UI Bold 20     | Títulos de sección     |
| `FUENTE_SUBTITULO`| Segoe UI Bold 16     | Subtítulos y saldos    |
| `FUENTE_CUERPO`   | Segoe UI Plain 14    | Texto general          |
| `FUENTE_BOTON`    | Segoe UI Bold 13     | Texto de botones       |
| `FUENTE_MONO`     | Consolas Plain 13    | Resultados/código      |

**Métodos utilitarios:**

| Método                          | Descripción                                        |
|---------------------------------|----------------------------------------------------|
| `crearHeader(titulo)`           | Panel de encabezado con fondo oscuro y texto claro |
| `crearBoton(texto, color)`      | Botón estilizado con color de fondo y cursor mano  |
| `configurarVentana(ventana, titulo, ancho, alto)` | Tamaño, título y centrado   |
| `aplicarEstiloCampo(campo)`     | Aplica tema oscuro a un JTextField                 |
| `aplicarEstiloArea(area)`       | Aplica tema oscuro a un JTextArea                  |
| `crearLabel(texto)`             | JLabel con fuente y color del tema                 |
| `aplicarEstiloTabla(tabla)`     | Aplica tema oscuro a tablas JTable                 |

### 9.2 ViewLogin

```
vista/ViewLogin.java
```

Pantalla de inicio de sesión y registro de usuario.

- **Campos:** `txtUsuario` (JTextField), `txtPassword` (JPasswordField)
- **Botones:** "Iniciar Sesión" (color PRIMARIO), "Crear Cuenta" (color EXITO)
- **Lógica:** Usa `Usuario.autenticar()` y `Usuario.registrar()` (métodos estáticos)
- **Navegación:** Al autenticarse, abre `ViewMenuPrincipal` y se cierra

### 9.3 ViewMenuPrincipal

```
vista/ViewMenuPrincipal.java
```

Menú principal con diseño de tarjetas (cards) en tema oscuro profundo.

- **Colores propios:** Usa su propia paleta más oscura que el resto (FONDO_MENU: 30,30,30)
- **Módulos disponibles (tarjetas):**
  1. **Movimientos** — Registrar ingresos y gastos (color Azul Acero)
  2. **Ahorros** — Gestionar ahorros (color Verde Oliva)
  3. **Inversiones** — Simular inversiones (color Gris Azulado)
  4. **Estadísticas** — Ver estadísticas financieras (color Índigo)
  5. **Histórico** — Consultar histórico mensual (color Bronce)
- **Efecto hover** en tarjetas al pasar el mouse
- **Pie de página** con botón sutil "Cerrar sesión"
- **Objeto compartido:** Crea un `Ahorro` que comparte entre `AhorroView` e `InversionView`

### 9.4 ViewMovimientos

```
vista/ViewMovimientos.java
```

Formulario para registrar ingresos y gastos.

- **Campos:** Monto, Descripción, Fecha (por defecto hoy en formato `yyyy-MM-dd`)
- **Botones:** "Agregar Ingreso", "Agregar Gasto", "Ver Movimientos", "Volver al Menú"
- **Formato de monto:** Acepta formato con puntos de miles (ej: 1.500.000)
- **Validación:** Verifica campos vacíos, monto válido > 0, formato de fecha correcto
- **Polimorfismo:** Al listar, usa `getTipo()` que retorna "Ingreso" o "Gasto" según la clase

### 9.5 AhorroView

```
vista/AhorroView.java
```

Módulo para ahorrar y retirar dinero del saldo disponible.

- **Panel informativo:** Muestra Disponible (ingresos − gastos) y Total ahorrado
- **Campo:** Monto a depositar/retirar
- **Validación:** No permite ahorrar más de lo disponible ni retirar más de lo ahorrado
- **Actualización en tiempo real:** Los saldos se actualizan después de cada operación
- **Composición:** Recibe el objeto `Ahorro` compartido desde el menú

### 9.6 InversionView

```
vista/InversionView.java
```

Simulador de inversión con interés simple.

- **Panel informativo:** Muestra el capital disponible (lo ahorrado)
- **Campos:** Capital a invertir, Tasa de interés (decimal), Tiempo (meses)
- **Resultados:** Muestra capital, tasa, tiempo, ganancia simple y total
- **Validación:** No permite invertir más de lo ahorrado
- **Creación de objetos:** Instancia un `new Inversion(capital, tasa, tiempo)` para calcular

### 9.7 EstadisticaView

```
vista/EstadisticaView.java
```

Muestra indicadores estadísticos generales.

- **Botón:** "Calcular Estadísticas"
- **Resultados en texto:**
  - Total Ingresos / Total Gastos / Balance
  - Porcentaje gastos/ingresos
  - Total de movimientos / Promedio por movimiento
  - Estado: POSITIVO o NEGATIVO
- **Gráfica:** Barras agrupadas Ingresos vs Gastos (usando `GraficaPanel`)

### 9.8 ViewHistorico

```
vista/ViewHistorico.java
```

Vista de solo lectura (NO CRUD) del historial mensual.

- **Panel superior:** Tabla resumen por mes (Periodo, Ingresos, Gastos, Balance) + Gráfica
- **Panel inferior:** Selector de mes (JComboBox) + Tabla detalle de movimientos
- **Layout:** JSplitPane divide la vista en dos secciones redimensionables
- **Gráfica:** Muestra hasta 6 meses de barras agrupadas
- **Solo lectura:** Las tablas no son editables (`isCellEditable` retorna `false`)

### 9.9 GraficaPanel

```
vista/GraficaPanel.java
```

Panel personalizado que dibuja una gráfica de barras agrupadas con Graphics2D.

- **Datos:** Recibe arreglos de etiquetas, ingresos y gastos
- **Barras:** Verde para ingresos, rojo para gastos, con bordes redondeados
- **Ejes:** Con líneas de referencia horizontales y valores formateados (K, M)
- **Leyenda:** Indicadores de color con texto descriptivo
- **Fondo:** Tema oscuro (color TARJETA)
- **Polimorfismo:** Sobreescribe `paintComponent()` para el dibujo personalizado

---

## 10. Conceptos de POO Aplicados

### 10.1 Herencia

```
Movimiento (abstracta)
    ├── Ingreso   → getTipo() retorna "Ingreso"
    └── Gasto     → getTipo() retorna "Gasto"
```

Todas las vistas heredan de `JFrame`. `GraficaPanel` hereda de `JPanel`.

### 10.2 Polimorfismo

- **Método `getTipo()`:** El servicio crea `Ingreso` o `Gasto` según el campo de BD, pero los trata como `Movimiento`. Al llamar `getTipo()`, Java ejecuta la versión correcta.
- **`paintComponent()`:** `GraficaPanel` sobreescribe el método de `JPanel` para dibujar barras personalizadas.

```java
// Ejemplo de polimorfismo en MovimientoService
Movimiento m;
if ("Ingreso".equals(tipo)) {
    m = new Ingreso(monto, descripcion);   // → getTipo() = "Ingreso"
} else {
    m = new Gasto(monto, descripcion);     // → getTipo() = "Gasto"
}
```

### 10.3 Abstracción

`Movimiento` es una clase **abstracta** con el método abstracto `getTipo()`. No se puede instanciar directamente — se debe usar `Ingreso` o `Gasto`.

### 10.4 Encapsulamiento

Todos los atributos son `private`. Se accede a ellos mediante getters y setters. Los componentes visuales internos de cada vista son privados.

### 10.5 Composición

- Los **controladores contienen servicios** (relación "tiene un").
- Los **servicios contienen otros servicios** (AhorroService usa EstadisticaService).
- Las **vistas contienen controladores**.
- `ViewMenuPrincipal` crea un `Ahorro` compartido entre `AhorroView` e `InversionView`.

### 10.6 Reutilización

- **`EstiloApp`:** Centraliza estilos, evitando duplicación de colores y fuentes.
- **`crearMovimientoDesdeRS()`:** Método compartido que evita duplicar la conversión ResultSet → Objeto.
- **`calcularTotal(tipo)`:** Suma genérica reutilizada para ingresos y gastos.
- **`parsearMonto()`:** Validación compartida entre depositar y retirar.

### 10.7 Try-with-resources

Usado en `Usuario.java` y `MovimientoService.java` para cerrar automáticamente `Connection`, `PreparedStatement` y `ResultSet`, evitando fugas de recursos.

---

## 11. Patrones de Diseño

| Patrón                 | Dónde se aplica                                                    |
|------------------------|--------------------------------------------------------------------|
| **MVC**                | Controladores median entre Vistas y Servicios/Modelos              |
| **Capa de Servicio**   | Servicios encapsulan la lógica de negocio entre Controller y Model |
| **Factory**            | `crearMovimientoDesdeRS()` crea `Ingreso` o `Gasto` según datos   |
| **Utility/Helper**     | `EstiloApp` como clase utilitaria con métodos y constantes static  |
| **Observer**           | ActionListeners en botones y JComboBox responden a eventos         |
| **Template Method**    | `Movimiento.getTipo()` abstracto, implementado por subclases       |

---

## 12. Flujo de Navegación

```
                   ┌──────────────┐
                   │   Main.java  │
                   │  (arranque)  │
                   └──────┬───────┘
                          │
                          ▼
                   ┌──────────────┐
                   │  ViewLogin   │
                   │  Login /     │
                   │  Registro    │
                   └──────┬───────┘
                          │ autenticación exitosa
                          ▼
              ┌────────────────────────┐
              │   ViewMenuPrincipal    │
              │   (Menú con tarjetas)  │
              └───┬──┬──┬──┬──┬───────┘
                  │  │  │  │  │
        ┌─────────┘  │  │  │  └──────────┐
        ▼            ▼  │  ▼             ▼
 ┌──────────┐ ┌────────┐│┌──────────┐ ┌──────────┐
 │Movimien- │ │Ahorro- │││Estadis-  │ │View      │
 │tos View  │ │View    ││ ticaView │ │Historico │
 └──────────┘ └────────┘│└──────────┘ └──────────┘
                        ▼
                 ┌──────────┐
                 │Inversion │
                 │View      │
                 └──────────┘
```

- Desde **ViewMovimientos** hay un botón "Volver al Menú" para regresar.
- Las demás vistas se cierran con `DISPOSE_ON_CLOSE` (el menú permanece abierto).
- "Cerrar sesión" regresa al `ViewLogin`.

---

## 13. Compilación y Ejecución

### Compilar

```bash
javac -encoding UTF-8 -cp "lib\mariadb-java-client-3.3.3.jar" -sourcepath src src\Main.java src\conexion\Conexion.java src\modelo\*.java src\controlador\*.java src\servicio\*.java src\vista\*.java -d out
```

### Ejecutar

```bash
java -cp "out;lib\mariadb-java-client-3.3.3.jar" Main
```

### Notas

- Asegurarse de que **XAMPP** esté ejecutándose con MySQL activo antes de iniciar la aplicación.
- La base de datos `finanzas_db` y sus tablas deben existir previamente.
- El driver JDBC `mariadb-java-client-3.3.3.jar` debe estar en la carpeta `lib/`.


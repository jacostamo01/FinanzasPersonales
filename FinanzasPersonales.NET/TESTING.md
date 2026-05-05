# Script de Prueba - Finanzas Personales .NET

## Instrucciones para probar la aplicación

### 1. Requisitos previos
Asegúrate de tener .NET 8.0 SDK instalado:
```bash
dotnet --version
```

### 2. Restaurar dependencias
```bash
dotnet restore
```

### 3. Compilar el proyecto
```bash
dotnet build
```

### 4. Ejecutar la aplicación
```bash
dotnet run
```

### 5. Flujo de prueba recomendado

#### Primera ejecución:
1. **Registro de usuario**
   - Seleccionar opción 2 (Registrarse)
   - Usuario: `admin`
   - Contraseña: `1234`
   - Confirmar contraseña: `1234`

2. **Inicio de sesión**
   - Seleccionar opción 1 (Iniciar sesión)
   - Usuario: `admin`
   - Contraseña: `1234`

3. **Probar funcionalidades**

#### Movimientos:
   - Menú principal → Opción 1 (Movimientos)
   - Agregar algunos ingresos de ejemplo:
	 - Salario: $3000
	 - Freelance: $500
   - Agregar algunos gastos de ejemplo:
	 - Comida: $300 (categoría: Alimentación)
	 - Transporte: $150 (categoría: Transporte)
   - Ver resumen financiero

#### Ahorros:
   - Menú principal → Opción 2 (Ahorros)
   - Crear meta: Vacaciones - $2000
   - Depositar: $500
   - Ver progreso

#### Inversiones:
   - Menú principal → Opción 3 (Inversiones)
   - Crear inversión: Fondo mutuo - $1000 - Tasa 0.08 (8%)
   - Ver rendimientos

#### Estadísticas:
   - Menú principal → Opción 4 (Estadísticas)
   - Ver resumen completo

### 6. Verificaciones de funcionalidad
- ✅ Base de datos se crea automáticamente (finanzas.db)
- ✅ Usuario se registra correctamente
- ✅ Autenticación funciona
- ✅ Movimientos se guardan y muestran
- ✅ Ahorros se calculan correctamente
- ✅ Inversiones calculan rendimientos
- ✅ Estadísticas muestran datos correctos
- ✅ Navegación entre menús funciona
- ✅ Validaciones de entrada funcionan

### 7. Archivos generados
- `finanzas.db`: Base de datos SQLite con todos los datos
- Los datos persisten entre ejecuciones

### 8. Casos de error a probar
- Intentar login con credenciales incorrectas
- Ingresar montos negativos o texto en campos numéricos
- Intentar retirar más dinero del disponible en ahorros
- Crear usuarios duplicados

### 9. Limpieza
Para reiniciar la aplicación completamente:
```bash
# Eliminar la base de datos
del finanzas.db   # Windows
rm finanzas.db    # Linux/macOS
```

---
**¡El proyecto está listo para usar!** 🎉
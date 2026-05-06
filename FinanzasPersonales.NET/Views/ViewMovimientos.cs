using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Views
{
    /// <summary>
    /// Vista para manejo de movimientos (ingresos y gastos).
    /// </summary>
    public class ViewMovimientos
    {
        private readonly MovimientoController _controller;

        public ViewMovimientos(MovimientoController controller)
        {
            _controller = controller;
        }

        public async Task MostrarMenuAsync()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                EstiloApp.MostrarTitulo("=== MOVIMIENTOS ===");
                Console.WriteLine("1. Agregar Ingreso");
                Console.WriteLine("2. Agregar Gasto");
                Console.WriteLine("3. Ver todos los movimientos");
                Console.WriteLine("4. Ver solo ingresos");
                Console.WriteLine("5. Ver solo gastos");
                Console.WriteLine("6. Ver resumen");
                Console.WriteLine("7. Volver al menú principal");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        await AgregarIngresoAsync();
                        break;
                    case "2":
                        await AgregarGastoAsync();
                        break;
                    case "3":
                        await MostrarTodosLosMovimientosAsync();
                        break;
                    case "4":
                        await MostrarIngresosAsync();
                        break;
                    case "5":
                        await MostrarGastosAsync();
                        break;
                    case "6":
                        await MostrarResumenAsync();
                        break;
                    case "7":
                        continuar = false;
                        break;
                    default:
                        EstiloApp.MostrarError("Opción no válida");
                        EstiloApp.EsperarTecla();
                        break;
                }
            }
        }

        private async Task AgregarIngresoAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== AGREGAR INGRESO ===");

            Console.Write("Monto: $");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                EstiloApp.MostrarError("Monto inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            Console.Write("Descripción: ");
            var descripcion = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                EstiloApp.MostrarError("Descripción no puede estar vacía");
                EstiloApp.EsperarTecla();
                return;
            }

            var exito = await _controller.AgregarIngresoAsync(monto, descripcion);

            if (exito)
            {
                EstiloApp.MostrarExito($"Ingreso de ${monto:F2} agregado exitosamente");
            }
            else
            {
                EstiloApp.MostrarError("Error al agregar el ingreso");
            }

            EstiloApp.EsperarTecla();
        }

        private async Task AgregarGastoAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== AGREGAR GASTO ===");

            Console.Write("Monto: $");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                EstiloApp.MostrarError("Monto inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            Console.Write("Descripción: ");
            var descripcion = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(descripcion))
            {
                EstiloApp.MostrarError("Descripción no puede estar vacía");
                EstiloApp.EsperarTecla();
                return;
            }

            Console.Write("Categoría (opcional): ");
            var categoria = Console.ReadLine();

            var exito = await _controller.AgregarGastoAsync(monto, descripcion, categoria ?? "");

            if (exito)
            {
                EstiloApp.MostrarExito($"Gasto de ${monto:F2} agregado exitosamente");
            }
            else
            {
                EstiloApp.MostrarError("Error al agregar el gasto");
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarTodosLosMovimientosAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== TODOS LOS MOVIMIENTOS ===");

            var movimientos = await _controller.ListarMovimientosAsync();

            if (!movimientos.Any())
            {
                EstiloApp.MostrarInfo("No hay movimientos registrados");
            }
            else
            {
                Console.WriteLine($"{"Fecha",-12} {"Tipo",-8} {"Monto",-12} {"Descripción",-30}");
                Console.WriteLine(new string('-', 65));

                foreach (var movimiento in movimientos)
                {
                    Console.WriteLine($"{movimiento.Fecha:dd/MM/yyyy,-12} {movimiento.GetTipo(),-8} ${movimiento.Monto,10:F2} {movimiento.Descripcion,-30}");
                }
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarIngresosAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== INGRESOS ===");

            var ingresos = await _controller.ListarIngresosAsync();

            if (!ingresos.Any())
            {
                EstiloApp.MostrarInfo("No hay ingresos registrados");
            }
            else
            {
                Console.WriteLine($"{"Fecha",-12} {"Monto",-12} {"Descripción",-30}");
                Console.WriteLine(new string('-', 55));

                foreach (var ingreso in ingresos)
                {
                    Console.WriteLine($"{ingreso.Fecha:dd/MM/yyyy,-12} ${ingreso.Monto,10:F2} {ingreso.Descripcion,-30}");
                }
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarGastosAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== GASTOS ===");

            var gastos = await _controller.ListarGastosAsync();

            if (!gastos.Any())
            {
                EstiloApp.MostrarInfo("No hay gastos registrados");
            }
            else
            {
                Console.WriteLine($"{"Fecha",-12} {"Monto",-12} {"Categoría",-15} {"Descripción",-30}");
                Console.WriteLine(new string('-', 70));

                foreach (var gasto in gastos)
                {
                    Console.WriteLine($"{gasto.Fecha:dd/MM/yyyy,-12} ${gasto.Monto,10:F2} {gasto.Categoria,-15} {gasto.Descripcion,-30}");
                }
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarResumenAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== RESUMEN FINANCIERO ===");

            var totalIngresos = await _controller.ObtenerTotalIngresosAsync();
            var totalGastos = await _controller.ObtenerTotalGastosAsync();
            var balance = await _controller.ObtenerBalanceAsync();

            Console.WriteLine($"Total Ingresos: ${totalIngresos:F2}");
            Console.WriteLine($"Total Gastos:   ${totalGastos:F2}");
            Console.WriteLine(new string('-', 30));

            if (balance >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Balance:        ${balance:F2}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Balance:        ${balance:F2}");
            }
            Console.ResetColor();

            EstiloApp.EsperarTecla();
        }
    }
}

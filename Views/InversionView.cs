using FinanzasPersonales.NET.Controllers;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Views
{
    /// <summary>
    /// Vista para manejo de inversiones.
    /// </summary>
    public class InversionView
    {
        private readonly InversionController _controller;

        public InversionView(InversionController controller)
        {
            _controller = controller;
        }

        public async Task MostrarMenuAsync()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                EstiloApp.MostrarTitulo("=== INVERSIONES ===");
                Console.WriteLine("1. Crear inversión");
                Console.WriteLine("2. Ver inversiones");
                Console.WriteLine("3. Actualizar valores");
                Console.WriteLine("4. Ver resumen");
                Console.WriteLine("5. Volver al menú principal");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        await CrearInversionAsync();
                        break;
                    case "2":
                        await MostrarInversionesAsync();
                        break;
                    case "3":
                        await ActualizarValoresAsync();
                        break;
                    case "4":
                        await MostrarResumenAsync();
                        break;
                    case "5":
                        continuar = false;
                        break;
                    default:
                        EstiloApp.MostrarError("Opción no válida");
                        EstiloApp.EsperarTecla();
                        break;
                }
            }
        }

        private async Task CrearInversionAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== CREAR INVERSIÓN ===");

            Console.Write("Monto inicial: $");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                EstiloApp.MostrarError("Monto inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            Console.Write("Tasa de interés anual (ej: 0.05 para 5%): ");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double tasa) || tasa < 0)
            {
                EstiloApp.MostrarError("Tasa inválida");
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

            var exito = await _controller.CrearInversionAsync(monto, tasa, descripcion);

            if (exito)
            {
                EstiloApp.MostrarExito("Inversión creada exitosamente");
            }
            else
            {
                EstiloApp.MostrarError("Error al crear la inversión");
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarInversionesAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== INVERSIONES ===");

            var inversiones = await _controller.ListarInversionesAsync();

            if (!inversiones.Any())
            {
                EstiloApp.MostrarInfo("No hay inversiones registradas");
            }
            else
            {
                Console.WriteLine($"{"ID",-4} {"Descripción",-25} {"Inicial",-12} {"Actual",-12} {"Tasa",-8} {"Rendimiento",-12}");
                Console.WriteLine(new string('-', 85));

                foreach (var inversion in inversiones)
                {
                    var rendimiento = inversion.GetRendimientoPorcentual();
                    Console.WriteLine($"{inversion.Id,-4} {inversion.Descripcion,-25} ${inversion.MontoInicial,10:F2} ${inversion.ValorActual,10:F2} {inversion.TasaInteres,6:P2} {rendimiento,10:F2}%");
                }
            }

            EstiloApp.EsperarTecla();
        }

        private async Task ActualizarValoresAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== ACTUALIZAR INVERSIÓN ===");

            var inversiones = await _controller.ListarInversionesAsync();
            if (!inversiones.Any())
            {
                EstiloApp.MostrarError("No hay inversiones para actualizar");
                EstiloApp.EsperarTecla();
                return;
            }

            // Mostrar lista
            Console.WriteLine($"{"ID",-4} {"Descripción",-25} {"Monto Inicial",-15}");
            Console.WriteLine(new string('-', 45));
            foreach (var inv in inversiones)
            {
                Console.WriteLine($"{inv.Id,-4} {inv.Descripcion,-25} ${inv.MontoInicial,12:F2}");
            }

            // Pedir ID
            Console.WriteLine();
            Console.WriteLine("(Ingrese 0 para salir)");
            Console.Write("Ingrese el ID de la inversión a actualizar: ");

            if (!int.TryParse(Console.ReadLine(), out int id) || id < 0)
            {
                EstiloApp.MostrarError("ID inválido");
                EstiloApp.EsperarTecla();
                return;
            }
            if(id == 0)
            {
                return;
            }

            var inversion = inversiones.FirstOrDefault(i => i.Id == id);
            if (inversion == null)
            {
                EstiloApp.MostrarError("Inversión no encontrada");
                EstiloApp.EsperarTecla();
                return;
            }

            // Pedir nuevo monto
            Console.Write($"Nuevo monto inicial (actual: ${inversion.MontoInicial:F2}): $");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                EstiloApp.MostrarError("Monto inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            var exito = await _controller.ActualizarValorInversionAsync(id, monto);

            if (exito)
                EstiloApp.MostrarExito("Inversión actualizada exitosamente");
            else
                EstiloApp.MostrarError("Error al actualizar la inversión");

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarResumenAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== RESUMEN DE INVERSIONES ===");

            var totalInvertido = await _controller.GetTotalInvertidoAsync();
            var valorActual = await _controller.GetValorTotalActualAsync();
            var gananciaTotal = await _controller.GetGananciaTotalAsync();

            Console.WriteLine($"Total invertido:    ${totalInvertido:F2}");
            Console.WriteLine($"Valor actual:       ${valorActual:F2}");

            if (gananciaTotal >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Ganancia total:     ${gananciaTotal:F2}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Pérdida total:      ${Math.Abs(gananciaTotal):F2}");
            }
            Console.ResetColor();

            if (totalInvertido > 0)
            {
                var rendimientoTotal = (gananciaTotal / totalInvertido) * 100;
                Console.WriteLine($"Rendimiento total:  {rendimientoTotal:F2}%");
            }

            EstiloApp.EsperarTecla();
        }
    }
}

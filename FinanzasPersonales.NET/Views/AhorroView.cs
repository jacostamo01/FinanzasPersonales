using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Views
{
    /// <summary>
    /// Vista para manejo de ahorros.
    /// </summary>
    public class AhorroView
    {
        private readonly AhorroController _controller;

        public AhorroView(AhorroController controller)
        {
            _controller = controller;
        }

        public async Task MostrarMenuAsync()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                EstiloApp.MostrarTitulo("=== AHORROS ===");
                Console.WriteLine("1. Crear meta de ahorro");
                Console.WriteLine("2. Ver metas de ahorro");
                Console.WriteLine("3. Depositar en ahorro");
                Console.WriteLine("4. Retirar de ahorro");
                Console.WriteLine("5. Ver resumen de ahorros");
                Console.WriteLine("6. Volver al menú principal");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        await CrearMetaAhorroAsync();
                        break;
                    case "2":
                        await MostrarMetasAhorroAsync();
                        break;
                    case "3":
                        await DepositarAhorroAsync();
                        break;
                    case "4":
                        await RetirarAhorroAsync();
                        break;
                    case "5":
                        await MostrarResumenAhorrosAsync();
                        break;
                    case "6":
                        continuar = false;
                        break;
                    default:
                        EstiloApp.MostrarError("Opción no válida");
                        EstiloApp.EsperarTecla();
                        break;
                }
            }
        }

        private async Task CrearMetaAhorroAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== CREAR META DE AHORRO ===");

            Console.Write("Monto objetivo: $");
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

            Console.Write("Fecha objetivo (dd/mm/yyyy) - opcional, presione Enter para omitir: ");
            var fechaStr = Console.ReadLine();
            DateTime? fechaObjetivo = null;

            if (!string.IsNullOrWhiteSpace(fechaStr))
            {
                if (DateTime.TryParseExact(fechaStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                {
                    fechaObjetivo = fecha;
                }
                else
                {
                    EstiloApp.MostrarAdvertencia("Fecha inválida, se creará sin fecha objetivo");
                }
            }

            var exito = await _controller.CrearMetaAhorroAsync(monto, descripcion, fechaObjetivo);

            if (exito)
            {
                EstiloApp.MostrarExito("Meta de ahorro creada exitosamente");
            }
            else
            {
                EstiloApp.MostrarError("Error al crear la meta de ahorro");
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarMetasAhorroAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== METAS DE AHORRO ===");

            var ahorros = await _controller.ListarAhorrosAsync();

            if (!ahorros.Any())
            {
                EstiloApp.MostrarInfo("No hay metas de ahorro creadas");
            }
            else
            {
                Console.WriteLine($"{"ID",-4} {"Descripción",-25} {"Actual",-12} {"Objetivo",-12} {"Progreso",-10} {"Estado",-12}");
                Console.WriteLine(new string('-', 85));

                foreach (var ahorro in ahorros)
                {
                    var progreso = ahorro.GetPorcentajeProgreso();
                    var estado = ahorro.ObjetivoAlcanzado ? "Completado" : "En progreso";

                    Console.WriteLine($"{ahorro.Id,-4} {ahorro.Descripcion,-25} ${ahorro.MontoActual,10:F2} ${ahorro.MontoObjetivo,10:F2} {progreso,8:F1}% {estado,-12}");
                }
            }

            EstiloApp.EsperarTecla();
        }

        private async Task DepositarAhorroAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== DEPOSITAR EN AHORRO ===");

            var ahorros = await _controller.ListarAhorrosAsync();

            if (!ahorros.Any())
            {
                EstiloApp.MostrarError("No hay metas de ahorro creadas");
                EstiloApp.EsperarTecla();
                return;
            }

            // Mostrar ahorros disponibles
            Console.WriteLine("Metas de ahorro disponibles:");
            foreach (var ahorro in ahorros.Where(a => !a.ObjetivoAlcanzado))
            {
                Console.WriteLine($"{ahorro.Id}. {ahorro.Descripcion} - ${ahorro.MontoActual:F2} / ${ahorro.MontoObjetivo:F2}");
            }

            Console.Write("\nSeleccione el ID de la meta: ");
            if (!int.TryParse(Console.ReadLine(), out int ahorroId))
            {
                EstiloApp.MostrarError("ID inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            Console.Write("Monto a depositar: $");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                EstiloApp.MostrarError("Monto inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            var disponible = await _controller.GetDisponibleAsync();
            if (monto > disponible)
            {
                EstiloApp.MostrarError($"Fondos insuficientes. Disponible: ${disponible:F2}");
                EstiloApp.EsperarTecla();
                return;
            }

            var exito = await _controller.DepositarAsync(ahorroId, monto);

            if (exito)
            {
                EstiloApp.MostrarExito($"${monto:F2} depositado exitosamente");
            }
            else
            {
                EstiloApp.MostrarError("Error al depositar el dinero");
            }

            EstiloApp.EsperarTecla();
        }

        private async Task RetirarAhorroAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== RETIRAR DE AHORRO ===");

            var ahorros = await _controller.ListarAhorrosAsync();

            if (!ahorros.Any())
            {
                EstiloApp.MostrarError("No hay metas de ahorro creadas");
                EstiloApp.EsperarTecla();
                return;
            }

            // Mostrar ahorros con dinero
            Console.WriteLine("Metas de ahorro con fondos:");
            var ahorrosConFondos = ahorros.Where(a => a.MontoActual > 0).ToList();

            if (!ahorrosConFondos.Any())
            {
                EstiloApp.MostrarError("No hay ahorros con fondos disponibles");
                EstiloApp.EsperarTecla();
                return;
            }

            foreach (var ahorro in ahorrosConFondos)
            {
                Console.WriteLine($"{ahorro.Id}. {ahorro.Descripcion} - ${ahorro.MontoActual:F2} disponible");
            }

            Console.Write("\nSeleccione el ID de la meta: ");
            if (!int.TryParse(Console.ReadLine(), out int ahorroId))
            {
                EstiloApp.MostrarError("ID inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            Console.Write("Monto a retirar: $");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                EstiloApp.MostrarError("Monto inválido");
                EstiloApp.EsperarTecla();
                return;
            }

            var exito = await _controller.RetirarAsync(ahorroId, monto);

            if (exito)
            {
                EstiloApp.MostrarExito($"${monto:F2} retirado exitosamente");
            }
            else
            {
                EstiloApp.MostrarError("Error al retirar el dinero o fondos insuficientes");
            }

            EstiloApp.EsperarTecla();
        }

        private async Task MostrarResumenAhorrosAsync()
        {
            Console.Clear();
            EstiloApp.MostrarSubtitulo("=== RESUMEN DE AHORROS ===");

            var totalAhorrado = await _controller.GetTotalAhorradoAsync();
            var ahorrosCompletos = await _controller.ListarAhorrosCompletosAsync();

            Console.WriteLine($"Total ahorrado: ${totalAhorrado:F2}");
            Console.WriteLine($"Metas completadas: {ahorrosCompletos.Count}");
            Console.WriteLine();

            if (ahorrosCompletos.Any())
            {
                Console.WriteLine("Metas completadas:");
                foreach (var ahorro in ahorrosCompletos)
                {
                    Console.WriteLine($"- {ahorro.Descripcion}: ${ahorro.MontoActual:F2}");
                }
            }

            EstiloApp.EsperarTecla();
        }
    }
}

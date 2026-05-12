using FinanzasPersonales.NET.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Views
{
    public class EstadisticaView
    {
        private readonly EstadisticaController _controller;

        public EstadisticaView(EstadisticaController controller)
        {
            _controller = controller;
        }

        public async Task MostrarEstadisticasAsync()
        {
            Console.Clear();
            EstiloApp.MostrarTitulo("=== ESTADÍSTICAS FINANCIERAS ===");

            // Obtener datos generales
            var totalIngresos = await _controller.GetTotalIngresosAsync();
            var totalGastos = await _controller.GetTotalGastosAsync();
            var balance = await _controller.GetBalanceAsync();
            var promedioIngresos = await _controller.GetPromedioIngresosAsync();
            var promedioGastos = await _controller.GetPromedioGastosAsync();
            var totalMovimientos = await _controller.ContarMovimientosAsync();

            // Mostrar estadísticas generales
            EstiloApp.MostrarSubtitulo("RESUMEN GENERAL");
            Console.WriteLine($"Total de ingresos:     ${totalIngresos:F2}");
            Console.WriteLine($"Total de gastos:       ${totalGastos:F2}");
            Console.WriteLine($"Balance actual:        ${balance:F2}");
            Console.WriteLine($"Promedio de ingresos:  ${promedioIngresos:F2}");
            Console.WriteLine($"Promedio de gastos:    ${promedioGastos:F2}");
            Console.WriteLine($"Total de movimientos:  {totalMovimientos}");

            if (totalIngresos > 0)
            {
                var porcentajeGastos = _controller.CalcularPorcentajeGastos(totalGastos, totalIngresos);
                Console.WriteLine($"% de gastos/ingresos:  {porcentajeGastos:F1}%");
            }

            Console.WriteLine();

            // Mostrar gastos por categoría
            EstiloApp.MostrarSubtitulo("GASTOS POR CATEGORÍA");
            var gastosPorCategoria = await _controller.GetGastosPorCategoriaAsync();

            if (gastosPorCategoria.Any())
            {
                Console.WriteLine($"{"Categoría",-20} {"Total",-12} {"Porcentaje",-10}");
                Console.WriteLine(new string('-', 45));

                foreach (var (categoria, total) in gastosPorCategoria.OrderByDescending(x => x.Total))
                {
                    var porcentaje = totalGastos > 0 ? (total / totalGastos) * 100 : 0;
                    Console.WriteLine($"{categoria,-20} ${total,10:F2} {porcentaje,8:F1}%");
                }
            }
            else
            {
                EstiloApp.MostrarInfo("No hay gastos registrados por categoría");
            }

            Console.WriteLine();

            // Mostrar resumen mensual
            EstiloApp.MostrarSubtitulo("RESUMEN MENSUAL");
            var resumenMensual = await _controller.GetResumenMensualAsync();

            if (resumenMensual.Any())
            {
                Console.WriteLine($"{"Mes/Año",-10} {"Ingresos",-12} {"Gastos",-12} {"Balance",-12}");
                Console.WriteLine(new string('-', 50));

                foreach (var (mes, anio, ingresos, gastos) in resumenMensual.TakeLast(6)) // Últimos 6 meses
                {
                    var balanceMensual = ingresos - gastos;
                    var nombreMes = new DateTime(anio, mes, 1).ToString("MMM/yyyy");

                    Console.Write($"{nombreMes,-10} ${ingresos,10:F2} ${gastos,10:F2} ");

                    if (balanceMensual >= 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"${balanceMensual,10:F2}");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"${balanceMensual,10:F2}");
                    }
                    Console.ResetColor();
                }
            }
            else
            {
                EstiloApp.MostrarInfo("No hay datos mensuales disponibles");
            }

            EstiloApp.EsperarTecla();
        }
    }
}

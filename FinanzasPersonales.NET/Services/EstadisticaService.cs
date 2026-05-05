using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    /// <summary>
    /// Servicio para cálculos de estadísticas financieras.
    /// </summary>
    public class EstadisticaService
    {
        private readonly FinanzasDbContext _context;

        public EstadisticaService(FinanzasDbContext context)
        {
            _context = context;
        }

        public async Task<double> CalcularTotalIngresosAsync()
        {
            return await _context.Ingresos.SumAsync(i => i.Monto);
        }

        public async Task<double> CalcularTotalGastosAsync()
        {
            return await _context.Gastos.SumAsync(g => g.Monto);
        }

        public async Task<double> CalcularBalanceAsync()
        {
            var ingresos = await CalcularTotalIngresosAsync();
            var gastos = await CalcularTotalGastosAsync();
            return ingresos - gastos;
        }

        public async Task<double> CalcularPromedioIngresosAsync()
        {
            var ingresos = await _context.Ingresos.ToListAsync();
            return ingresos.Any() ? ingresos.Average(i => i.Monto) : 0;
        }

        public async Task<double> CalcularPromedioGastosAsync()
        {
            var gastos = await _context.Gastos.ToListAsync();
            return gastos.Any() ? gastos.Average(g => g.Monto) : 0;
        }

        public async Task<int> ContarMovimientosAsync()
        {
            var ingresos = await _context.Ingresos.CountAsync();
            var gastos = await _context.Gastos.CountAsync();
            return ingresos + gastos;
        }

        public double CalcularPorcentajeGastos(double gastos, double ingresos)
        {
            if (ingresos == 0) return 0;
            return (gastos / ingresos) * 100;
        }

        public async Task<List<(string Categoria, double Total)>> ObtenerGastosPorCategoriaAsync()
        {
            return await _context.Gastos
                .GroupBy(g => g.Categoria)
                .Select(group => new ValueTuple<string, double>(
                    group.Key ?? "Sin categoría", 
                    group.Sum(g => g.Monto)))
                .ToListAsync();
        }

        public async Task<List<(int Mes, int Anio, double TotalIngresos, double TotalGastos)>> ObtenerResumenMensualAsync()
        {
            var ingresosPorMes = await _context.Ingresos
                .GroupBy(i => new { i.Fecha.Month, i.Fecha.Year })
                .Select(g => new { g.Key.Month, g.Key.Year, Total = g.Sum(i => i.Monto) })
                .ToListAsync();

            var gastosPorMes = await _context.Gastos
                .GroupBy(g => new { g.Fecha.Month, g.Fecha.Year })
                .Select(g => new { g.Key.Month, g.Key.Year, Total = g.Sum(i => i.Monto) })
                .ToListAsync();

            var resumen = new List<(int, int, double, double)>();

            var periodos = ingresosPorMes.Select(i => new { i.Month, i.Year })
                .Union(gastosPorMes.Select(g => new { g.Month, g.Year }))
                .OrderBy(p => p.Year).ThenBy(p => p.Month);

            foreach (var periodo in periodos)
            {
                var totalIngresos = ingresosPorMes
                    .FirstOrDefault(i => i.Month == periodo.Month && i.Year == periodo.Year)?.Total ?? 0;
                var totalGastos = gastosPorMes
                    .FirstOrDefault(g => g.Month == periodo.Month && g.Year == periodo.Year)?.Total ?? 0;

                resumen.Add((periodo.Month, periodo.Year, totalIngresos, totalGastos));
            }

            return resumen;
        }
    }

    /// <summary>
    /// Clase auxiliar para cálculos básicos.
    /// </summary>
    public class Calculadora
    {
        public double Sumar(double a, double b) => a + b;
        public double Restar(double a, double b) => a - b;
        public double Multiplicar(double a, double b) => a * b;
        public double Dividir(double a, double b) => b != 0 ? a / b : 0;
        public double Porcentaje(double cantidad, double total) => total != 0 ? (cantidad / total) * 100 : 0;
        public double Promedio(double total, int cantidad) => cantidad > 0 ? total / cantidad : 0;
    }
}

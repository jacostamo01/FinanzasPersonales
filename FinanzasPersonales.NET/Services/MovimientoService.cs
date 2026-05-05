using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    /// <summary>
    /// Servicio para movimientos (ingresos y gastos). Maneja la lógica y la conexión a BD.
    /// </summary>
    public class MovimientoService
    {
        private readonly FinanzasDbContext _context;

        public MovimientoService(FinanzasDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AgregarIngresoAsync(double monto, string descripcion)
        {
            try
            {
                var ingreso = new Ingreso(monto, descripcion);
                _context.Ingresos.Add(ingreso);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AgregarGastoAsync(double monto, string descripcion, string categoria = "")
        {
            try
            {
                var gasto = new Gasto(monto, descripcion, categoria);
                _context.Gastos.Add(gasto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Movimiento>> ListarMovimientosAsync()
        {
            var movimientos = new List<Movimiento>();

            var ingresos = await _context.Ingresos.ToListAsync();
            var gastos = await _context.Gastos.ToListAsync();

            movimientos.AddRange(ingresos);
            movimientos.AddRange(gastos);

            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }

        public async Task<List<Ingreso>> ListarIngresosAsync()
        {
            return await _context.Ingresos.OrderByDescending(i => i.Fecha).ToListAsync();
        }

        public async Task<List<Gasto>> ListarGastosAsync()
        {
            return await _context.Gastos.OrderByDescending(g => g.Fecha).ToListAsync();
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
            var totalIngresos = await CalcularTotalIngresosAsync();
            var totalGastos = await CalcularTotalGastosAsync();
            return totalIngresos - totalGastos;
        }

        public async Task<List<Movimiento>> ListarMovimientosPorMesAsync(int mes, int anio)
        {
            var movimientos = new List<Movimiento>();

            var ingresos = await _context.Ingresos
                .Where(i => i.Fecha.Month == mes && i.Fecha.Year == anio)
                .ToListAsync();

            var gastos = await _context.Gastos
                .Where(g => g.Fecha.Month == mes && g.Fecha.Year == anio)
                .ToListAsync();

            movimientos.AddRange(ingresos);
            movimientos.AddRange(gastos);

            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }
    }
}

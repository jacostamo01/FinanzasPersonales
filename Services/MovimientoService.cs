using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class MovimientoService
    {
        private readonly FinanzasDbContext _context;

        public MovimientoService(FinanzasDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AgregarIngresoAsync(double monto, string descripcion, int usuarioId)
        {
            try
            {
                var ingreso = new Ingreso(monto, descripcion) { UsuarioId = usuarioId };
                _context.Ingresos.Add(ingreso);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AgregarGastoAsync(double monto, string descripcion, int usuarioId, string categoria = "")
        {
            try
            {
                var gasto = new Gasto(monto, descripcion, categoria) { UsuarioId = usuarioId };
                _context.Gastos.Add(gasto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Movimiento>> ListarMovimientosAsync(int usuarioId)
        {
            var movimientos = new List<Movimiento>();

            var ingresos = await _context.Ingresos.Where(i => i.UsuarioId == usuarioId).ToListAsync();
            var gastos = await _context.Gastos.Where(g => g.UsuarioId == usuarioId).ToListAsync();

            movimientos.AddRange(ingresos);
            movimientos.AddRange(gastos);

            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }

        public async Task<List<Ingreso>> ListarIngresosAsync(int usuarioId)
        {
            return await _context.Ingresos.Where(i => i.UsuarioId == usuarioId).OrderByDescending(i => i.Fecha).ToListAsync();
        }

        public async Task<List<Gasto>> ListarGastosAsync(int usuarioId)
        {
            return await _context.Gastos.Where(g => g.UsuarioId == usuarioId).OrderByDescending(g => g.Fecha).ToListAsync();
        }

        public async Task<double> CalcularTotalIngresosAsync(int usuarioId)
        {
            var tiene = await _context.Ingresos.AnyAsync(i => i.UsuarioId == usuarioId);
            if (!tiene) return 0;
            return await _context.Ingresos.Where(i => i.UsuarioId == usuarioId).SumAsync(i => i.Monto);
        }

        public async Task<double> CalcularTotalGastosAsync(int usuarioId)
        {
            var tiene = await _context.Gastos.AnyAsync(g => g.UsuarioId == usuarioId);
            if (!tiene) return 0;
            return await _context.Gastos.Where(g => g.UsuarioId == usuarioId).SumAsync(g => g.Monto);
        }

        public async Task<double> CalcularBalanceAsync(int usuarioId)
        {
            var totalIngresos = await CalcularTotalIngresosAsync(usuarioId);
            var totalGastos = await CalcularTotalGastosAsync(usuarioId);
            return totalIngresos - totalGastos;
        }

        public async Task<List<Movimiento>> ListarMovimientosPorMesAsync(int mes, int anio, int usuarioId)
        {
            var movimientos = new List<Movimiento>();

            var ingresos = await _context.Ingresos
                .Where(i => i.UsuarioId == usuarioId && i.Fecha.Month == mes && i.Fecha.Year == anio)
                .ToListAsync();

            var gastos = await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId && g.Fecha.Month == mes && g.Fecha.Year == anio)
                .ToListAsync();

            movimientos.AddRange(ingresos);
            movimientos.AddRange(gastos);

            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }
    }
}

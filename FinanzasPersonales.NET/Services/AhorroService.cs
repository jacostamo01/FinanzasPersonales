using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    /// <summary>
    /// Servicio para el módulo de ahorros. Valida y modifica el modelo Ahorro.
    /// </summary>
    public class AhorroService
    {
        private readonly FinanzasDbContext _context;
        private readonly MovimientoService _movimientoService;

        public AhorroService(FinanzasDbContext context, MovimientoService movimientoService)
        {
            _context = context;
            _movimientoService = movimientoService;
        }

        public async Task<bool> CrearMetaAhorroAsync(double montoObjetivo, string descripcion, DateTime? fechaObjetivo = null)
        {
            try
            {
                var ahorro = new Ahorro(montoObjetivo, descripcion, fechaObjetivo);
                _context.Ahorros.Add(ahorro);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Ahorro>> ListarAhorrosAsync()
        {
            return await _context.Ahorros.OrderByDescending(a => a.FechaCreacion).ToListAsync();
        }

        public async Task<double> CalcularDisponibleAsync()
        {
            return await _movimientoService.CalcularBalanceAsync();
        }

        public async Task<bool> DepositarAhorroAsync(int ahorroId, double monto)
        {
            if (monto <= 0) return false;

            try
            {
                var ahorro = await _context.Ahorros.FindAsync(ahorroId);
                if (ahorro == null) return false;

                var disponible = await CalcularDisponibleAsync();
                if (monto > disponible) return false;

                ahorro.Depositar(monto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RetirarAhorroAsync(int ahorroId, double monto)
        {
            if (monto <= 0) return false;

            try
            {
                var ahorro = await _context.Ahorros.FindAsync(ahorroId);
                if (ahorro == null) return false;

                if (ahorro.Retirar(monto))
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<double> CalcularTotalAhorradoAsync()
        {
            return await _context.Ahorros.SumAsync(a => a.MontoActual);
        }

        public async Task<List<Ahorro>> ListarAhorrosCompletosAsync()
        {
            return await _context.Ahorros
                .Where(a => a.MontoActual >= a.MontoObjetivo)
                .ToListAsync();
        }
    }
}

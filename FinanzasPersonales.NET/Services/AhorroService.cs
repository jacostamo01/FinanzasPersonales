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

        public async Task<bool> CrearMetaAhorroAsync(double montoObjetivo, string descripcion, int usuarioId, DateTime? fechaObjetivo = null)
        {
            try
            {
                var ahorro = new Ahorro(montoObjetivo, descripcion, fechaObjetivo) { UsuarioId = usuarioId };
                _context.Ahorros.Add(ahorro);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Ahorro>> ListarAhorrosAsync(int usuarioId)
        {
            return await _context.Ahorros.Where(a => a.UsuarioId == usuarioId).OrderByDescending(a => a.FechaCreacion).ToListAsync();
        }

        public async Task<double> CalcularDisponibleAsync(int usuarioId)
        {
            return await _movimientoService.CalcularBalanceAsync(usuarioId);
        }

        public async Task<bool> DepositarAhorroAsync(int ahorroId, double monto, int usuarioId)
        {
            if (monto <= 0) return false;

            try
            {
                var ahorro = await _context.Ahorros.FindAsync(ahorroId);
                if (ahorro == null) return false;

                var disponible = await CalcularDisponibleAsync(usuarioId);
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

        public async Task<double> CalcularTotalAhorradoAsync(int usuarioId)
        {
            var tiene = await _context.Ahorros.AnyAsync(a => a.UsuarioId == usuarioId);
            if (!tiene) return 0;
            return await _context.Ahorros.Where(a => a.UsuarioId == usuarioId).SumAsync(a => a.MontoActual);
        }

        public async Task<List<Ahorro>> ListarAhorrosCompletosAsync(int usuarioId)
        {
            return await _context.Ahorros
                .Where(a => a.UsuarioId == usuarioId && a.MontoActual >= a.MontoObjetivo)
                .ToListAsync();
        }
    }
}

using FinanzasPersonales.NET.Data;
using FinanzasPersonales.NET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class ColchonFinancieroService
    {
        private readonly FinanzasDbContext _context;
        private readonly MovimientoService _movimientoService;

        public ColchonFinancieroService(FinanzasDbContext context, MovimientoService movimientoService)
        {
            _context = context;
            _movimientoService = movimientoService;
        }

        public async Task<bool> CrearColchonAsync(int usuarioId, double meta, double porcentajeAhorro)
        {
            if (meta <= 0) return false;
            if (porcentajeAhorro < 0 || porcentajeAhorro > 100) return false;

            try
            {
                var colchon = new ColchonFinanciero(meta, porcentajeAhorro)
                {
                    UsuarioId = usuarioId
                };

                _context.ColchonesFinancieros.Add(colchon);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ColchonFinanciero?> ObtenerColchonAsync(int usuarioId)
        {
            return await _context.ColchonesFinancieros
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<double> CalcularDisponibleAsync(int usuarioId)
        {
            return await _movimientoService.CalcularBalanceAsync(usuarioId);
        }

        public async Task<bool> DepositarColchonAsync(int colchonId, double monto, int usuarioId)
        {
            if (monto <= 0) return false;

            try
            {
                var colchon = await _context.ColchonesFinancieros.FindAsync(colchonId);
                if (colchon == null) return false;

                var disponible = await CalcularDisponibleAsync(usuarioId);
                if (monto > disponible) return false;

                colchon.Depositar(monto);

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RetirarColchonAsync(int colchonId, double monto)
        {
            if (monto <= 0) return false;

            try
            {
                var colchon = await _context.ColchonesFinancieros.FindAsync(colchonId);
                if (colchon == null) return false;

                if (colchon.Retirar(monto))
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

        public async Task<double> CalcularTotalColchonAsync(int usuarioId)
        {
            var tiene = await _context.ColchonesFinancieros
                .AnyAsync(c => c.UsuarioId == usuarioId);

            if (!tiene) return 0;

            return await _context.ColchonesFinancieros
                .Where(c => c.UsuarioId == usuarioId)
                .SumAsync(c => c.MontoActual);
        }

        public async Task<List<ColchonFinanciero>> ListarColchonesAsync(int usuarioId)
        {
            return await _context.ColchonesFinancieros
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
        }

        public async Task<List<ColchonFinanciero>> ListarColchonesCompletosAsync(int usuarioId)
        {
            return await _context.ColchonesFinancieros
                .Where(c => c.UsuarioId == usuarioId && c.MontoActual >= c.Meta)
                .ToListAsync();
        }
    }
}

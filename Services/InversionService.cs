using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    /// <summary>
    /// Servicio para el módulo de inversiones. Maneja la lógica de inversiones.
    /// </summary>
    public class InversionService
    {
        private readonly FinanzasDbContext _context;

        public InversionService(FinanzasDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CrearInversionAsync(double montoInicial, double tasaInteres, string descripcion, int usuarioId, DateTime? fechaVencimiento = null)
        {
            try
            {
                var inversion = new Inversion(montoInicial, tasaInteres, descripcion, fechaVencimiento) { UsuarioId = usuarioId };
                _context.Inversiones.Add(inversion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Inversion>> ListarInversionesAsync(int usuarioId)
        {
            return await _context.Inversiones.Where(i => i.UsuarioId == usuarioId).OrderByDescending(i => i.FechaInicio).ToListAsync();
        }

        public async Task<bool> ActualizarValorInversionAsync(int inversionId, double montoInicial)
        {
            try
            {
                var inversion = await _context.Inversiones.FindAsync(inversionId);
                if (inversion == null) return false;

                inversion.MontoInicial = montoInicial;
                inversion.ValorActual = montoInicial;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<double> CalcularTotalInvertidoAsync(int usuarioId)
        {
            var tiene = await _context.Inversiones.AnyAsync(i => i.UsuarioId == usuarioId);
            if (!tiene) return 0;
            return await _context.Inversiones.Where(i => i.UsuarioId == usuarioId).SumAsync(i => i.MontoInicial);
        }

        public async Task<double> CalcularValorTotalActualAsync(int usuarioId)
        {
            var inversiones = await _context.Inversiones.Where(i => i.UsuarioId == usuarioId).ToListAsync();
            double total = 0;

            foreach (var inversion in inversiones)
            {
                inversion.ActualizarValor();
                total += inversion.ValorActual;
            }

            await _context.SaveChangesAsync();
            return total;
        }

        public async Task<double> CalcularGananciaTotalAsync(int usuarioId)
        {
            var totalInvertido = await CalcularTotalInvertidoAsync(usuarioId);
            var valorActual = await CalcularValorTotalActualAsync(usuarioId);
            return valorActual - totalInvertido;
        }

        public async Task<List<Inversion>> ListarInversionesVencidasAsync(int usuarioId)
        {
            var today = DateTime.Today;
            return await _context.Inversiones
                .Where(i => i.UsuarioId == usuarioId && i.FechaVencimiento.HasValue && i.FechaVencimiento.Value <= today)
                .ToListAsync();
        }

        public double CalcularGanancia(Inversion inversion)
        {
            return inversion.CalcularGananciaCompuesta();
        }

        public double CalcularMontoTotal(Inversion inversion)
        {
            return inversion.CalcularValorActual();
        }
    }
}

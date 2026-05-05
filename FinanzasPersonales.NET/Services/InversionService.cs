using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> CrearInversionAsync(double montoInicial, double tasaInteres, string descripcion, DateTime? fechaVencimiento = null)
        {
            try
            {
                var inversion = new Inversion(montoInicial, tasaInteres, descripcion, fechaVencimiento);
                _context.Inversiones.Add(inversion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Inversion>> ListarInversionesAsync()
        {
            return await _context.Inversiones.OrderByDescending(i => i.FechaInicio).ToListAsync();
        }

        public async Task<bool> ActualizarValorInversionAsync(int inversionId)
        {
            try
            {
                var inversion = await _context.Inversiones.FindAsync(inversionId);
                if (inversion == null) return false;

                inversion.ActualizarValor();
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<double> CalcularTotalInvertidoAsync()
        {
            return await _context.Inversiones.SumAsync(i => i.MontoInicial);
        }

        public async Task<double> CalcularValorTotalActualAsync()
        {
            var inversiones = await _context.Inversiones.ToListAsync();
            double total = 0;

            foreach (var inversion in inversiones)
            {
                inversion.ActualizarValor();
                total += inversion.ValorActual;
            }

            await _context.SaveChangesAsync();
            return total;
        }

        public async Task<double> CalcularGananciaTotalAsync()
        {
            var totalInvertido = await CalcularTotalInvertidoAsync();
            var valorActual = await CalcularValorTotalActualAsync();
            return valorActual - totalInvertido;
        }

        public async Task<List<Inversion>> ListarInversionesVencidasAsync()
        {
            var today = DateTime.Today;
            return await _context.Inversiones
                .Where(i => i.FechaVencimiento.HasValue && i.FechaVencimiento.Value <= today)
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

using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    /// <summary>
    /// Controlador del módulo de inversiones. Conecta la Vista con el Servicio.
    /// </summary>
    public class InversionController
    {
        private readonly InversionService _inversionService;

        public InversionController(InversionService inversionService)
        {
            _inversionService = inversionService;
        }

        public async Task<bool> CrearInversionAsync(double montoInicial, double tasaInteres, string descripcion, DateTime? fechaVencimiento = null)
        {
            if (montoInicial <= 0 || tasaInteres < 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _inversionService.CrearInversionAsync(montoInicial, tasaInteres, descripcion.Trim(), fechaVencimiento);
        }

        public async Task<List<Inversion>> ListarInversionesAsync()
        {
            return await _inversionService.ListarInversionesAsync();
        }

        public async Task<bool> ActualizarValorInversionAsync(int inversionId)
        {
            if (inversionId <= 0)
                return false;

            return await _inversionService.ActualizarValorInversionAsync(inversionId);
        }

        public async Task<double> GetTotalInvertidoAsync()
        {
            return await _inversionService.CalcularTotalInvertidoAsync();
        }

        public async Task<double> GetValorTotalActualAsync()
        {
            return await _inversionService.CalcularValorTotalActualAsync();
        }

        public async Task<double> GetGananciaTotalAsync()
        {
            return await _inversionService.CalcularGananciaTotalAsync();
        }

        public async Task<List<Inversion>> ListarInversionesVencidasAsync()
        {
            return await _inversionService.ListarInversionesVencidasAsync();
        }

        public double CalcularGanancia(Inversion inversion)
        {
            if (inversion == null)
                return 0;

            return _inversionService.CalcularGanancia(inversion);
        }

        public double CalcularMontoTotal(Inversion inversion)
        {
            if (inversion == null)
                return 0;

            return _inversionService.CalcularMontoTotal(inversion);
        }
    }
}

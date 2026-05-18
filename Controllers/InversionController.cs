using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    public class InversionController
    {
        private readonly InversionService _inversionService;
        private readonly int _usuarioId;

        public InversionController(InversionService inversionService, int usuarioId)
        {
            _inversionService = inversionService;
            _usuarioId = usuarioId;
        }

        public async Task<bool> CrearInversionAsync(double montoInicial, double tasaInteres, string descripcion, DateTime? fechaVencimiento = null)
        {
            if (montoInicial <= 0 || tasaInteres < 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _inversionService.CrearInversionAsync(montoInicial, tasaInteres, descripcion.Trim(), _usuarioId, fechaVencimiento);
        }

        public async Task<List<Inversion>> ListarInversionesAsync()
        {
            return await _inversionService.ListarInversionesAsync(_usuarioId);
        }

        public async Task<bool> ActualizarValorInversionAsync(int inversionId, double montoInicial)
        {
            if (inversionId <= 0 || montoInicial <= 0)
                return false;

            return await _inversionService.ActualizarValorInversionAsync(inversionId, montoInicial);
        }

        public async Task<double> GetTotalInvertidoAsync()
        {
            return await _inversionService.CalcularTotalInvertidoAsync(_usuarioId);
        }

        public async Task<double> GetValorTotalActualAsync()
        {
            return await _inversionService.CalcularValorTotalActualAsync(_usuarioId);
        }

        public async Task<double> GetGananciaTotalAsync()
        {
            return await _inversionService.CalcularGananciaTotalAsync(_usuarioId);
        }

        public async Task<List<Inversion>> ListarInversionesVencidasAsync()
        {
            return await _inversionService.ListarInversionesVencidasAsync(_usuarioId);
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

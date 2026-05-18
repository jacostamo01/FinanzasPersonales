using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    public class AhorroController
    {
        private readonly AhorroService _ahorroService;
        private readonly int _usuarioId;

        public AhorroController(AhorroService ahorroService, int usuarioId)
        {
            _ahorroService = ahorroService;
            _usuarioId = usuarioId;
        }

        public async Task<bool> CrearMetaAhorroAsync(double montoObjetivo, string descripcion, DateTime? fechaObjetivo = null)
        {
            if (montoObjetivo <= 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _ahorroService.CrearMetaAhorroAsync(montoObjetivo, descripcion.Trim(), _usuarioId, fechaObjetivo);
        }

        public async Task<List<Ahorro>> ListarAhorrosAsync()
        {
            return await _ahorroService.ListarAhorrosAsync(_usuarioId);
        }

        public async Task<bool> DepositarAsync(int ahorroId, double monto)
        {
            if (monto <= 0)
                return false;

            return await _ahorroService.DepositarAhorroAsync(ahorroId, monto, _usuarioId);
        }

        public async Task<bool> RetirarAsync(int ahorroId, double monto)
        {
            if (monto <= 0)
                return false;

            return await _ahorroService.RetirarAhorroAsync(ahorroId, monto);
        }

        public async Task<double> GetDisponibleAsync()
        {
            return await _ahorroService.CalcularDisponibleAsync(_usuarioId);
        }

        public async Task<double> GetTotalAhorradoAsync()
        {
            return await _ahorroService.CalcularTotalAhorradoAsync(_usuarioId);
        }

        public async Task<List<Ahorro>> ListarAhorrosCompletosAsync()
        {
            return await _ahorroService.ListarAhorrosCompletosAsync(_usuarioId);
        }
    }
}

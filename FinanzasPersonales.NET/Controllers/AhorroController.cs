using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    /// <summary>
    /// Controlador del módulo de ahorros. Conecta la Vista con el Servicio.
    /// </summary>
    public class AhorroController
    {
        private readonly AhorroService _ahorroService;

        public AhorroController(AhorroService ahorroService)
        {
            _ahorroService = ahorroService;
        }

        public async Task<bool> CrearMetaAhorroAsync(double montoObjetivo, string descripcion, DateTime? fechaObjetivo = null)
        {
            if (montoObjetivo <= 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _ahorroService.CrearMetaAhorroAsync(montoObjetivo, descripcion.Trim(), fechaObjetivo);
        }

        public async Task<List<Ahorro>> ListarAhorrosAsync()
        {
            return await _ahorroService.ListarAhorrosAsync();
        }

        public async Task<bool> DepositarAsync(int ahorroId, double monto)
        {
            if (monto <= 0)
                return false;

            return await _ahorroService.DepositarAhorroAsync(ahorroId, monto);
        }

        public async Task<bool> RetirarAsync(int ahorroId, double monto)
        {
            if (monto <= 0)
                return false;

            return await _ahorroService.RetirarAhorroAsync(ahorroId, monto);
        }

        public async Task<double> GetDisponibleAsync()
        {
            return await _ahorroService.CalcularDisponibleAsync();
        }

        public async Task<double> GetTotalAhorradoAsync()
        {
            return await _ahorroService.CalcularTotalAhorradoAsync();
        }

        public async Task<List<Ahorro>> ListarAhorrosCompletosAsync()
        {
            return await _ahorroService.ListarAhorrosCompletosAsync();
        }
    }
}

using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    public class ColchonFinancieroController
    {
        private readonly ColchonFinancieroService _colchonService;
        private readonly int _usuarioId;

        public ColchonFinancieroController(ColchonFinancieroService colchonService, int usuarioId)
        {
            _colchonService = colchonService;
            _usuarioId = usuarioId;
        }

        public async Task<bool> CrearColchonAsync(double meta)
        {
            if (meta <= 0)
                return false;

            return await _colchonService.CrearColchonAsync(_usuarioId, meta);
        }

        public async Task<ColchonFinanciero?> ObtenerColchonAsync()
        {
            return await _colchonService.ObtenerColchonAsync(_usuarioId);
        }

        public async Task<List<ColchonFinanciero>> ListarColchonesAsync()
        {
            return await _colchonService.ListarColchonesAsync(_usuarioId);
        }

        public async Task<bool> DepositarAsync(int colchonId, double monto)
        {
            if (monto <= 0)
                return false;

            return await _colchonService.DepositarColchonAsync(colchonId, monto, _usuarioId);
        }

        public async Task<bool> RetirarAsync(int colchonId, double monto)
        {
            if (monto <= 0)
                return false;

            return await _colchonService.RetirarColchonAsync(colchonId, monto);
        }

        public async Task<double> GetDisponibleAsync()
        {
            return await _colchonService.CalcularDisponibleAsync(_usuarioId);
        }

        public async Task<double> GetTotalColchonAsync()
        {
            return await _colchonService.CalcularTotalColchonAsync(_usuarioId);
        }

        public async Task<List<ColchonFinanciero>> ListarColchonesCompletosAsync()
        {
            return await _colchonService.ListarColchonesCompletosAsync(_usuarioId);
        }
    }
}

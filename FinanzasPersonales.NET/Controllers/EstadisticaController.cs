using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    /// <summary>
    /// Controlador del módulo de estadísticas. Conecta la Vista con el Servicio.
    /// </summary>
    public class EstadisticaController
    {
        private readonly EstadisticaService _estadisticaService;

        public EstadisticaController(EstadisticaService estadisticaService)
        {
            _estadisticaService = estadisticaService;
        }

        public async Task<double> GetTotalIngresosAsync()
        {
            return await _estadisticaService.CalcularTotalIngresosAsync();
        }

        public async Task<double> GetTotalGastosAsync()
        {
            return await _estadisticaService.CalcularTotalGastosAsync();
        }

        public async Task<double> GetBalanceAsync()
        {
            return await _estadisticaService.CalcularBalanceAsync();
        }

        public async Task<double> GetPromedioIngresosAsync()
        {
            return await _estadisticaService.CalcularPromedioIngresosAsync();
        }

        public async Task<double> GetPromedioGastosAsync()
        {
            return await _estadisticaService.CalcularPromedioGastosAsync();
        }

        public async Task<int> ContarMovimientosAsync()
        {
            return await _estadisticaService.ContarMovimientosAsync();
        }

        public double CalcularPorcentajeGastos(double gastos, double ingresos)
        {
            return _estadisticaService.CalcularPorcentajeGastos(gastos, ingresos);
        }

        public async Task<List<(string Categoria, double Total)>> GetGastosPorCategoriaAsync()
        {
            return await _estadisticaService.ObtenerGastosPorCategoriaAsync();
        }

        public async Task<List<(int Mes, int Anio, double TotalIngresos, double TotalGastos)>> GetResumenMensualAsync()
        {
            return await _estadisticaService.ObtenerResumenMensualAsync();
        }
    }
}

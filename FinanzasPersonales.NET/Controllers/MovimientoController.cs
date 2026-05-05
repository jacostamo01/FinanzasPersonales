using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    /// <summary>
    /// Controlador del módulo de movimientos (ingresos y gastos).
    /// </summary>
    public class MovimientoController
    {
        private readonly MovimientoService _movimientoService;

        public MovimientoController(MovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        public async Task<bool> AgregarIngresoAsync(double monto, string descripcion)
        {
            if (monto <= 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _movimientoService.AgregarIngresoAsync(monto, descripcion.Trim());
        }

        public async Task<bool> AgregarGastoAsync(double monto, string descripcion, string categoria = "")
        {
            if (monto <= 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _movimientoService.AgregarGastoAsync(monto, descripcion.Trim(), categoria?.Trim() ?? "");
        }

        public async Task<List<Movimiento>> ListarMovimientosAsync()
        {
            return await _movimientoService.ListarMovimientosAsync();
        }

        public async Task<List<Ingreso>> ListarIngresosAsync()
        {
            return await _movimientoService.ListarIngresosAsync();
        }

        public async Task<List<Gasto>> ListarGastosAsync()
        {
            return await _movimientoService.ListarGastosAsync();
        }

        public async Task<double> ObtenerTotalIngresosAsync()
        {
            return await _movimientoService.CalcularTotalIngresosAsync();
        }

        public async Task<double> ObtenerTotalGastosAsync()
        {
            return await _movimientoService.CalcularTotalGastosAsync();
        }

        public async Task<double> ObtenerBalanceAsync()
        {
            return await _movimientoService.CalcularBalanceAsync();
        }

        public async Task<List<Movimiento>> ListarMovimientosPorMesAsync(int mes, int anio)
        {
            if (mes < 1 || mes > 12 || anio < 1900)
                return new List<Movimiento>();

            return await _movimientoService.ListarMovimientosPorMesAsync(mes, anio);
        }
    }
}

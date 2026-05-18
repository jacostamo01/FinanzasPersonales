using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    public class MovimientoController
    {
        private readonly MovimientoService _movimientoService;
        private readonly int _usuarioId;

        public MovimientoController(MovimientoService movimientoService, int usuarioId)
        {
            _movimientoService = movimientoService;
            _usuarioId = usuarioId;
        }

        public async Task<bool> AgregarIngresoAsync(double monto, string descripcion)
        {
            if (monto <= 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _movimientoService.AgregarIngresoAsync(monto, descripcion.Trim(), _usuarioId);
        }

        public async Task<bool> AgregarGastoAsync(double monto, string descripcion, string categoria = "")
        {
            if (monto <= 0 || string.IsNullOrWhiteSpace(descripcion))
                return false;

            return await _movimientoService.AgregarGastoAsync(monto, descripcion.Trim(), _usuarioId, categoria?.Trim() ?? "");
        }

        public async Task<List<Movimiento>> ListarMovimientosAsync()
        {
            return await _movimientoService.ListarMovimientosAsync(_usuarioId);
        }

        public async Task<List<Ingreso>> ListarIngresosAsync()
        {
            return await _movimientoService.ListarIngresosAsync(_usuarioId);
        }

        public async Task<List<Gasto>> ListarGastosAsync()
        {
            return await _movimientoService.ListarGastosAsync(_usuarioId);
        }

        public async Task<double> ObtenerTotalIngresosAsync()
        {
            return await _movimientoService.CalcularTotalIngresosAsync(_usuarioId);
        }

        public async Task<double> ObtenerTotalGastosAsync()
        {
            return await _movimientoService.CalcularTotalGastosAsync(_usuarioId);
        }

        public async Task<double> ObtenerBalanceAsync()
        {
            return await _movimientoService.CalcularBalanceAsync(_usuarioId);
        }

        public async Task<List<Movimiento>> ListarMovimientosPorMesAsync(int mes, int anio)
        {
            if (mes < 1 || mes > 12 || anio < 1900)
                return new List<Movimiento>();

            return await _movimientoService.ListarMovimientosPorMesAsync(mes, anio, _usuarioId);
        }
    }
}

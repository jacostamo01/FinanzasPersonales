using FinanzasPersonales.NET.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinanzasPersonales.NET.Controllers
{
    /// <summary>
    /// Controlador del módulo de estadísticas. Conecta la Vista con el Servicio.
    /// </summary>
    public class EstadisticaController
    {
        private readonly EstadisticaService _estadisticaService;
        private readonly IEmailService _emailService;
        private readonly int _usuarioId;

        public EstadisticaController(EstadisticaService estadisticaService, int usuarioId)
            : this(estadisticaService, new EmailService(), usuarioId)
        {
        }

        public EstadisticaController(EstadisticaService estadisticaService, IEmailService emailService, int usuarioId)
        {
            _estadisticaService = estadisticaService;
            _emailService = emailService;
            _usuarioId = usuarioId;
        }

        public async Task<double> GetTotalIngresosAsync()
        {
            return await _estadisticaService.CalcularTotalIngresosAsync(_usuarioId);
        }

        public async Task<double> GetTotalGastosAsync()
        {
            return await _estadisticaService.CalcularTotalGastosAsync(_usuarioId);
        }

        public async Task<double> GetBalanceAsync()
        {
            return await _estadisticaService.CalcularBalanceAsync(_usuarioId);
        }

        public async Task<double> GetPromedioIngresosAsync()
        {
            return await _estadisticaService.CalcularPromedioIngresosAsync(_usuarioId);
        }

        public async Task<double> GetPromedioGastosAsync()
        {
            return await _estadisticaService.CalcularPromedioGastosAsync(_usuarioId);
        }

        public async Task<int> ContarMovimientosAsync()
        {
            return await _estadisticaService.ContarMovimientosAsync(_usuarioId);
        }

        public double CalcularPorcentajeGastos(double gastos, double ingresos)
        {
            return _estadisticaService.CalcularPorcentajeGastos(gastos, ingresos);
        }

        public async Task<List<(string Categoria, double Total)>> GetGastosPorCategoriaAsync()
        {
            return await _estadisticaService.ObtenerGastosPorCategoriaAsync(_usuarioId);
        }

        public async Task<List<(int Mes, int Anio, double TotalIngresos, double TotalGastos)>> GetResumenMensualAsync()
        {
            return await _estadisticaService.ObtenerResumenMensualAsync(_usuarioId);
        }

        public async Task<string> GenerarReporteCompletoStringAsync()
        {
            var totalIngresos = await GetTotalIngresosAsync();
            var totalGastos = await GetTotalGastosAsync();
            var balance = await GetBalanceAsync();
            var promedioIngresos = await GetPromedioIngresosAsync();
            var promedioGastos = await GetPromedioGastosAsync();
            var totalMovimientos = await ContarMovimientosAsync();
            var gastosPorCategoria = await GetGastosPorCategoriaAsync();
            var resumenMensual = await GetResumenMensualAsync();

            var sb = new StringBuilder();

            sb.AppendLine("=== RESUMEN GENERAL ===");
            sb.AppendLine($"Total ingresos:    ${totalIngresos:F2}");
            sb.AppendLine($"Total gastos:      ${totalGastos:F2}");
            sb.AppendLine($"Balance:           ${balance:F2}");
            sb.AppendLine($"Prom. ingresos:    ${promedioIngresos:F2}");
            sb.AppendLine($"Prom. gastos:      ${promedioGastos:F2}");
            sb.AppendLine($"Total movimientos: {totalMovimientos}");

            if (totalIngresos > 0)
            {
                var porcentaje = CalcularPorcentajeGastos(totalGastos, totalIngresos);
                sb.AppendLine($"% gastos/ingresos: {porcentaje:F1}%");
            }

            if (gastosPorCategoria.Any())
            {
                sb.AppendLine();
                sb.AppendLine("=== GASTOS POR CATEGORÍA ===");
                foreach (var (categoria, total) in gastosPorCategoria.OrderByDescending(x => x.Total))
                {
                    var porcentaje = totalGastos > 0 ? (total / totalGastos) * 100 : 0;
                    sb.AppendLine($"{categoria}: ${total:F2} ({porcentaje:F1}%)");
                }
            }

            if (resumenMensual.Any())
            {
                sb.AppendLine();
                sb.AppendLine("=== RESUMEN MENSUAL ===");
                foreach (var (mes, anio, ingresos, gastos) in resumenMensual.TakeLast(6))
                {
                    var balanceMensual = ingresos - gastos;
                    var nombreMes = new DateTime(anio, mes, 1).ToString("MMM/yyyy");
                    sb.AppendLine($"{nombreMes}: +${ingresos:F2} / -${gastos:F2} = ${balanceMensual:F2}");
                }
            }

            return sb.ToString();
        }

        public async Task EnviarReportePorCorreoAsync(string correoDestino)
        {
            var reporte = await GenerarReporteCompletoStringAsync();
            var asunto = $"Reporte Financiero - {DateTime.Now:dd/MM/yyyy}";
            await _emailService.EnviarCorreoAsync(correoDestino, asunto, reporte);
        }
    }
}

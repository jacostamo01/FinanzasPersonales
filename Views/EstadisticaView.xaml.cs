using FinanzasPersonales.NET.Controllers;
using System;
using System.Linq;
using System.Text;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class EstadisticaView : Window
    {
        private readonly EstadisticaController _controller;

        public EstadisticaView(EstadisticaController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private async void BtnResumenGeneral_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos = await _controller.GetTotalIngresosAsync();
            var totalGastos = await _controller.GetTotalGastosAsync();
            var balance = await _controller.GetBalanceAsync();
            var promedioIngresos = await _controller.GetPromedioIngresosAsync();
            var promedioGastos = await _controller.GetPromedioGastosAsync();
            var totalMovimientos = await _controller.ContarMovimientosAsync();

            var sb = new StringBuilder();
            sb.AppendLine("=== RESUMEN GENERAL ===");
            sb.AppendLine($"Total de ingresos:     ${totalIngresos:F2}");
            sb.AppendLine($"Total de gastos:       ${totalGastos:F2}");
            sb.AppendLine($"Balance actual:        ${balance:F2}");
            sb.AppendLine($"Promedio de ingresos:  ${promedioIngresos:F2}");
            sb.AppendLine($"Promedio de gastos:    ${promedioGastos:F2}");
            sb.AppendLine($"Total de movimientos:  {totalMovimientos}");

            if (totalIngresos > 0)
            {
                var porcentajeGastos = _controller.CalcularPorcentajeGastos(totalGastos, totalIngresos);
                sb.AppendLine($"% de gastos/ingresos:  {porcentajeGastos:F1}%");
            }

            MessageBox.Show(sb.ToString(), "Resumen General");
        }

        private async void BtnGastosPorCategoria_Click(object sender, RoutedEventArgs e)
        {
            var totalGastos = await _controller.GetTotalGastosAsync();
            var gastosPorCategoria = await _controller.GetGastosPorCategoriaAsync();

            if (!gastosPorCategoria.Any())
            {
                txtMensaje.Text = "No hay gastos registrados por categoría.";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("=== GASTOS POR CATEGORÍA ===");
            sb.AppendLine();

            foreach (var (categoria, total) in gastosPorCategoria.OrderByDescending(x => x.Total))
            {
                var porcentaje = totalGastos > 0 ? (total / totalGastos) * 100 : 0;
                sb.AppendLine($"{categoria}: ${total:F2} ({porcentaje:F1}%)");
            }

            MessageBox.Show(sb.ToString(), "Gastos por Categoría");
        }

        private async void BtnResumenMensual_Click(object sender, RoutedEventArgs e)
        {
            var resumenMensual = await _controller.GetResumenMensualAsync();

            if (!resumenMensual.Any())
            {
                txtMensaje.Text = "No hay datos mensuales disponibles.";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("=== RESUMEN MENSUAL (últimos 6 meses) ===");
            sb.AppendLine();

            foreach (var (mes, anio, ingresos, gastos) in resumenMensual.TakeLast(6))
            {
                var balanceMensual = ingresos - gastos;
                var nombreMes = new DateTime(anio, mes, 1).ToString("MMM/yyyy");
                sb.AppendLine($"{nombreMes}: Ingresos ${ingresos:F2} | Gastos ${gastos:F2} | Balance ${balanceMensual:F2}");
            }

            MessageBox.Show(sb.ToString(), "Resumen Mensual");
        }

        private async void BtnVerTodo_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos = await _controller.GetTotalIngresosAsync();
            var totalGastos = await _controller.GetTotalGastosAsync();
            var balance = await _controller.GetBalanceAsync();
            var promedioIngresos = await _controller.GetPromedioIngresosAsync();
            var promedioGastos = await _controller.GetPromedioGastosAsync();
            var totalMovimientos = await _controller.ContarMovimientosAsync();
            var gastosPorCategoria = await _controller.GetGastosPorCategoriaAsync();
            var resumenMensual = await _controller.GetResumenMensualAsync();

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
                var porcentaje = _controller.CalcularPorcentajeGastos(totalGastos, totalIngresos);
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

            MessageBox.Show(sb.ToString(), "Estadísticas Completas");
        }

        private void BtnVerGraficas_Click(object sender, RoutedEventArgs e)
        {
            var graficasView = new GraficasView(_controller);
            graficasView.ShowDialog();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnEnviarCorreo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var correoDestino = txtCorreoDestino.Text.Trim();
                if (string.IsNullOrEmpty(correoDestino) || !correoDestino.Contains("@"))
                {
                    MessageBox.Show("Por favor, ingresa una dirección de correo válida.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                txtMensaje.Text = "Enviando correo, por favor espera...";

                var btn = sender as System.Windows.Controls.Button;
                if (btn != null) btn.IsEnabled = false;

                await _controller.EnviarReportePorCorreoAsync(correoDestino);

                txtMensaje.Text = "";
                MessageBox.Show("¡El reporte ha sido enviado exitosamente al correo!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCorreoDestino.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el correo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMensaje.Text = "No se pudo enviar el correo.";
            }
            finally
            {
                var btn = sender as System.Windows.Controls.Button;
                if (btn != null) btn.IsEnabled = true;
            }
        }
    }
}

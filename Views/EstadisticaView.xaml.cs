using FinanzasPersonales.NET.Controllers;
using System;
using System.Linq;
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
            LimpiarTabla();
        }

        private async void BtnResumenGeneral_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos = await _controller.GetTotalIngresosAsync();
            var totalGastos = await _controller.GetTotalGastosAsync();
            var balance = await _controller.GetBalanceAsync();
            var promedioIngresos = await _controller.GetPromedioIngresosAsync();
            var promedioGastos = await _controller.GetPromedioGastosAsync();
            var totalMovimientos = await _controller.ContarMovimientosAsync();
            var porcentaje = totalIngresos > 0
                ? $"{_controller.CalcularPorcentajeGastos(totalGastos, totalIngresos):F1}%"
                : "N/A";

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = "Resumen general";
            dgDatos.ItemsSource = new[]
            {
                new { Indicador = "Total ingresos", Valor = $"${totalIngresos:F2}" },
                new { Indicador = "Total gastos", Valor = $"${totalGastos:F2}" },
                new { Indicador = "Balance actual", Valor = $"${balance:F2}" },
                new { Indicador = "Promedio ingresos", Valor = $"${promedioIngresos:F2}" },
                new { Indicador = "Promedio gastos", Valor = $"${promedioGastos:F2}" },
                new { Indicador = "Total movimientos", Valor = totalMovimientos.ToString() },
                new { Indicador = "% gastos / ingresos", Valor = porcentaje }
            };
        }

        private async void BtnGastosPorCategoria_Click(object sender, RoutedEventArgs e)
        {
            var totalGastos = await _controller.GetTotalGastosAsync();
            var gastosPorCategoria = await _controller.GetGastosPorCategoriaAsync();

            if (!gastosPorCategoria.Any())
            {
                txtMensaje.Text = "No hay gastos registrados por categoría.";
                LimpiarTabla();
                return;
            }

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = "Gastos por categoría";
            dgDatos.ItemsSource = gastosPorCategoria
                .OrderByDescending(x => x.Total)
                .Select(x => new
                {
                    Categoria = x.Categoria,
                    Total = $"${x.Total:F2}",
                    Porcentaje = totalGastos > 0 ? $"{(x.Total / totalGastos) * 100:F1}%" : "0%"
                })
                .ToList();
        }

        private async void BtnResumenMensual_Click(object sender, RoutedEventArgs e)
        {
            var resumenMensual = await _controller.GetResumenMensualAsync();

            if (!resumenMensual.Any())
            {
                txtMensaje.Text = "No hay datos mensuales disponibles.";
                LimpiarTabla();
                return;
            }

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = "Resumen mensual";
            dgDatos.ItemsSource = resumenMensual
                .TakeLast(6)
                .Select(r => new
                {
                    Mes = new DateTime(r.Anio, r.Mes, 1).ToString("MMM/yyyy"),
                    Ingresos = $"${r.TotalIngresos:F2}",
                    Gastos = $"${r.TotalGastos:F2}",
                    Balance = $"${r.TotalIngresos - r.TotalGastos:F2}"
                })
                .ToList();
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

            var filas = new System.Collections.Generic.List<object>
            {
                new { Seccion = "General", Indicador = "Total ingresos", Valor = $"${totalIngresos:F2}" },
                new { Seccion = "General", Indicador = "Total gastos", Valor = $"${totalGastos:F2}" },
                new { Seccion = "General", Indicador = "Balance", Valor = $"${balance:F2}" },
                new { Seccion = "General", Indicador = "Promedio ingresos", Valor = $"${promedioIngresos:F2}" },
                new { Seccion = "General", Indicador = "Promedio gastos", Valor = $"${promedioGastos:F2}" },
                new { Seccion = "General", Indicador = "Movimientos totales", Valor = totalMovimientos.ToString() },
                new { Seccion = "General", Indicador = "% gastos/ingresos", Valor = totalIngresos > 0 ? $"{_controller.CalcularPorcentajeGastos(totalGastos, totalIngresos):F1}%" : "N/A" }
            };

            foreach (var gasto in gastosPorCategoria.OrderByDescending(x => x.Total))
            {
                var porcentaje = totalGastos > 0 ? $"{(gasto.Total / totalGastos) * 100:F1}%" : "0%";
                filas.Add(new { Seccion = "Categorías", Indicador = gasto.Categoria, Valor = $"${gasto.Total:F2} ({porcentaje})" });
            }

            foreach (var resumen in resumenMensual.TakeLast(6))
            {
                var mes = new DateTime(resumen.Anio, resumen.Mes, 1).ToString("MMM/yyyy");
                filas.Add(new { Seccion = "Mensual", Indicador = mes, Valor = $"+${resumen.TotalIngresos:F2} / -${resumen.TotalGastos:F2} = ${resumen.TotalIngresos - resumen.TotalGastos:F2}" });
            }

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = "Estadísticas completas";
            dgDatos.ItemsSource = filas;
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

        private void LimpiarTabla()
        {
            txtTablaTitulo.Text = string.Empty;
            dgDatos.ItemsSource = null;
        }
    }
}
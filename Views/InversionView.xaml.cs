using FinanzasPersonales.NET.Controllers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class InversionView : Window
    {
        private readonly InversionController _controller;

        public InversionView(InversionController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private async void BtnCrearInversion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                txtMensaje.Text = "La descripción no puede estar vacía.";
                return;
            }

            if (!double.TryParse(txtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                txtMensaje.Text = "Monto inválido. Ingresa un número mayor a 0.";
                return;
            }

            if (!double.TryParse(txtTasa.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double tasa) || tasa < 0)
            {
                txtMensaje.Text = "Tasa inválida. Ingresa un número mayor o igual a 0.";
                return;
            }

            var exito = await _controller.CrearInversionAsync(monto, tasa, txtDescripcion.Text);
            txtMensaje.Text = exito ? "Inversión creada exitosamente." : "Error al crear la inversión.";

            if (exito)
            {
                txtDescripcion.Clear();
                txtMonto.Clear();
                txtTasa.Clear();
            }
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtInversionId.Text, out int id))
            {
                txtMensaje.Text = "ID de inversión inválido.";
                return;
            }

            if (!double.TryParse(txtNuevoMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                txtMensaje.Text = "Monto inválido.";
                return;
            }

            var exito = await _controller.ActualizarValorInversionAsync(id, monto);
            txtMensaje.Text = exito ? "Inversión actualizada exitosamente." : "Error al actualizar la inversión.";
        }

        private async void BtnVerInversiones_Click(object sender, RoutedEventArgs e)
        {
            var inversiones = await _controller.ListarInversionesAsync();

            if (!inversiones.Any())
            {
                txtMensaje.Text = "No hay inversiones registradas.";
                dgDatos.ItemsSource = null;
                txtTablaTitulo.Text = "";
                return;
            }

            txtTablaTitulo.Text = $"Inversiones ({inversiones.Count})";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = inversiones.Select(inv => new
            {
                ID          = inv.Id,
                Descripción = inv.Descripcion,
                Inicial     = $"${inv.MontoInicial:F2}",
                Actual      = $"${inv.ValorActual:F2}",
                Tasa        = $"{inv.TasaInteres:P2}",
                Rendimiento = $"{inv.GetRendimientoPorcentual():F2}%",
                Inicio      = inv.FechaInicio.ToString("dd/MM/yyyy")
            }).ToList();
        }

        private async void BtnVerResumen_Click(object sender, RoutedEventArgs e)
        {
            var totalInvertido = await _controller.GetTotalInvertidoAsync();
            var valorActual    = await _controller.GetValorTotalActualAsync();
            var gananciaTotal  = await _controller.GetGananciaTotalAsync();

            var rendimiento = totalInvertido > 0
                ? $"{(gananciaTotal / totalInvertido) * 100:F2}%"
                : "N/A";

            txtTablaTitulo.Text = "Resumen de inversiones";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = new[]
            {
                new { Indicador = "Total invertido",             Valor = $"${totalInvertido:F2}" },
                new { Indicador = "Valor actual total",          Valor = $"${valorActual:F2}"    },
                new { Indicador = gananciaTotal >= 0 ? "Ganancia total" : "Pérdida total",
                                                                 Valor = $"${Math.Abs(gananciaTotal):F2}" },
                new { Indicador = "Rendimiento total",           Valor = rendimiento             }
            };
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
using FinanzasPersonales.NET.Controllers;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
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
                return;
            }

            var sb = new StringBuilder();
            foreach (var inv in inversiones)
            {
                sb.AppendLine($"ID: {inv.Id} | {inv.Descripcion}");
                sb.AppendLine($"   Inicial: ${inv.MontoInicial:F2} | Actual: ${inv.ValorActual:F2}");
                sb.AppendLine($"   Tasa: {inv.TasaInteres:P2} | Rendimiento: {inv.GetRendimientoPorcentual():F2}%");
                sb.AppendLine();
            }

            MessageBox.Show(sb.ToString(), "Inversiones");
        }

        private async void BtnVerResumen_Click(object sender, RoutedEventArgs e)
        {
            var totalInvertido = await _controller.GetTotalInvertidoAsync();
            var valorActual = await _controller.GetValorTotalActualAsync();
            var gananciaTotal = await _controller.GetGananciaTotalAsync();

            var sb = new StringBuilder();
            sb.AppendLine($"Total invertido: ${totalInvertido:F2}");
            sb.AppendLine($"Valor actual:    ${valorActual:F2}");

            if (gananciaTotal >= 0)
                sb.AppendLine($"Ganancia total:  ${gananciaTotal:F2}");
            else
                sb.AppendLine($"Pérdida total:   ${Math.Abs(gananciaTotal):F2}");

            if (totalInvertido > 0)
            {
                var rendimiento = (gananciaTotal / totalInvertido) * 100;
                sb.AppendLine($"Rendimiento:     {rendimiento:F2}%");
            }

            MessageBox.Show(sb.ToString(), "Resumen de Inversiones");
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
using FinanzasPersonales.NET.Controllers;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class ViewMovimientos : Window
    {
        private readonly MovimientoController _controller;

        public ViewMovimientos(MovimientoController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private async void BtnAgregarIngreso_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                txtMensaje.Text = "Monto inválido. Ingresa un número mayor a 0.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                txtMensaje.Text = "La descripción no puede estar vacía.";
                return;
            }

            var exito = await _controller.AgregarIngresoAsync(monto, txtDescripcion.Text);

            txtMensaje.Text = exito
                ? $"Ingreso de ${monto:F2} agregado exitosamente."
                : "Error al agregar el ingreso.";

            LimpiarFormulario();
        }

        private async void BtnAgregarGasto_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                txtMensaje.Text = "Monto inválido. Ingresa un número mayor a 0.";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                txtMensaje.Text = "La descripción no puede estar vacía.";
                return;
            }

            var categoriaSeleccionada = (cmbCategoria.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() ?? "Otros";
            var exito = await _controller.AgregarGastoAsync(monto, txtDescripcion.Text, categoriaSeleccionada);

            txtMensaje.Text = exito
                ? $"Gasto de ${monto:F2} agregado exitosamente."
                : "Error al agregar el gasto.";

            LimpiarFormulario();
        }

        private async void BtnVerTodos_Click(object sender, RoutedEventArgs e)
        {
            var movimientos = await _controller.ListarMovimientosAsync();

            if (!movimientos.Any())
            {
                txtMensaje.Text = "No hay movimientos registrados.";
                return;
            }

            var sb = new StringBuilder();
            foreach (var m in movimientos)
                sb.AppendLine($"{m.Fecha:dd/MM/yyyy} | {m.GetTipo()} | ${m.Monto:F2} | {m.Descripcion}");

            MessageBox.Show(sb.ToString(), "Todos los Movimientos");
        }

        private async void BtnVerIngresos_Click(object sender, RoutedEventArgs e)
        {
            var ingresos = await _controller.ListarIngresosAsync();

            if (!ingresos.Any())
            {
                txtMensaje.Text = "No hay ingresos registrados.";
                return;
            }

            var sb = new StringBuilder();
            foreach (var i in ingresos)
                sb.AppendLine($"{i.Fecha:dd/MM/yyyy} | ${i.Monto:F2} | {i.Descripcion}");

            MessageBox.Show(sb.ToString(), "Ingresos");
        }

        private async void BtnVerGastos_Click(object sender, RoutedEventArgs e)
        {
            var gastos = await _controller.ListarGastosAsync();

            if (!gastos.Any())
            {
                txtMensaje.Text = "No hay gastos registrados.";
                return;
            }

            var sb = new StringBuilder();
            foreach (var g in gastos)
                sb.AppendLine($"{g.Fecha:dd/MM/yyyy} | ${g.Monto:F2} | {g.Categoria} | {g.Descripcion}");

            MessageBox.Show(sb.ToString(), "Gastos");
        }

        private async void BtnVerResumen_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos = await _controller.ObtenerTotalIngresosAsync();
            var totalGastos = await _controller.ObtenerTotalGastosAsync();
            var balance = await _controller.ObtenerBalanceAsync();

            var mensaje = $"Total Ingresos: ${totalIngresos:F2}\n" +
                          $"Total Gastos:   ${totalGastos:F2}\n" +
                          $"------------------------\n" +
                          $"Balance:        ${balance:F2}";

            MessageBox.Show(mensaje, "Resumen Financiero");
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LimpiarFormulario()
        {
            txtMonto.Clear();
            txtDescripcion.Clear();
            cmbCategoria.SelectedIndex = -1;
        }
    }
}
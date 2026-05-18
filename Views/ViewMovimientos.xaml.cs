using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FinanzasPersonales.NET.Views
{
    public partial class ViewMovimientos : Window
    {
        private readonly MovimientoController _controller;

        public ViewMovimientos(MovimientoController controller)
        {
            InitializeComponent();
            _controller = controller;
            CargarCategorias();
            LimpiarTabla();
        }

        private void CargarCategorias()
        {
            foreach (var categoria in CategoriaGasto.ObtenerCategorias())
            {
                // Encabezado de categoría padre (no seleccionable)
                var encabezado = new ComboBoxItem
                {
                    Content = $"── {categoria.Nombre} ──",
                    IsEnabled = false,
                    FontWeight = System.Windows.FontWeights.Bold
                };
                cmbCategoria.Items.Add(encabezado);

                // Subcategorías seleccionables
                foreach (var sub in categoria.Subcategorias)
                {
                    var item = new ComboBoxItem
                    {
                        Content = sub,
                        Tag = $"{categoria.Nombre}: {sub}"
                    };
                    cmbCategoria.Items.Add(item);
                }
            }
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

            chkUsarCategoria.IsChecked = false;
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

            if (chkUsarCategoria.IsChecked != true)
            {
                txtMensaje.Text = "Activa la casilla de categoría para registrar el gasto.";
                return;
            }

            if (cmbCategoria.SelectedItem is not ComboBoxItem item || item.Tag is not string categoria)
            {
                txtMensaje.Text = "Selecciona una categoría para el gasto.";
                cmbCategoria.IsDropDownOpen = true;
                return;
            }

            var exito = await _controller.AgregarGastoAsync(monto, txtDescripcion.Text, categoria);

            txtMensaje.Text = exito
                ? $"Gasto de ${monto:F2} ({categoria}) agregado exitosamente."
                : "Error al agregar el gasto.";

            chkUsarCategoria.IsChecked = false;
            LimpiarFormulario();
        }

        private async void BtnVerTodos_Click(object sender, RoutedEventArgs e)
        {
            var movimientos = await _controller.ListarMovimientosAsync();

            if (!movimientos.Any())
            {
                txtMensaje.Text = "No hay movimientos registrados.";
                LimpiarTabla();
                return;
            }

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = $"Todos los movimientos ({movimientos.Count})";
            dgDatos.ItemsSource = movimientos.Select(m => new
            {
                Fecha = m.Fecha.ToString("dd/MM/yyyy"),
                Tipo = m.GetTipo(),
                Monto = $"${m.Monto:F2}",
                Descripcion = m.Descripcion,
                Categoria = m is Gasto gasto ? gasto.Categoria : "-"
            }).ToList();
        }

        private async void BtnVerIngresos_Click(object sender, RoutedEventArgs e)
        {
            var ingresos = await _controller.ListarIngresosAsync();

            if (!ingresos.Any())
            {
                txtMensaje.Text = "No hay ingresos registrados.";
                LimpiarTabla();
                return;
            }

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = $"Ingresos ({ingresos.Count})";
            dgDatos.ItemsSource = ingresos.Select(i => new
            {
                Fecha = i.Fecha.ToString("dd/MM/yyyy"),
                Monto = $"${i.Monto:F2}",
                Descripcion = i.Descripcion
            }).ToList();
        }

        private async void BtnVerGastos_Click(object sender, RoutedEventArgs e)
        {
            var gastos = await _controller.ListarGastosAsync();

            if (!gastos.Any())
            {
                txtMensaje.Text = "No hay gastos registrados.";
                LimpiarTabla();
                return;
            }

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = $"Gastos ({gastos.Count})";
            dgDatos.ItemsSource = gastos.Select(g => new
            {
                Fecha = g.Fecha.ToString("dd/MM/yyyy"),
                Monto = $"${g.Monto:F2}",
                Categoria = g.Categoria,
                Descripcion = g.Descripcion
            }).ToList();
        }

        private async void BtnVerResumen_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos = await _controller.ObtenerTotalIngresosAsync();
            var totalGastos = await _controller.ObtenerTotalGastosAsync();
            var balance = await _controller.ObtenerBalanceAsync();

            txtMensaje.Text = string.Empty;
            txtTablaTitulo.Text = "Resumen financiero";
            dgDatos.ItemsSource = new[]
            {
                new { Indicador = "Total ingresos", Valor = $"${totalIngresos:F2}" },
                new { Indicador = "Total gastos", Valor = $"${totalGastos:F2}" },
                new { Indicador = "Balance", Valor = $"${balance:F2}" }
            };
        }

        private void ChkUsarCategoria_Checked(object sender, RoutedEventArgs e)
        {
            cmbCategoria.IsEnabled = true;
            btnAgregarIngreso.IsEnabled = false;
            btnAgregarGasto.IsEnabled = true;
        }

        private void ChkUsarCategoria_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbCategoria.SelectedIndex = -1;
            cmbCategoria.IsEnabled = false;
            btnAgregarIngreso.IsEnabled = true;
            btnAgregarGasto.IsEnabled = true;
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
            cmbCategoria.IsEnabled = false;
            btnAgregarIngreso.IsEnabled = true;
            btnAgregarGasto.IsEnabled = true;
        }

        private void LimpiarTabla()
        {
            txtTablaTitulo.Text = string.Empty;
            dgDatos.ItemsSource = null;
        }
    }
}
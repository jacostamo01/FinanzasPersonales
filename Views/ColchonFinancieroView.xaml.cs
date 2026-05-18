using FinanzasPersonales.NET.Controllers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class ColchonFinancieroView : Window
    {
        private readonly ColchonFinancieroController _controller;

        public ColchonFinancieroView(ColchonFinancieroController controller)
        {
            InitializeComponent();
            _controller = controller;
            CargarColchones();
        }

        private async void CargarColchones()
        {
            try
            {
                var colchones = await _controller.ListarColchonesAsync();

                DgColchones.ItemsSource = colchones.Select(c => new
                {
                    c.Id,
                    c.UsuarioId,
                    MontoActual = $"${c.MontoActual:F2}",
                    Meta = $"${c.Meta:F2}",
                    PorcentajeAhorro = $"{c.PorcentajeAhorro:F1}%",
                    Progreso = $"{c.GetPorcentajeProgreso():F1}%",
                    Estado = c.MetaAlcanzada ? "Completado" : "En progreso",
                    FechaCreacion = c.FechaCreacion.ToString("dd/MM/yyyy")
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar el modulo de colchon financiero.\n\nDetalle: " + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(TxtMeta.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double meta) ||
                !double.TryParse(TxtPorcentaje.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double porcentaje))
            {
                MessageBox.Show("Ingrese una meta y un porcentaje validos.");
                return;
            }

            bool creado = await _controller.CrearColchonAsync(meta, porcentaje);

            MessageBox.Show(creado
                ? "Colchon financiero creado correctamente."
                : "No se pudo crear el colchon financiero.");

            if (creado)
            {
                TxtMeta.Clear();
                TxtPorcentaje.Clear();
            }

            CargarColchones();
        }

        private async void BtnDepositar_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetColchonSeleccionado(out int colchonId))
            {
                MessageBox.Show("Seleccione un colchon financiero.");
                return;
            }

            if (!double.TryParse(TxtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto valido.");
                return;
            }

            var disponible = await _controller.GetDisponibleAsync();
            if (monto > disponible)
            {
                MessageBox.Show($"Fondos insuficientes. Disponible: ${disponible:F2}");
                return;
            }

            bool depositado = await _controller.DepositarAsync(colchonId, monto);

            MessageBox.Show(depositado
                ? "Deposito realizado correctamente."
                : "No se pudo realizar el deposito.");

            if (depositado)
            {
                TxtMonto.Clear();
            }

            CargarColchones();
        }

        private async void BtnRetirar_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetColchonSeleccionado(out int colchonId))
            {
                MessageBox.Show("Seleccione un colchon financiero.");
                return;
            }

            if (!double.TryParse(TxtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                MessageBox.Show("Ingrese un monto valido.");
                return;
            }

            bool retirado = await _controller.RetirarAsync(colchonId, monto);

            MessageBox.Show(retirado
                ? "Retiro realizado correctamente."
                : "No se pudo realizar el retiro.");

            if (retirado)
            {
                TxtMonto.Clear();
            }

            CargarColchones();
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool TryGetColchonSeleccionado(out int colchonId)
        {
            colchonId = 0;

            if (DgColchones.SelectedItem == null)
                return false;

            var idProperty = DgColchones.SelectedItem.GetType().GetProperty("Id");
            var idValue = idProperty?.GetValue(DgColchones.SelectedItem);

            return idValue is int id && (colchonId = id) > 0;
        }
    }
}

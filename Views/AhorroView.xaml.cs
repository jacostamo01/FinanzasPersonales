using FinanzasPersonales.NET.Controllers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class AhorroView : Window
    {
        private readonly AhorroController _controller;

        public AhorroView(AhorroController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private async void BtnCrearMeta_Click(object sender, RoutedEventArgs e)
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

            DateTime? fechaObjetivo = null;
            if (!string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                if (DateTime.TryParseExact(txtFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
                    fechaObjetivo = fecha;
                else
                {
                    txtMensaje.Text = "Fecha inválida. Usa el formato dd/MM/yyyy.";
                    return;
                }
            }

            var exito = await _controller.CrearMetaAhorroAsync(monto, txtDescripcion.Text, fechaObjetivo);
            txtMensaje.Text = exito ? "Meta de ahorro creada exitosamente." : "Error al crear la meta de ahorro.";

            if (exito)
            {
                txtDescripcion.Clear();
                txtMonto.Clear();
                txtFecha.Clear();
            }
        }

        private async void BtnDepositar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtAhorroId.Text, out int ahorroId))
            {
                txtMensaje.Text = "ID de meta inválido.";
                return;
            }

            if (!double.TryParse(txtMontoAccion.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                txtMensaje.Text = "Monto inválido.";
                return;
            }

            var disponible = await _controller.GetDisponibleAsync();
            if (monto > disponible)
            {
                txtMensaje.Text = $"Fondos insuficientes. Disponible: ${disponible:F2}";
                return;
            }

            var exito = await _controller.DepositarAsync(ahorroId, monto);
            txtMensaje.Text = exito ? $"${monto:F2} depositado exitosamente." : "Error al depositar.";
        }

        private async void BtnRetirar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtAhorroId.Text, out int ahorroId))
            {
                txtMensaje.Text = "ID de meta inválido.";
                return;
            }

            if (!double.TryParse(txtMontoAccion.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double monto) || monto <= 0)
            {
                txtMensaje.Text = "Monto inválido.";
                return;
            }

            var exito = await _controller.RetirarAsync(ahorroId, monto);
            txtMensaje.Text = exito ? $"${monto:F2} retirado exitosamente." : "Error al retirar o fondos insuficientes.";
        }

        private async void BtnVerMetas_Click(object sender, RoutedEventArgs e)
        {
            var ahorros = await _controller.ListarAhorrosAsync();

            if (!ahorros.Any())
            {
                txtMensaje.Text = "No hay metas de ahorro creadas.";
                dgDatos.ItemsSource = null;
                txtTablaTitulo.Text = "";
                return;
            }

            txtTablaTitulo.Text = $"Metas de ahorro ({ahorros.Count})";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = ahorros.Select(a => new
            {
                ID          = a.Id,
                Descripción = a.Descripcion,
                Objetivo    = $"${a.MontoObjetivo:F2}",
                Actual      = $"${a.MontoActual:F2}",
                Progreso    = $"{a.GetPorcentajeProgreso():F1}%",
                Estado      = a.ObjetivoAlcanzado ? "✅ Completado" : "🕐 En progreso",
                FechaLímite = a.FechaObjetivo.HasValue ? a.FechaObjetivo.Value.ToString("dd/MM/yyyy") : "-"
            }).ToList();
        }

        private async void BtnVerResumen_Click(object sender, RoutedEventArgs e)
        {
            var totalAhorrado    = await _controller.GetTotalAhorradoAsync();
            var ahorrosCompletos = await _controller.ListarAhorrosCompletosAsync();
            var todosAhorros     = await _controller.ListarAhorrosAsync();

            txtTablaTitulo.Text = "Resumen de ahorros";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = new[]
            {
                new { Indicador = "Total ahorrado",      Valor = $"${totalAhorrado:F2}"          },
                new { Indicador = "Metas totales",        Valor = $"{todosAhorros.Count}"         },
                new { Indicador = "Metas completadas",    Valor = $"{ahorrosCompletos.Count}"     },
                new { Indicador = "Metas en progreso",    Valor = $"{todosAhorros.Count - ahorrosCompletos.Count}" }
            };
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
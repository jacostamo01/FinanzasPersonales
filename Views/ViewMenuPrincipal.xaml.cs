using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class ViewMenuPrincipal : Window
    {
        private readonly MovimientoController _movimientoController;
        private readonly AhorroController _ahorroController;
        private readonly EstadisticaController _estadisticaController;
        private readonly InversionController _inversionController;
        private readonly Usuario _usuarioLogueado;

        public ViewMenuPrincipal(MovimientoController movimientoController,
                                  AhorroController ahorroController,
                                  EstadisticaController estadisticaController,
                                  InversionController inversionController,
                                  Usuario usuarioLogueado)
        {
            InitializeComponent();
            _movimientoController = movimientoController;
            _ahorroController = ahorroController;
            _estadisticaController = estadisticaController;
            _inversionController = inversionController;
            _usuarioLogueado = usuarioLogueado;
            txtBienvenida.Text = "Bienvenido/a: " + usuarioLogueado.Username;
        }

        private void BtnMovimientos_Click(object sender, RoutedEventArgs e)
        {
            var view = new ViewMovimientos(_movimientoController);
            view.Show();
        }

        private void BtnAhorros_Click(object sender, RoutedEventArgs e)
        {
            var view = new AhorroView(_ahorroController);
            view.Show();
        }

        private void BtnInversiones_Click(object sender, RoutedEventArgs e)
        {
            var view = new InversionView(_inversionController);
            view.Show();
        }

        private void BtnEstadisticas_Click(object sender, RoutedEventArgs e)
        {
            var view = new EstadisticaView(_estadisticaController);
            view.Show();
        }

        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
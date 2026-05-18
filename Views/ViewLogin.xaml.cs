using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Data;
using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class ViewLogin : Window
    {
        private readonly UsuarioController _usuarioController;
        private readonly FinanzasDbContext _context;

        public ViewLogin(UsuarioController usuarioController, FinanzasDbContext context)
        {
            InitializeComponent();
            _usuarioController = usuarioController;
            _context = context;
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMensaje.Text = "";
                string username = txtUsuario.Text.Trim();
                string password = txtPassword.Password;

                Usuario? usuario = await _usuarioController.IniciarSesionAsync(username, password);

                if (usuario != null)
                {
                    var movimientoService = new MovimientoService(_context);
                    var ahorroService = new AhorroService(_context, movimientoService);
                    var estadisticaService = new EstadisticaService(_context);
                    var inversionService = new InversionService(_context);
                    var colchonFinancieroService = new ColchonFinancieroService(_context, movimientoService);

                    var menuPrincipal = new ViewMenuPrincipal(
                        new MovimientoController(movimientoService, usuario.Id),
                        new AhorroController(ahorroService, usuario.Id),
                        new EstadisticaController(estadisticaService, usuario.Id),
                        new InversionController(inversionService, usuario.Id),
                        new ColchonFinancieroController(colchonFinancieroService, usuario.Id),
                        usuario);
                    menuPrincipal.Show();
                    Close();
                }
                else
                {
                    txtMensaje.Text = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar sesión: " + ex.Message);
            }
        }

        private async void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtMensaje.Text = "";
                string username = txtUsuario.Text.Trim();
                string password = txtPassword.Password;
                string confirmPassword = txtConfirmarPassword.Password;

                bool exito = await _usuarioController.RegistrarUsuarioAsync(
                    username, password, confirmPassword);

                if (exito)
                {
                    txtMensaje.Text = "Usuario registrado correctamente. Ahora puedes iniciar sesión.";
                    txtPassword.Clear();
                    txtConfirmarPassword.Clear();
                }
                else
                {
                    txtMensaje.Text =
                        "No se pudo registrar. Verifica que el usuario tenga entre 3 y 50 caracteres, " +
                        "que la contraseña tenga mínimo 4 caracteres, que coincidan las contraseñas " +
                        "y que el usuario no exista.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar usuario: " + ex.Message);
            }
        }
    }
}

using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class ViewLogin : Window
    {
        private readonly UsuarioController _usuarioController;

        public ViewLogin(UsuarioController usuarioController)
        {
            InitializeComponent();
            _usuarioController = usuarioController;
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
                    MessageBox.Show(
                        "Inicio de sesión exitoso. Bienvenido " + usuario.Username, "Login", MessageBoxButton.OK, MessageBoxImage.Information
                    );

                    txtMensaje.Text = "Usuario autenticado correctamente.";
                }
                else
                {
                    txtMensaje.Text = "Usuario o contraseña incorrectos.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al iniciar sesión:\n",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
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
                    username,
                    password,
                    confirmPassword
                );

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
            catch(Exception ex)
            {
                MessageBox.Show(
                    "Error al iniciar sesión:\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }
        }
    }

using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Data;
using FinanzasPersonales.NET.Services;
using FinanzasPersonales.NET.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace FinanzasPersonales.NET
{
    public partial class App : Application
    {
        private FinanzasDbContext? _context;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                string conexion =
                    "server=127.0.0.1;" +
                    "port=3306;" +
                    "database=finanzas_db;" +
                    "user=root;" +
                    "password=;" +
                    "SslMode=none;";

                var options = new DbContextOptionsBuilder<FinanzasDbContext>()
                    .UseMySql(conexion, ServerVersion.AutoDetect(conexion))
                    .Options;

                _context = new FinanzasDbContext(options);
                _context.Database.EnsureCreated();

                var usuarioService = new UsuarioService(_context);
                var usuarioController = new UsuarioController(usuarioService);

                var login = new ViewLogin(usuarioController);
                MainWindow = login;
                login.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al iniciar la aplicación:\n" + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _context?.Dispose();
            base.OnExit(e);
        }
    }
}

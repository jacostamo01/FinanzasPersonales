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

                // Crea las tablas si no existen. La base de datos finanzas_db debe existir en MariaDB/MySQL.
                _context.Database.EnsureCreated();
                CrearTablaColchonFinancieroSiNoExiste(_context);

                var usuarioService = new UsuarioService(_context);
                var usuarioController = new UsuarioController(usuarioService);

                var login = new ViewLogin(usuarioController, _context);
                MainWindow = login;
                login.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo conectar a la base de datos finanzas_db.\n\n" +
                    "Verifica que MySQL/MariaDB esté encendido en XAMPP y que la base de datos exista.\n\n" +
                    "Detalle: " + ex.Message,
                    "Error de conexión",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _context?.Dispose();
            base.OnExit(e);
        }

        private static void CrearTablaColchonFinancieroSiNoExiste(FinanzasDbContext context)
        {
            context.Database.ExecuteSqlRaw(@"
                CREATE TABLE IF NOT EXISTS colchones_financieros (
                    id INT NOT NULL AUTO_INCREMENT,
                    usuario_id INT NULL,
                    monto_actual DOUBLE NOT NULL DEFAULT 0,
                    meta DOUBLE NOT NULL,
                    porcentaje_ahorro DOUBLE NOT NULL,
                    fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    PRIMARY KEY (id)
                );");
        }
    }
}

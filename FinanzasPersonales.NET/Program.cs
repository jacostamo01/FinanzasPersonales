using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FinanzasPersonales.NET.Data;
using FinanzasPersonales.NET.Services;
using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Views;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("=== INICIANDO FINANZAS PERSONALES .NET ===");
            Console.WriteLine();

            // Configurar servicios
            var serviceProvider = ConfigurarServicios();

            try
            {
                // Crear la base de datos si no existe
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<FinanzasDbContext>();

                    Console.WriteLine("Intentando conectar a la base de datos...");

                    // Probar la conexión
                    var canConnect = await context.Database.CanConnectAsync();

                    if (!canConnect)
                    {
                        Console.WriteLine("❌ No se pudo conectar a la base de datos.");
                        Console.WriteLine("Verifica que:");
                        Console.WriteLine("- MySQL/MariaDB esté corriendo en XAMPP");
                        Console.WriteLine("- La base de datos 'finanzas_db' exista");
                        Console.WriteLine("- El usuario 'root' tenga acceso sin contraseña");
                        Console.WriteLine("\nPresione cualquier tecla para salir...");
                        Console.ReadKey();
                        return;
                    }

                    await context.Database.EnsureCreatedAsync();
                    Console.WriteLine("✓ Base de datos inicializada");
                }

                // Iniciar la aplicación
                await EjecutarAplicacionAsync(serviceProvider);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fatal: {ex.Message}");
                Console.WriteLine($"\nDetalles técnicos: {ex.InnerException?.Message}");
                Console.WriteLine("\nPresione cualquier tecla para salir...");
                Console.ReadKey();
            }
            finally
            {
                serviceProvider.Dispose();
            }
        }

        private static ServiceProvider ConfigurarServicios()
        {
            // Configuración
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Contenedor de servicios
            var services = new ServiceCollection();

            // Configurar Entity Framework para MySQL
            services.AddDbContext<FinanzasDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))
                ));

            // Registrar servicios
            services.AddScoped<UsuarioService>();
            services.AddScoped<MovimientoService>();
            services.AddScoped<AhorroService>();
            services.AddScoped<EstadisticaService>();
            services.AddScoped<InversionService>();

            // Registrar controladores
            services.AddScoped<UsuarioController>();
            services.AddScoped<MovimientoController>();
            services.AddScoped<AhorroController>();
            services.AddScoped<EstadisticaController>();
            services.AddScoped<InversionController>();

            // Registrar vistas
            services.AddScoped<ViewLogin>();
            services.AddScoped<ViewMenuPrincipal>();
            services.AddScoped<ViewMovimientos>();
            services.AddScoped<AhorroView>();
            services.AddScoped<EstadisticaView>();
            services.AddScoped<InversionView>();

            // Configuración
            services.AddSingleton<IConfiguration>(configuration);

            return services.BuildServiceProvider();
        }

        private static async Task EjecutarAplicacionAsync(ServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            // Obtener el controlador de login
            var usuarioController = scope.ServiceProvider.GetRequiredService<UsuarioController>();
            var viewLogin = new ViewLogin(usuarioController);

            // Proceso de login
            var usuarioLogueado = await viewLogin.MostrarLoginAsync();

            if (usuarioLogueado == null)
            {
                Console.WriteLine("Aplicación cerrada por el usuario.");
                return;
            }

            // Una vez logueado, mostrar el menú principal
            var movimientoController = scope.ServiceProvider.GetRequiredService<MovimientoController>();
            var ahorroController = scope.ServiceProvider.GetRequiredService<AhorroController>();
            var estadisticaController = scope.ServiceProvider.GetRequiredService<EstadisticaController>();
            var inversionController = scope.ServiceProvider.GetRequiredService<InversionController>();

            var menuPrincipal = new ViewMenuPrincipal(
                movimientoController,
                ahorroController,
                estadisticaController,
                inversionController,
                usuarioLogueado);

            await menuPrincipal.MostrarMenuAsync();
        }
    }
}

using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Views
{
    public class ViewMenuPrincipal
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
            _movimientoController = movimientoController;
            _ahorroController = ahorroController;
            _estadisticaController = estadisticaController;
            _inversionController = inversionController;
            _usuarioLogueado = usuarioLogueado;
        }

        public async Task MostrarMenuAsync()
        {
            bool continuar = true;
            while (continuar)
            {
                Console.Clear();
                EstiloApp.MostrarTitulo("=== FINANZAS PERSONALES ===");
                Console.WriteLine($"Bienvenido/a: {_usuarioLogueado.Username}");
                Console.WriteLine();
                Console.WriteLine("1. Movimientos (Ingresos/Gastos)");
                Console.WriteLine("2. Ahorros");
                Console.WriteLine("3. Inversiones");
                Console.WriteLine("4. Estadísticas");
                Console.WriteLine("5. Salir");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        var viewMovimientos = new ViewMovimientos(_movimientoController);
                        await viewMovimientos.MostrarMenuAsync();
                        break;
                    case "2":
                        var ahorroView = new AhorroView(_ahorroController);
                        await ahorroView.MostrarMenuAsync();
                        break;
                    case "3":
                        var inversionView = new InversionView(_inversionController);
                        await inversionView.MostrarMenuAsync();
                        break;
                    case "4":
                        var estadisticaView = new EstadisticaView(_estadisticaController);
                        await estadisticaView.MostrarEstadisticasAsync();
                        break;
                    case "5":
                        continuar = false;
                        Console.WriteLine("¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}

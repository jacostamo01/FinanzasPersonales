using FinanzasPersonales.NET.Controllers;
using FinanzasPersonales.NET.Models;
using System;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Views
{
    /// <summary>
    /// Vista de inicio de sesión y registro.
    /// </summary>
    public class ViewLogin
    {
        private readonly UsuarioController _usuarioController;

        public ViewLogin(UsuarioController usuarioController)
        {
            _usuarioController = usuarioController;
        }

        public async Task<Usuario?> MostrarLoginAsync()
        {
            while (true)
            {
                Console.Clear();
                EstiloApp.MostrarTitulo("=== FINANZAS PERSONALES - LOGIN ===");
                Console.WriteLine("1. Iniciar sesión");
                Console.WriteLine("2. Registrarse");
                Console.WriteLine("3. Salir");
                Console.WriteLine();
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        var usuario = await IniciarSesionAsync();
                        if (usuario != null) return usuario;
                        break;
                    case "2":
                        await RegistrarUsuarioAsync();
                        break;
                    case "3":
                        return null;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task<Usuario?> IniciarSesionAsync()
        {
            Console.Clear();
            EstiloApp.MostrarTitulo("=== INICIAR SESIÓN ===");

            Console.Write("Usuario: ");
            var username = Console.ReadLine();

            Console.Write("Contraseña: ");
            var password = LeerPasswordOculto();

            var usuario = await _usuarioController.IniciarSesionAsync(username ?? "", password);

            if (usuario != null)
            {
                Console.WriteLine("\n¡Inicio de sesión exitoso!");
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return usuario;
            }
            else
            {
                Console.WriteLine("\nCredenciales incorrectas. Presione cualquier tecla para continuar...");
                Console.ReadKey();
                return null;
            }
        }

        private async Task RegistrarUsuarioAsync()
        {
            Console.Clear();
            EstiloApp.MostrarTitulo("=== REGISTRARSE ===");

            Console.Write("Usuario (3-50 caracteres): ");
            var username = Console.ReadLine();

            Console.Write("Contraseña (mínimo 4 caracteres): ");
            var password = LeerPasswordOculto();

            Console.Write("\nConfirmar contraseña: ");
            var confirmPassword = LeerPasswordOculto();

            var exito = await _usuarioController.RegistrarUsuarioAsync(username ?? "", password, confirmPassword);

            if (exito)
            {
                Console.WriteLine("\n¡Usuario registrado exitosamente!");
            }
            else
            {
                Console.WriteLine("\nError al registrar usuario. Verifique que:");
                Console.WriteLine("- El usuario tenga entre 3 y 50 caracteres");
                Console.WriteLine("- La contraseña tenga al menos 4 caracteres");
                Console.WriteLine("- Las contraseñas coincidan");
                Console.WriteLine("- El usuario no exista ya");
            }

            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        private string LeerPasswordOculto()
        {
            var password = string.Empty;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            return password;
        }
    }
}

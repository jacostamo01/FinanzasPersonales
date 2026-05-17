using System;

namespace FinanzasPersonales.NET.Views
{
    /// <summary>
    /// Clase para manejar el estilo y presentación de la aplicación de consola.
    /// </summary>
    public static class EstiloApp
    {
        public static void MostrarTitulo(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(titulo);
            Console.WriteLine(new string('=', titulo.Length));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void MostrarSubtitulo(string subtitulo)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(subtitulo);
            Console.WriteLine(new string('-', subtitulo.Length));
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✗ {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarAdvertencia(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ {mensaje}");
            Console.ResetColor();
        }

        public static void MostrarInfo(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"ℹ {mensaje}");
            Console.ResetColor();
        }

        public static void EsperarTecla(string mensaje = "Presione cualquier tecla para continuar...")
        {
            Console.WriteLine();
            Console.WriteLine(mensaje);
            Console.ReadKey();
        }
    }
}

using MySql.Data.MySqlClient;
using System;

namespace FinanzasPersonales.NET.Data
{
    public class DataBase
    {
       
        private static string cadenaConexion =
            "Server=127.0.0.1;" +
            "Port=3306;" +
            "Database=finanzas_db;" +
            "Uid=root;" +
            "Pwd=root;" +
            "SslMode=none;" +
            "AllowUserVariables=True;";

        public static MySqlConnection Conectar()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);

            try
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                {
                    conexion.Open();
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception($"Error {ex.Number}: No se pudo conectar a finanzas_db. " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general de conexión: " + ex.Message);
            }

            return conexion;
        }

        public static void Desconectar(MySqlConnection conexion)
        {
            try
            {
                if (conexion != null && conexion.State != System.Data.ConnectionState.Closed)
                {
                    conexion.Close();
                    conexion.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cerrar la conexión: " + ex.Message);
            }
        }
    }
}

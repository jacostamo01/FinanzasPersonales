using MySql.Data.MySqlClient;
using System;

namespace FinanzasPersonales.NET.Data
{
    public class DataBase
    {
        private static string cadenaConexion =
            "server=127.0.0.1;" +
            "port=3306;" +
            "database=finanzas_db;" +
            "user=root;" +
            "password=;" +
            "SslMode=none;";

        public static MySqlConnection Conectar()
        {
            MySqlConnection conexion = new MySqlConnection(cadenaConexion);

            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar con finanzas_db: " + ex.Message, ex);
            }

            return conexion;
        }

        public static void Desconectar(MySqlConnection conexion)
        {
            if (conexion != null)
            {
                conexion.Close();
            }
        }
    }
}

using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class UsuarioService
    {
        public UsuarioService()
        {
        }

        public async Task<bool> RegistrarUsuarioAsync(string username, string password)
        {
            MySqlConnection? conexion = null;
            try
            {
                if (await ExisteUsuarioAsync(username))
                    return false;

                conexion = DataBase.Conectar();
                string query = "INSERT INTO usuarios (username, password) VALUES (@username, @password)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (conexion != null)
                    DataBase.Desconectar(conexion);
            }
        }

        public async Task<Usuario?> AutenticarUsuarioAsync(string username, string password)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, username, password FROM usuarios WHERE username = @username";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            int idVal = reader.GetInt32(reader.GetOrdinal("id"));
                            string userVal = reader.GetString(reader.GetOrdinal("username"));
                            string passVal = reader.GetString(reader.GetOrdinal("password"));

                            var usuario = new Usuario(userVal, passVal)
                            {
                                Id = idVal
                            };

                            if (usuario.ValidarPassword(password))
                            {
                                return usuario;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (conexion != null)
                    DataBase.Desconectar(conexion);
            }
        }

        public async Task<bool> ExisteUsuarioAsync(string username)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT COUNT(1) FROM usuarios WHERE username = @username";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    var countObj = await cmd.ExecuteScalarAsync();
                    if (countObj != null)
                    {
                        int count = Convert.ToInt32(countObj);
                        return count > 0;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (conexion != null)
                    DataBase.Desconectar(conexion);
            }
        }
    }
}
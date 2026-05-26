using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class AhorroService
    {
        private readonly MovimientoService _movimientoService;

        public AhorroService(MovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        public async Task<bool> CrearMetaAhorroAsync(double montoObjetivo, string descripcion, int usuarioId, DateTime? fechaObjetivo = null)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "INSERT INTO ahorros (monto_objetivo, monto_actual, descripcion, usuario_id, fecha_objetivo) " +
                               "VALUES (@montoObjetivo, 0, @descripcion, @usuarioId, @fechaObjetivo)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@montoObjetivo", montoObjetivo);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@fechaObjetivo", (object?)fechaObjetivo ?? DBNull.Value);
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

        public async Task<List<Ahorro>> ListarAhorrosAsync(int usuarioId)
        {
            var lista = new List<Ahorro>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_objetivo, monto_actual, descripcion, fecha_creacion, fecha_objetivo FROM ahorros WHERE usuario_id = @usuarioId ORDER BY fecha_creacion DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var ahorro = new Ahorro(
                                reader.GetDouble(reader.GetOrdinal("monto_objetivo")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("fecha_objetivo")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_objetivo"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(ahorro);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conexion != null)
                    DataBase.Desconectar(conexion);
            }
            return lista;
        }

        public async Task<double> CalcularDisponibleAsync(int usuarioId)
        {
            return await _movimientoService.CalcularBalanceAsync(usuarioId);
        }

        public async Task<Ahorro?> ObtenerAhorroPorIdAsync(int ahorroId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_objetivo, monto_actual, descripcion, fecha_creacion, fecha_objetivo FROM ahorros WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", ahorroId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var ahorro = new Ahorro(
                                reader.GetDouble(reader.GetOrdinal("monto_objetivo")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("fecha_objetivo")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_objetivo"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            return ahorro;
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

        public async Task<bool> DepositarAhorroAsync(int ahorroId, double monto, int usuarioId)
        {
            if (monto <= 0) return false;

            try
            {
                var ahorro = await ObtenerAhorroPorIdAsync(ahorroId);
                if (ahorro == null) return false;

                var disponible = await CalcularDisponibleAsync(usuarioId);
                if (monto > disponible) return false;

                ahorro.Depositar(monto);

                MySqlConnection? conexion = null;
                try
                {
                    conexion = DataBase.Conectar();
                    string query = "UPDATE ahorros SET monto_actual = @montoActual WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@montoActual", ahorro.MontoActual);
                        cmd.Parameters.AddWithValue("@id", ahorroId);
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
            catch
            {
                return false;
            }
        }

        public async Task<bool> RetirarAhorroAsync(int ahorroId, double monto)
        {
            if (monto <= 0) return false;

            try
            {
                var ahorro = await ObtenerAhorroPorIdAsync(ahorroId);
                if (ahorro == null) return false;

                if (ahorro.Retirar(monto))
                {
                    MySqlConnection? conexion = null;
                    try
                    {
                        conexion = DataBase.Conectar();
                        string query = "UPDATE ahorros SET monto_actual = @montoActual WHERE id = @id";
                        using (var cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@montoActual", ahorro.MontoActual);
                            cmd.Parameters.AddWithValue("@id", ahorroId);
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
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<double> CalcularTotalAhorradoAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT SUM(monto_actual) FROM ahorros WHERE usuario_id = @usuarioId";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    var valObj = await cmd.ExecuteScalarAsync();
                    if (valObj != null && valObj != DBNull.Value)
                    {
                        return Convert.ToDouble(valObj);
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                if (conexion != null)
                    DataBase.Desconectar(conexion);
            }
        }

        public async Task<List<Ahorro>> ListarAhorrosCompletosAsync(int usuarioId)
        {
            var lista = new List<Ahorro>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_objetivo, monto_actual, descripcion, fecha_creacion, fecha_objetivo FROM ahorros WHERE usuario_id = @usuarioId AND monto_actual >= monto_objetivo";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var ahorro = new Ahorro(
                                reader.GetDouble(reader.GetOrdinal("monto_objetivo")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("fecha_objetivo")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_objetivo"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(ahorro);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (conexion != null)
                    DataBase.Desconectar(conexion);
            }
            return lista;
        }
    }
}

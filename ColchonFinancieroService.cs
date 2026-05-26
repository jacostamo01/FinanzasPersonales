using FinanzasPersonales.NET.Data;
using FinanzasPersonales.NET.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class ColchonFinancieroService
    {
        private readonly MovimientoService _movimientoService;

        public ColchonFinancieroService(MovimientoService movimientoService)
        {
            _movimientoService = movimientoService;
        }

        public async Task<bool> CrearColchonAsync(int usuarioId, double meta)
        {
            if (meta <= 0) return false;

            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "INSERT INTO colchon_financiero (usuario_id, meta, monto_actual, porcentaje_ahorro) VALUES (@usuarioId, @meta, 0, 0)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@meta", meta);
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

        public async Task<ColchonFinanciero?> ObtenerColchonAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_actual, meta, porcentaje_ahorro, fecha_creacion FROM colchon_financiero WHERE usuario_id = @usuarioId LIMIT 1";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var colchon = new ColchonFinanciero(
                                reader.GetDouble(reader.GetOrdinal("meta"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                PorcentajeAhorro = reader.GetDouble(reader.GetOrdinal("porcentaje_ahorro")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            return colchon;
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

        public async Task<double> CalcularDisponibleAsync(int usuarioId)
        {
            return await _movimientoService.CalcularBalanceAsync(usuarioId);
        }

        public async Task<ColchonFinanciero?> ObtenerColchonPorIdAsync(int colchonId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_actual, meta, porcentaje_ahorro, fecha_creacion FROM colchon_financiero WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@id", colchonId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var colchon = new ColchonFinanciero(
                                reader.GetDouble(reader.GetOrdinal("meta"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                PorcentajeAhorro = reader.GetDouble(reader.GetOrdinal("porcentaje_ahorro")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            return colchon;
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

        public async Task<bool> DepositarColchonAsync(int colchonId, double monto, int usuarioId)
        {
            if (monto <= 0) return false;

            try
            {
                var colchon = await ObtenerColchonPorIdAsync(colchonId);
                if (colchon == null) return false;

                var disponible = await CalcularDisponibleAsync(usuarioId);
                if (monto > disponible) return false;

                colchon.Depositar(monto);

                MySqlConnection? conexion = null;
                try
                {
                    conexion = DataBase.Conectar();
                    string query = "UPDATE colchon_financiero SET monto_actual = @montoActual, porcentaje_ahorro = @porcentajeAhorro WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@montoActual", colchon.MontoActual);
                        cmd.Parameters.AddWithValue("@porcentajeAhorro", colchon.PorcentajeAhorro);
                        cmd.Parameters.AddWithValue("@id", colchonId);
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

        public async Task<bool> RetirarColchonAsync(int colchonId, double monto)
        {
            if (monto <= 0) return false;

            try
            {
                var colchon = await ObtenerColchonPorIdAsync(colchonId);
                if (colchon == null) return false;

                if (colchon.Retirar(monto))
                {
                    MySqlConnection? conexion = null;
                    try
                    {
                        conexion = DataBase.Conectar();
                        string query = "UPDATE colchon_financiero SET monto_actual = @montoActual, porcentaje_ahorro = @porcentajeAhorro WHERE id = @id";
                        using (var cmd = new MySqlCommand(query, conexion))
                        {
                            cmd.Parameters.AddWithValue("@montoActual", colchon.MontoActual);
                            cmd.Parameters.AddWithValue("@porcentajeAhorro", colchon.PorcentajeAhorro);
                            cmd.Parameters.AddWithValue("@id", colchonId);
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

        public async Task<double> CalcularTotalColchonAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT SUM(monto_actual) FROM colchon_financiero WHERE usuario_id = @usuarioId";
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

        public async Task<List<ColchonFinanciero>> ListarColchonesAsync(int usuarioId)
        {
            var lista = new List<ColchonFinanciero>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_actual, meta, porcentaje_ahorro, fecha_creacion FROM colchon_financiero WHERE usuario_id = @usuarioId ORDER BY fecha_creacion DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var colchon = new ColchonFinanciero(
                                reader.GetDouble(reader.GetOrdinal("meta"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                PorcentajeAhorro = reader.GetDouble(reader.GetOrdinal("porcentaje_ahorro")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(colchon);
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

        public async Task<List<ColchonFinanciero>> ListarColchonesCompletosAsync(int usuarioId)
        {
            var lista = new List<ColchonFinanciero>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_actual, meta, porcentaje_ahorro, fecha_creacion FROM colchon_financiero WHERE usuario_id = @usuarioId AND monto_actual >= meta";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var colchon = new ColchonFinanciero(
                                reader.GetDouble(reader.GetOrdinal("meta"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                MontoActual = reader.GetDouble(reader.GetOrdinal("monto_actual")),
                                PorcentajeAhorro = reader.GetDouble(reader.GetOrdinal("porcentaje_ahorro")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha_creacion")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(colchon);
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

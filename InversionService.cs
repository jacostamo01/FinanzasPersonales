using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class InversionService
    {
        public InversionService()
        {
        }

        public async Task<bool> CrearInversionAsync(double montoInicial, double tasaInteres, string descripcion, int usuarioId, DateTime? fechaVencimiento = null)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "INSERT INTO inversiones (monto_inicial, valor_actual, tasa_interes, descripcion, usuario_id, fecha_vencimiento) " +
                               "VALUES (@montoInicial, @valorActual, @tasaInteres, @descripcion, @usuarioId, @fechaVencimiento)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@montoInicial", montoInicial);
                    cmd.Parameters.AddWithValue("@valorActual", montoInicial);
                    cmd.Parameters.AddWithValue("@tasaInteres", tasaInteres);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@fechaVencimiento", (object?)fechaVencimiento ?? DBNull.Value);
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

        public async Task<List<Inversion>> ListarInversionesAsync(int usuarioId)
        {
            var lista = new List<Inversion>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_inicial, valor_actual, tasa_interes, descripcion, fecha_inicio, fecha_vencimiento FROM inversiones WHERE usuario_id = @usuarioId ORDER BY fecha_inicio DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inversion = new Inversion(
                                reader.GetDouble(reader.GetOrdinal("monto_inicial")),
                                reader.GetDouble(reader.GetOrdinal("tasa_interes")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("fecha_vencimiento")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_vencimiento"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                ValorActual = reader.GetDouble(reader.GetOrdinal("valor_actual")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(inversion);
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

        public async Task<bool> ActualizarValorInversionAsync(int inversionId, double montoInicial)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "UPDATE inversiones SET monto_inicial = @montoInicial, valor_actual = @valorActual WHERE id = @id";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@montoInicial", montoInicial);
                    cmd.Parameters.AddWithValue("@valorActual", montoInicial);
                    cmd.Parameters.AddWithValue("@id", inversionId);
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

        public async Task<double> CalcularTotalInvertidoAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT SUM(monto_inicial) FROM inversiones WHERE usuario_id = @usuarioId";
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

        public async Task<double> CalcularValorTotalActualAsync(int usuarioId)
        {
            var inversiones = await ListarInversionesAsync(usuarioId);
            double total = 0;

            foreach (var inversion in inversiones)
            {
                inversion.ActualizarValor();
                total += inversion.ValorActual;

                MySqlConnection? conexion = null;
                try
                {
                    conexion = DataBase.Conectar();
                    string query = "UPDATE inversiones SET valor_actual = @valorActual WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@valorActual", inversion.ValorActual);
                        cmd.Parameters.AddWithValue("@id", inversion.Id);
                        await cmd.ExecuteNonQueryAsync();
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
            }

            return total;
        }

        public async Task<double> CalcularGananciaTotalAsync(int usuarioId)
        {
            var totalInvertido = await CalcularTotalInvertidoAsync(usuarioId);
            var valorActual = await CalcularValorTotalActualAsync(usuarioId);
            return valorActual - totalInvertido;
        }

        public async Task<List<Inversion>> ListarInversionesVencidasAsync(int usuarioId)
        {
            var lista = new List<Inversion>();
            var today = DateTime.Today;
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, usuario_id, monto_inicial, valor_actual, tasa_interes, descripcion, fecha_inicio, fecha_vencimiento " +
                               "FROM inversiones WHERE usuario_id = @usuarioId AND fecha_vencimiento IS NOT NULL AND fecha_vencimiento <= @today ORDER BY fecha_inicio DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@today", today);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var inversion = new Inversion(
                                reader.GetDouble(reader.GetOrdinal("monto_inicial")),
                                reader.GetDouble(reader.GetOrdinal("tasa_interes")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("fecha_vencimiento")) ? null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("fecha_vencimiento"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                ValorActual = reader.GetDouble(reader.GetOrdinal("valor_actual")),
                                FechaInicio = reader.GetDateTime(reader.GetOrdinal("fecha_inicio")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(inversion);
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

        public double CalcularGanancia(Inversion inversion)
        {
            return inversion.CalcularGananciaCompuesta();
        }

        public double CalcularMontoTotal(Inversion inversion)
        {
            return inversion.CalcularValorActual();
        }
    }
}

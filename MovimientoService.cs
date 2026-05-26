using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class MovimientoService
    {
        public MovimientoService()
        {
        }

        public async Task<bool> AgregarIngresoAsync(double monto, string descripcion, int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "INSERT INTO ingresos (monto, descripcion, usuario_id) VALUES (@monto, @descripcion, @usuarioId)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@monto", monto);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
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

        public async Task<bool> AgregarGastoAsync(double monto, string descripcion, int usuarioId, string categoria = "")
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "INSERT INTO gastos (monto, descripcion, categoria, usuario_id) VALUES (@monto, @descripcion, @categoria, @usuarioId)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@monto", monto);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@categoria", categoria);
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
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

        public async Task<List<Movimiento>> ListarMovimientosAsync(int usuarioId)
        {
            var movimientos = new List<Movimiento>();
            var ingresos = await ListarIngresosAsync(usuarioId);
            var gastos = await ListarGastosAsync(usuarioId);

            movimientos.AddRange(ingresos);
            movimientos.AddRange(gastos);

            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }

        public async Task<List<Ingreso>> ListarIngresosAsync(int usuarioId)
        {
            var lista = new List<Ingreso>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, monto, descripcion, fecha, usuario_id FROM ingresos WHERE usuario_id = @usuarioId ORDER BY fecha DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var ingreso = new Ingreso(
                                reader.GetDouble(reader.GetOrdinal("monto")),
                                reader.GetString(reader.GetOrdinal("descripcion"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(ingreso);
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

        public async Task<List<Gasto>> ListarGastosAsync(int usuarioId)
        {
            var lista = new List<Gasto>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, monto, descripcion, categoria, fecha, usuario_id FROM gastos WHERE usuario_id = @usuarioId ORDER BY fecha DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var gasto = new Gasto(
                                reader.GetDouble(reader.GetOrdinal("monto")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("categoria")) ? "" : reader.GetString(reader.GetOrdinal("categoria"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(gasto);
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

        public async Task<double> CalcularTotalIngresosAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT SUM(monto) FROM ingresos WHERE usuario_id = @usuarioId";
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

        public async Task<double> CalcularTotalGastosAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT SUM(monto) FROM gastos WHERE usuario_id = @usuarioId";
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

        public async Task<double> CalcularBalanceAsync(int usuarioId)
        {
            var totalIngresos = await CalcularTotalIngresosAsync(usuarioId);
            var totalGastos = await CalcularTotalGastosAsync(usuarioId);
            return totalIngresos - totalGastos;
        }

        public async Task<List<Movimiento>> ListarMovimientosPorMesAsync(int mes, int anio, int usuarioId)
        {
            var movimientos = new List<Movimiento>();

            var ingresos = await ListarIngresosPorMesAsync(mes, anio, usuarioId);
            var gastos = await ListarGastosPorMesAsync(mes, anio, usuarioId);

            movimientos.AddRange(ingresos);
            movimientos.AddRange(gastos);

            return movimientos.OrderByDescending(m => m.Fecha).ToList();
        }

        private async Task<List<Ingreso>> ListarIngresosPorMesAsync(int mes, int anio, int usuarioId)
        {
            var lista = new List<Ingreso>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, monto, descripcion, fecha, usuario_id FROM ingresos WHERE usuario_id = @usuarioId AND MONTH(fecha) = @mes AND YEAR(fecha) = @anio ORDER BY fecha DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var ingreso = new Ingreso(
                                reader.GetDouble(reader.GetOrdinal("monto")),
                                reader.GetString(reader.GetOrdinal("descripcion"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(ingreso);
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

        private async Task<List<Gasto>> ListarGastosPorMesAsync(int mes, int anio, int usuarioId)
        {
            var lista = new List<Gasto>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT id, monto, descripcion, categoria, fecha, usuario_id FROM gastos WHERE usuario_id = @usuarioId AND MONTH(fecha) = @mes AND YEAR(fecha) = @anio ORDER BY fecha DESC";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var gasto = new Gasto(
                                reader.GetDouble(reader.GetOrdinal("monto")),
                                reader.GetString(reader.GetOrdinal("descripcion")),
                                reader.IsDBNull(reader.GetOrdinal("categoria")) ? "" : reader.GetString(reader.GetOrdinal("categoria"))
                            )
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                UsuarioId = reader.GetInt32(reader.GetOrdinal("usuario_id"))
                            };
                            lista.Add(gasto);
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

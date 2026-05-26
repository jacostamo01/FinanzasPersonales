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
    public class EstadisticaService
    {
        public EstadisticaService()
        {
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
            var ingresos = await CalcularTotalIngresosAsync(usuarioId);
            var gastos = await CalcularTotalGastosAsync(usuarioId);
            return ingresos - gastos;
        }

        public async Task<double> CalcularPromedioIngresosAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT AVG(monto) FROM ingresos WHERE usuario_id = @usuarioId";
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

        public async Task<double> CalcularPromedioGastosAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT AVG(monto) FROM gastos WHERE usuario_id = @usuarioId";
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

        public async Task<int> ContarMovimientosAsync(int usuarioId)
        {
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT " +
                               "(SELECT COUNT(1) FROM ingresos WHERE usuario_id = @usuarioId) + " +
                               "(SELECT COUNT(1) FROM gastos WHERE usuario_id = @usuarioId)";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    var valObj = await cmd.ExecuteScalarAsync();
                    if (valObj != null && valObj != DBNull.Value)
                    {
                        return Convert.ToInt32(valObj);
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

        public double CalcularPorcentajeGastos(double gastos, double ingresos)
        {
            if (ingresos == 0) return 0;
            return (gastos / ingresos) * 100;
        }

        public async Task<List<(string Categoria, double Total)>> ObtenerGastosPorCategoriaAsync(int usuarioId)
        {
            var lista = new List<(string Categoria, double Total)>();
            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();
                string query = "SELECT categoria, SUM(monto) AS Total FROM gastos WHERE usuario_id = @usuarioId GROUP BY categoria";
                using (var cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string categoria = reader.IsDBNull(reader.GetOrdinal("categoria")) || string.IsNullOrWhiteSpace(reader.GetString(reader.GetOrdinal("categoria")))
                                ? "Sin categoría" 
                                : reader.GetString(reader.GetOrdinal("categoria"));
                            double total = reader.GetDouble(reader.GetOrdinal("Total"));
                            lista.Add((categoria, total));
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

        public async Task<List<(int Mes, int Anio, double TotalIngresos, double TotalGastos)>> ObtenerResumenMensualAsync(int usuarioId)
        {
            var ingresosPorMes = new List<(int Month, int Year, double Total)>();
            var gastosPorMes = new List<(int Month, int Year, double Total)>();

            MySqlConnection? conexion = null;
            try
            {
                conexion = DataBase.Conectar();

                // Ingresos
                string queryIngresos = "SELECT MONTH(fecha) AS Mes, YEAR(fecha) AS Anio, SUM(monto) AS Total " +
                                       "FROM ingresos WHERE usuario_id = @usuarioId GROUP BY YEAR(fecha), MONTH(fecha)";
                using (var cmd = new MySqlCommand(queryIngresos, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ingresosPorMes.Add((
                                reader.GetInt32(reader.GetOrdinal("Mes")),
                                reader.GetInt32(reader.GetOrdinal("Anio")),
                                reader.GetDouble(reader.GetOrdinal("Total"))
                            ));
                        }
                    }
                }

                // Gastos
                string queryGastos = "SELECT MONTH(fecha) AS Mes, YEAR(fecha) AS Anio, SUM(monto) AS Total " +
                                     "FROM gastos WHERE usuario_id = @usuarioId GROUP BY YEAR(fecha), MONTH(fecha)";
                using (var cmd = new MySqlCommand(queryGastos, conexion))
                {
                    cmd.Parameters.AddWithValue("@usuarioId", usuarioId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            gastosPorMes.Add((
                                reader.GetInt32(reader.GetOrdinal("Mes")),
                                reader.GetInt32(reader.GetOrdinal("Anio")),
                                reader.GetDouble(reader.GetOrdinal("Total"))
                            ));
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

            var resumen = new List<(int, int, double, double)>();

            var periodos = ingresosPorMes.Select(i => new { Month = i.Month, Year = i.Year })
                .Union(gastosPorMes.Select(g => new { Month = g.Month, Year = g.Year }))
                .OrderBy(p => p.Year).ThenBy(p => p.Month);

            foreach (var periodo in periodos)
            {
                var totalIngresos = ingresosPorMes
                    .FirstOrDefault(i => i.Month == periodo.Month && i.Year == periodo.Year).Total;
                var totalGastos = gastosPorMes
                    .FirstOrDefault(g => g.Month == periodo.Month && g.Year == periodo.Year).Total;

                resumen.Add((periodo.Month, periodo.Year, totalIngresos, totalGastos));
            }

            return resumen;
        }
    }

    public class Calculadora
    {
        public double Sumar(double a, double b) => a + b;
        public double Restar(double a, double b) => a - b;
        public double Multiplicar(double a, double b) => a * b;
        public double Dividir(double a, double b) => b != 0 ? a / b : 0;
        public double Porcentaje(double cantidad, double total) => total != 0 ? (cantidad / total) * 100 : 0;
        public double Promedio(double total, int cantidad) => cantidad > 0 ? total / cantidad : 0;
    }
}

using FinanzasPersonales.NET.Controllers;
using System;
using System.Linq;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class EstadisticaView : Window
    {
        private readonly EstadisticaController _controller;

        public EstadisticaView(EstadisticaController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private async void BtnResumenGeneral_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos    = await _controller.GetTotalIngresosAsync();
            var totalGastos      = await _controller.GetTotalGastosAsync();
            var balance          = await _controller.GetBalanceAsync();
            var promedioIngresos = await _controller.GetPromedioIngresosAsync();
            var promedioGastos   = await _controller.GetPromedioGastosAsync();
            var totalMovimientos = await _controller.ContarMovimientosAsync();

            var porcentajeGastos = totalIngresos > 0
                ? $"{_controller.CalcularPorcentajeGastos(totalGastos, totalIngresos):F1}%"
                : "N/A";

            txtTablaTitulo.Text = "Resumen general";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = new[]
            {
                new { Indicador = "Total ingresos",       Valor = $"${totalIngresos:F2}"    },
                new { Indicador = "Total gastos",         Valor = $"${totalGastos:F2}"      },
                new { Indicador = "Balance actual",       Valor = $"${balance:F2}"          },
                new { Indicador = "Promedio ingresos",    Valor = $"${promedioIngresos:F2}" },
                new { Indicador = "Promedio gastos",      Valor = $"${promedioGastos:F2}"   },
                new { Indicador = "Total movimientos",    Valor = $"{totalMovimientos}"      },
                new { Indicador = "% gastos / ingresos",  Valor = porcentajeGastos           }
            };
        }

        private async void BtnGastosPorCategoria_Click(object sender, RoutedEventArgs e)
        {
            var totalGastos         = await _controller.GetTotalGastosAsync();
            var gastosPorCategoria  = await _controller.GetGastosPorCategoriaAsync();

            if (!gastosPorCategoria.Any())
            {
                txtMensaje.Text = "No hay gastos registrados por categoría.";
                dgDatos.ItemsSource = null;
                txtTablaTitulo.Text = "";
                return;
            }

            txtTablaTitulo.Text = "Gastos por categoría";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = gastosPorCategoria
                .OrderByDescending(x => x.Total)
                .Select(x => new
                {
                    Categoría   = x.Categoria,
                    Total       = $"${x.Total:F2}",
                    Porcentaje  = totalGastos > 0 ? $"{(x.Total / totalGastos) * 100:F1}%" : "0%"
                }).ToList();
        }

        private async void BtnResumenMensual_Click(object sender, RoutedEventArgs e)
        {
            var resumenMensual = await _controller.GetResumenMensualAsync();

            if (!resumenMensual.Any())
            {
                txtMensaje.Text = "No hay datos mensuales disponibles.";
                dgDatos.ItemsSource = null;
                txtTablaTitulo.Text = "";
                return;
            }

            txtTablaTitulo.Text = "Resumen mensual (últimos 6 meses)";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = resumenMensual
                .TakeLast(6)
                .Select(r => new
                {
                    Mes      = new DateTime(r.Anio, r.Mes, 1).ToString("MMM/yyyy"),
                    Ingresos = $"${r.TotalIngresos:F2}",
                    Gastos   = $"${r.TotalGastos:F2}",
                    Balance  = $"${r.TotalIngresos - r.TotalGastos:F2}"
                }).ToList();
        }

        private async void BtnVerTodo_Click(object sender, RoutedEventArgs e)
        {
            var totalIngresos    = await _controller.GetTotalIngresosAsync();
            var totalGastos      = await _controller.GetTotalGastosAsync();
            var balance          = await _controller.GetBalanceAsync();
            var promedioIngresos = await _controller.GetPromedioIngresosAsync();
            var promedioGastos   = await _controller.GetPromedioGastosAsync();
            var totalMovimientos = await _controller.ContarMovimientosAsync();
            var gastosPorCategoria = await _controller.GetGastosPorCategoriaAsync();
            var resumenMensual   = await _controller.GetResumenMensualAsync();

            var porcentajeGastos = totalIngresos > 0
                ? $"{_controller.CalcularPorcentajeGastos(totalGastos, totalIngresos):F1}%"
                : "N/A";

            // Combinamos todas las secciones en una sola tabla Indicador/Valor/Extra
            var filas = new System.Collections.Generic.List<object>();

            // Resumen general
            filas.Add(new { Sección = "General", Indicador = "Total ingresos",      Valor = $"${totalIngresos:F2}"    });
            filas.Add(new { Sección = "General", Indicador = "Total gastos",        Valor = $"${totalGastos:F2}"      });
            filas.Add(new { Sección = "General", Indicador = "Balance",             Valor = $"${balance:F2}"          });
            filas.Add(new { Sección = "General", Indicador = "Promedio ingresos",   Valor = $"${promedioIngresos:F2}" });
            filas.Add(new { Sección = "General", Indicador = "Promedio gastos",     Valor = $"${promedioGastos:F2}"   });
            filas.Add(new { Sección = "General", Indicador = "Movimientos totales", Valor = $"{totalMovimientos}"      });
            filas.Add(new { Sección = "General", Indicador = "% gastos/ingresos",   Valor = porcentajeGastos           });

            // Gastos por categoría
            foreach (var (categoria, total) in gastosPorCategoria.OrderByDescending(x => x.Total))
            {
                var pct = totalGastos > 0 ? $"{(total / totalGastos) * 100:F1}%" : "0%";
                filas.Add(new { Sección = "Categorías", Indicador = categoria, Valor = $"${total:F2} ({pct})" });
            }

            // Resumen mensual
            foreach (var r in resumenMensual.TakeLast(6))
            {
                var nombreMes = new DateTime(r.Anio, r.Mes, 1).ToString("MMM/yyyy");
                filas.Add(new { Sección = "Mensual", Indicador = nombreMes, Valor = $"+${r.TotalIngresos:F2} / -${r.TotalGastos:F2} = ${r.TotalIngresos - r.TotalGastos:F2}" });
            }

            txtTablaTitulo.Text = "Estadísticas completas";
            txtMensaje.Text = "";
            dgDatos.ItemsSource = filas;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
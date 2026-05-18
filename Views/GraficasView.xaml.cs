using FinanzasPersonales.NET.Controllers;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace FinanzasPersonales.NET.Views
{
    public partial class GraficasView : Window, INotifyPropertyChanged
    {
        private readonly EstadisticaController _controller;

        public event PropertyChangedEventHandler? PropertyChanged;

        private ISeries[]? _seriesPastel;
        public ISeries[]? SeriesPastel
        {
            get => _seriesPastel;
            set
            {
                _seriesPastel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeriesPastel)));
            }
        }

        private ISeries[]? _seriesBarras;
        public ISeries[]? SeriesBarras
        {
            get => _seriesBarras;
            set
            {
                _seriesBarras = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeriesBarras)));
            }
        }

        private Axis[]? _ejesXBarras;
        public Axis[]? EjesXBarras
        {
            get => _ejesXBarras;
            set
            {
                _ejesXBarras = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EjesXBarras)));
            }
        }

        public GraficasView(EstadisticaController controller)
        {
            InitializeComponent();
            _controller = controller;
            DataContext = this;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarDatosPastelAsync();
            await CargarDatosBarrasAsync();
        }

        private async System.Threading.Tasks.Task CargarDatosPastelAsync()
        {
            var gastos = await _controller.GetGastosPorCategoriaAsync();
            
            if (gastos == null || !gastos.Any())
            {
                SeriesPastel = Array.Empty<ISeries>();
                return;
            }

            var series = new List<ISeries>();
            foreach (var item in gastos)
            {
                series.Add(new PieSeries<double>
                {
                    Values = new double[] { item.Total },
                    Name = item.Categoria,
                    DataLabelsFormatter = point => $"{item.Categoria}: ${item.Total:N2}"
                });
            }

            SeriesPastel = series.ToArray();
        }

        private async System.Threading.Tasks.Task CargarDatosBarrasAsync()
        {
            var historial = await _controller.GetResumenMensualAsync();
            
            if (historial == null || !historial.Any())
            {
                SeriesBarras = Array.Empty<ISeries>();
                EjesXBarras = Array.Empty<Axis>();
                return;
            }

            // Tomar máximo los últimos 6 meses
            var ultimosMeses = historial.TakeLast(6).ToList();
            
            var valoresIngresos = new double[ultimosMeses.Count];
            var valoresGastos = new double[ultimosMeses.Count];
            var etiquetasMeses = new string[ultimosMeses.Count];

            for (int i = 0; i < ultimosMeses.Count; i++)
            {
                var item = ultimosMeses[i];
                valoresIngresos[i] = item.TotalIngresos;
                valoresGastos[i] = item.TotalGastos;
                etiquetasMeses[i] = new DateTime(item.Anio, item.Mes, 1).ToString("MMM yy").ToUpper();
            }

            SeriesBarras = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Name = "Ingresos",
                    Values = valoresIngresos,
                    Fill = new SolidColorPaint(SKColors.LightGreen)
                },
                new ColumnSeries<double>
                {
                    Name = "Gastos",
                    Values = valoresGastos,
                    Fill = new SolidColorPaint(SKColors.IndianRed)
                }
            };

            EjesXBarras = new Axis[]
            {
                new Axis
                {
                    Labels = etiquetasMeses,
                    LabelsRotation = 15
                }
            };
        }
    }
}

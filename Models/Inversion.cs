using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    public class Inversion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double MontoInicial { get; set; }

        public double ValorActual { get; set; }

        [Required]
        public double TasaInteres { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; } = DateTime.Now;

        public DateTime? FechaVencimiento { get; set; }

        public int? UsuarioId { get; set; }

        public Inversion() 
        {
            FechaInicio = DateTime.Now;
        }

        public Inversion(double montoInicial, double tasaInteres, string descripcion, DateTime? fechaVencimiento = null)
        {
            MontoInicial = montoInicial;
            ValorActual = montoInicial;
            TasaInteres = tasaInteres;
            Descripcion = descripcion;
            FechaInicio = DateTime.Now;
            FechaVencimiento = fechaVencimiento;
        }

        public double CalcularGananciaSimple()
        {
            var tiempoEnAnios = (DateTime.Now - FechaInicio).Days / 365.0;
            return MontoInicial * TasaInteres * tiempoEnAnios;
        }

        public double CalcularGananciaCompuesta()
        {
            var tiempoEnAnios = (DateTime.Now - FechaInicio).Days / 365.0;
            return MontoInicial * Math.Pow(1 + TasaInteres, tiempoEnAnios) - MontoInicial;
        }

        public double CalcularValorActual()
        {
            var tiempoEnAnios = (DateTime.Now - FechaInicio).Days / 365.0;
            return MontoInicial * Math.Pow(1 + TasaInteres, tiempoEnAnios);
        }

        public void ActualizarValor()
        {
            ValorActual = CalcularValorActual();
        }

        public double GetRendimientoPorcentual()
        {
            if (MontoInicial <= 0) return 0;
            return ((ValorActual - MontoInicial) / MontoInicial) * 100;
        }
    }
}

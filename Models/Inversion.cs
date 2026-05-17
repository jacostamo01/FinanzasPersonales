using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    /// <summary>
    /// Representa una inversión financiera.
    /// </summary>
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

        // Relación con Usuario
        public int? UsuarioId { get; set; }

        // Constructor sin parámetros para Entity Framework
        public Inversion() 
        {
            FechaInicio = DateTime.Now;
        }

        // Constructor con parámetros
        public Inversion(double montoInicial, double tasaInteres, string descripcion, DateTime? fechaVencimiento = null)
        {
            MontoInicial = montoInicial;
            ValorActual = montoInicial;
            TasaInteres = tasaInteres;
            Descripcion = descripcion;
            FechaInicio = DateTime.Now;
            FechaVencimiento = fechaVencimiento;
        }

        // Calcula la ganancia con interés simple
        public double CalcularGananciaSimple()
        {
            var tiempoEnAnios = (DateTime.Now - FechaInicio).Days / 365.0;
            return MontoInicial * TasaInteres * tiempoEnAnios;
        }

        // Calcula la ganancia con interés compuesto
        public double CalcularGananciaCompuesta()
        {
            var tiempoEnAnios = (DateTime.Now - FechaInicio).Days / 365.0;
            return MontoInicial * Math.Pow(1 + TasaInteres, tiempoEnAnios) - MontoInicial;
        }

        // Calcula el valor total actual con interés compuesto
        public double CalcularValorActual()
        {
            var tiempoEnAnios = (DateTime.Now - FechaInicio).Days / 365.0;
            return MontoInicial * Math.Pow(1 + TasaInteres, tiempoEnAnios);
        }

        // Actualiza el valor actual de la inversión
        public void ActualizarValor()
        {
            ValorActual = CalcularValorActual();
        }

        // Calcula el rendimiento porcentual
        public double GetRendimientoPorcentual()
        {
            if (MontoInicial <= 0) return 0;
            return ((ValorActual - MontoInicial) / MontoInicial) * 100;
        }
    }
}

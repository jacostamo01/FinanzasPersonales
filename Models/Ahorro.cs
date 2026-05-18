using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    public class Ahorro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double MontoObjetivo { get; set; }

        public double MontoActual { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaObjetivo { get; set; }

        public int? UsuarioId { get; set; }

        public Ahorro() 
        {
            MontoActual = 0.0;
            FechaCreacion = DateTime.Now;
        }

        public Ahorro(double montoObjetivo, string descripcion, DateTime? fechaObjetivo = null)
        {
            MontoObjetivo = montoObjetivo;
            MontoActual = 0.0;
            Descripcion = descripcion;
            FechaCreacion = DateTime.Now;
            FechaObjetivo = fechaObjetivo;
        }

        public void Depositar(double monto)
        {
            if (monto > 0)
            {
                MontoActual += monto;
            }
        }

        public bool Retirar(double monto)
        {
            if (monto > 0 && MontoActual >= monto)
            {
                MontoActual -= monto;
                return true;
            }
            return false;
        }

        public double GetPorcentajeProgreso()
        {
            if (MontoObjetivo <= 0) return 0;
            return Math.Min((MontoActual / MontoObjetivo) * 100, 100);
        }

        public bool ObjetivoAlcanzado => MontoActual >= MontoObjetivo;
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    /// <summary>
    /// Representa una meta de ahorro del usuario.
    /// </summary>
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

        // Relación con Usuario
        public int? UsuarioId { get; set; }

        // Constructor sin parámetros para Entity Framework
        public Ahorro() 
        {
            MontoActual = 0.0;
            FechaCreacion = DateTime.Now;
        }

        // Constructor con parámetros
        public Ahorro(double montoObjetivo, string descripcion, DateTime? fechaObjetivo = null)
        {
            MontoObjetivo = montoObjetivo;
            MontoActual = 0.0;
            Descripcion = descripcion;
            FechaCreacion = DateTime.Now;
            FechaObjetivo = fechaObjetivo;
        }

        // Agrega dinero al ahorro (solo si el monto es positivo)
        public void Depositar(double monto)
        {
            if (monto > 0)
            {
                MontoActual += monto;
            }
        }

        // Retira dinero del ahorro (solo si hay suficiente saldo)
        public bool Retirar(double monto)
        {
            if (monto > 0 && MontoActual >= monto)
            {
                MontoActual -= monto;
                return true;
            }
            return false;
        }

        // Calcula el porcentaje de progreso hacia el objetivo
        public double GetPorcentajeProgreso()
        {
            if (MontoObjetivo <= 0) return 0;
            return Math.Min((MontoActual / MontoObjetivo) * 100, 100);
        }

        // Indica si se ha alcanzado el objetivo
        public bool ObjetivoAlcanzado => MontoActual >= MontoObjetivo;
    }
}

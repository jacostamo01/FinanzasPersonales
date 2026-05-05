using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    /// <summary>
    /// Clase abstracta que representa un movimiento financiero (ingreso o gasto).
    /// </summary>
    public abstract class Movimiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Monto { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        // Relación con Usuario
        public int? UsuarioId { get; set; }

        // Constructor sin parámetros para Entity Framework
        public Movimiento() { }

        // Constructor con parámetros
        public Movimiento(double monto, string descripcion)
        {
            this.Monto = monto;
            this.Descripcion = descripcion;
            this.Fecha = DateTime.Now;
        }

        // Método abstracto
        public abstract string GetTipo();
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    public abstract class Movimiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Monto { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.Now;

        public int? UsuarioId { get; set; }

        public Movimiento() { }

        public Movimiento(double monto, string descripcion)
        {
            this.Monto = monto;
            this.Descripcion = descripcion;
            this.Fecha = DateTime.Now;
        }

        public abstract string GetTipo();
    }
}

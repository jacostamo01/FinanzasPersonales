using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    /// <summary>
    /// Representa un gasto de dinero (hereda de Movimiento).
    /// </summary>
    public class Gasto : Movimiento
    {
        [StringLength(100)]
        public string Categoria { get; set; } = string.Empty;

        // Constructor sin parámetros para Entity Framework
        public Gasto() : base() { }

        public Gasto(double monto, string descripcion, string categoria = "") : base(monto, descripcion) 
        { 
            Categoria = categoria;
        }

        public override string GetTipo()
        {
            return "Gasto";
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    public class Gasto : Movimiento
    {
        [StringLength(100)]
        public string Categoria { get; set; } = string.Empty;

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

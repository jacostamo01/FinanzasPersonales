using System;

namespace FinanzasPersonales.NET.Models
{
    /// <summary>
    /// Representa un ingreso de dinero (hereda de Movimiento).
    /// </summary>
    public class Ingreso : Movimiento
    {
        // Constructor sin parámetros para Entity Framework
        public Ingreso() : base() { }

        public Ingreso(double monto, string descripcion) : base(monto, descripcion) { }

        public override string GetTipo()
        {
            return "Ingreso";
        }
    }
}

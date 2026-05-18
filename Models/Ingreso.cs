using System;

namespace FinanzasPersonales.NET.Models
{
    public class Ingreso : Movimiento
    {
        public Ingreso() : base() { }

        public Ingreso(double monto, string descripcion) : base(monto, descripcion) { }

        public override string GetTipo()
        {
            return "Ingreso";
        }
    }
}

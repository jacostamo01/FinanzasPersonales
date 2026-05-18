using System;
using System.ComponentModel.DataAnnotations;

namespace FinanzasPersonales.NET.Models
{
    public class ColchonFinanciero
    {
        [Key]
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public double MontoActual { get; set; }
        public double Meta { get; set; }
        public double PorcentajeAhorro { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ColchonFinanciero()
        {
            MontoActual = 0.0;
            FechaCreacion = DateTime.Now;
        }

        public ColchonFinanciero(double meta, double porcentajeAhorro)
        {
            Meta = meta;
            PorcentajeAhorro = porcentajeAhorro;
            MontoActual = 0.0;
            FechaCreacion = DateTime.Now;
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
            if (Meta <= 0) return 0;
            return Math.Min((MontoActual / Meta) * 100, 100);
        }

        public bool MetaAlcanzada => MontoActual >= Meta;
    }
}

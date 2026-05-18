using System.Collections.Generic;

namespace FinanzasPersonales.NET.Models
{
    public class CategoriaGasto
    {
        public string Nombre { get; }
        public IReadOnlyList<string> Subcategorias { get; }

        public CategoriaGasto(string nombre, IEnumerable<string> subcategorias)
        {
            Nombre = nombre;
            Subcategorias = new List<string>(subcategorias).AsReadOnly();
        }

        public static IReadOnlyList<CategoriaGasto> ObtenerCategorias()
        {
            return new List<CategoriaGasto>
            {
                new CategoriaGasto("Vivienda", new[]
                {
                    "Arriendo",
                    "Hipoteca",
                    "Impuestos prediales"
                }),
                new CategoriaGasto("Servicios Públicos", new[]
                {
                    "Luz",
                    "Agua",
                    "Gas",
                    "Internet",
                    "Telefonía"
                }),
                new CategoriaGasto("Alimentación", new[]
                {
                    "Supermercado",
                    "Despensa básica"
                }),
                new CategoriaGasto("Salud", new[]
                {
                    "Seguro médico",
                    "Medicamentos",
                    "Consultas"
                }),
                new CategoriaGasto("Transporte", new[]
                {
                    "Combustible",
                    "Transporte público",
                    "Mantenimiento de moto",
                    "Seguros"
                }),
                new CategoriaGasto("Otros", new[]
                {
                    "Otros"
                })
            }.AsReadOnly();
        }
    }
}

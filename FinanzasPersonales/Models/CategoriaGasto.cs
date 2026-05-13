using System.Collections.Generic;

namespace FinanzasPersonales.NET.Models
{
    
    /// Representa una categoría de gasto con sus subcategorías predefinidas.
   
    public class CategoriaGasto
    {
        public string Nombre { get; }
        public IReadOnlyList<string> Subcategorias { get; }

        public CategoriaGasto(string nombre, IEnumerable<string> subcategorias)
        {
            Nombre = nombre;
            Subcategorias = new List<string>(subcategorias).AsReadOnly();
        }

        
        /// Devuelve las categorías de gasto
        
        public static IReadOnlyList<CategoriaGasto> ObtenerCategorias()
        {
            return new List<CategoriaGasto>
            {
                new CategoriaGasto("Vivienda", new[]
                {
                    "Alquiler",
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
                    "Farmacia",
                    "Consultas",
                    "Dentista"
                }),
                new CategoriaGasto("Transporte", new[]
                {
                    "Combustible",
                    "Transporte público",
                    "Mantenimiento del auto",
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

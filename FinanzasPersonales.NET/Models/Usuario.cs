using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanzasPersonales.NET.Models
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// </summary>
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Password { get; set; } = string.Empty;

        [NotMapped] // No mapear a la base de datos si la columna no existe
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Constructor sin parámetros requerido por Entity Framework
        public Usuario() { }

        // Constructor con parámetros
        public Usuario(string username, string password)
        {
            Username = username;
            Password = password;
            FechaCreacion = DateTime.Now;
        }

        // Método para validar la contraseña (simple comparación por ahora)
        public bool ValidarPassword(string password)
        {
            return Password == password;
        }
    }
}

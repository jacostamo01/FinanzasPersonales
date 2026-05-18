using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanzasPersonales.NET.Models
{
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

        [NotMapped] 
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public Usuario() { }

        public Usuario(string username, string password)
        {
            Username = username;
            Password = password;
            FechaCreacion = DateTime.Now;
        }

        public bool ValidarPassword(string password)
        {
            return Password == password;
        }
    }
}

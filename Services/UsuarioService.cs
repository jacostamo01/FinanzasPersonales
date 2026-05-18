using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class UsuarioService
    {
        private readonly FinanzasDbContext _context;

        public UsuarioService(FinanzasDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarUsuarioAsync(string username, string password)
        {
            try
            {
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (usuarioExistente != null)
                    return false;

                var nuevoUsuario = new Usuario(username, password);
                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Usuario?> AutenticarUsuarioAsync(string username, string password)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (usuario == null)
                    return null;

                if (usuario.Password == password)
                    return usuario;

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ExisteUsuarioAsync(string username)
        {
            return await _context.Usuarios.AnyAsync(u => u.Username == username);
        }
    }
}
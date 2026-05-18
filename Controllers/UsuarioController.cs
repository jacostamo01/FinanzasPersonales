using FinanzasPersonales.NET.Models;
using FinanzasPersonales.NET.Services;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Controllers
{
    public class UsuarioController
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public async Task<bool> RegistrarUsuarioAsync(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            if (username.Length < 3 || username.Length > 50)
                return false;

            if (password.Length < 4)
                return false;

            if (password != confirmPassword)
                return false;

            if (await _usuarioService.ExisteUsuarioAsync(username.Trim()))
                return false;

            return await _usuarioService.RegistrarUsuarioAsync(username.Trim(), password);
        }

        public async Task<Usuario?> IniciarSesionAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            return await _usuarioService.AutenticarUsuarioAsync(username.Trim(), password);
        }

        public async Task<bool> ExisteUsuarioAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return await _usuarioService.ExisteUsuarioAsync(username.Trim());
        }
    }
}
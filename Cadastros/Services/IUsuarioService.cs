using Cadastros.Models;
using System.Collections.Generic;

namespace Cadastros.Services
{
    public interface IUsuarioService
    {
        void AdicionarUsuario(AdicionarUsuarioRequest adicionarUsuario);
        Usuario? ObterUsuarioPorId(int id);
        IEnumerable<Usuario> ObterTodosUsuarios();
        void AtualizarUsuario(AdicionarUsuarioRequest adicionarUsuario);
        void RemoverUsuario(int id);
        Usuario AutenticarUsuario(string email, string senha);
    }
}

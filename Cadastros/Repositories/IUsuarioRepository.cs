using Cadastros.Models;
using System.Collections.Generic;

public interface IUsuarioRepository
{
    void Adicionar(Usuario usuario);
    Usuario? ObterPorId(int id);
    IEnumerable<Usuario> ObterTodos();
    void Atualizar(Usuario usuario);
    void Remover(int id);
    bool ExisteUsuario(int id);
    Usuario ObterPorEmail(string email);
}

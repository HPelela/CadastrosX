using Cadastros.Models;
using Cadastros.Services;
using System.Collections.Generic;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public void AdicionarUsuario(AdicionarUsuarioRequest adicionarUsuario)
    {
        var novoUsuario = adicionarUsuario.Usuario;
        var usuarioAtual = _usuarioRepository.ObterPorId(adicionarUsuario.UsuarioAtualId);
        var listaUsuarios = _usuarioRepository.ObterTodos();
        var existeUsuario = !listaUsuarios.Any(x => x.Permissao == Permissao.Diretor || x.Permissao == Permissao.Lider);

        if (usuarioAtual == null && !existeUsuario && novoUsuario.Permissao != Permissao.Funcionario)
        {
            ValidarUsuario(novoUsuario);

            _usuarioRepository.Adicionar(novoUsuario);
        }
        else if (usuarioAtual != null)
        {
            ValidarUsuario(novoUsuario);

            // Validação de permissão
            if ((int)novoUsuario.Permissao > (int)usuarioAtual.Permissao)
                throw new InvalidOperationException("Você não tem permissão para criar um usuário com nível superior.");

            if (novoUsuario.GerenteId.HasValue && !_usuarioRepository.ExisteUsuario(novoUsuario.GerenteId.Value))
                throw new ArgumentException("O gerente especificado não existe.");

            _usuarioRepository.Adicionar(novoUsuario);
        }
        else
            throw new InvalidOperationException("Não foi possivel cadastrar novo usúario.");
    }

    public Usuario? ObterUsuarioPorId(int id)
    {
        return _usuarioRepository.ObterPorId(id);
    }

    public IEnumerable<Usuario> ObterTodosUsuarios()
    {
        return _usuarioRepository.ObterTodos();
    }

    public void AtualizarUsuario(AdicionarUsuarioRequest adicionarUsuario)
    {
        var usuario = adicionarUsuario.Usuario;
       
        if (_usuarioRepository.ObterPorId(usuario.Id) == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        ValidarUsuario(usuario);
        _usuarioRepository.Atualizar(usuario);
    }

    public void RemoverUsuario(int id)
    {
        if (_usuarioRepository.ObterPorId(id) == null)
            throw new KeyNotFoundException("Usuário não encontrado.");

        _usuarioRepository.Remover(id);
    }

    private void ValidarUsuario(Usuario usuario)
    {
        if (string.IsNullOrWhiteSpace(usuario.PrimeiroNome) ||
            string.IsNullOrWhiteSpace(usuario.UltimoNome) ||
            string.IsNullOrWhiteSpace(usuario.Email) ||
            usuario.Telefones == null || !usuario.Telefones.Any())
        {
            throw new ArgumentException("Todos os campos obrigatórios devem ser preenchidos.");
        }

        if (!usuario.Email.Contains("@"))
            throw new ArgumentException("E-mail inválido.");
    }

    public Usuario AutenticarUsuario(string email, string senha)
    {
        var usuario = _usuarioRepository.ObterPorEmail(email);
        if (usuario == null || usuario.Senha != senha)
        {
            return null; // Retorna null se não autenticado
        }
        return usuario; // Retorna o usuário autenticado
    }
}

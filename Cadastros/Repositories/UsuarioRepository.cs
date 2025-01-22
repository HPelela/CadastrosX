using Cadastros.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IConnectionMultiplexer _redis;

    public UsuarioRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void Adicionar(Usuario usuario)
    {
        var db = _redis.GetDatabase();

        // Obtém o próximo ID único do contador no Redis
        var chaveContador = "contador:usuarios";
        long novoId = db.StringIncrement(chaveContador);

        // Define o ID do usuário
        usuario.Id = (int)novoId;

        // Usando o ID como chave
        var chaveUsuario = $"usuario:{usuario.Id}";
        db.StringSet(chaveUsuario, JsonSerializer.Serialize(usuario));

        // Criando um índice para o e-mail
        var chaveEmail = $"usuario:email:{usuario.Email}";
        db.StringSet(chaveEmail, usuario.Id);
    }

    public Usuario? ObterPorId(int id)
    {
        var db = _redis.GetDatabase();
        var chaveUsuario = $"usuario:{id}";
        var dadosUsuario = db.StringGet(chaveUsuario);

        return string.IsNullOrEmpty(dadosUsuario)
            ? null
            : JsonSerializer.Deserialize<Usuario>(dadosUsuario);
    }

    public IEnumerable<Usuario> ObterTodos()
    {
        var db = _redis.GetDatabase();
        var server = _redis.GetServer(_redis.GetEndPoints().First());
        var chaves = server.Keys(pattern: "usuario:*");

        return chaves
            .Where(chave => chave.ToString().StartsWith("usuario:") && !chave.ToString().Contains("email"))
            .Select(chave => db.StringGet(chave))
            .Where(dados => !string.IsNullOrEmpty(dados))
            .Select(dados =>
            {
                try
                {
                    return JsonSerializer.Deserialize<Usuario>(dados);
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Erro ao desserializar usuário: {ex.Message}");
                    return null;
                }
            })
            .Where(usuario => usuario != null)!;
    }

    public void Atualizar(Usuario usuario)
    {
        var db = _redis.GetDatabase();

        // Usando o ID como chave
        var chaveUsuario = $"usuario:{usuario.Id}";

        // Verificar se o usuário existe no banco de dados
        if (!db.KeyExists(chaveUsuario))
        {
            throw new KeyNotFoundException($"Usuário com ID {usuario.Id} não encontrado.");
        }

        // Atualizar os dados do usuário
        db.StringSet(chaveUsuario, JsonSerializer.Serialize(usuario));

        // Atualizar o índice para o e-mail
        var chaveEmail = $"usuario:email:{usuario.Email}";
        db.StringSet(chaveEmail, usuario.Id);
    }

    public void Remover(int id)
    {
        var db = _redis.GetDatabase();

        // Usando o ID como chave
        var chaveUsuario = $"usuario:{id}";

        // Verificar se o usuário existe no banco de dados
        if (!db.KeyExists(chaveUsuario))
        {
            throw new KeyNotFoundException($"Usuário com ID {id} não encontrado.");
        }

        // Remover o usuário
        var dadosUsuario = db.StringGet(chaveUsuario);
        db.KeyDelete(chaveUsuario);

        // Remover o índice de e-mail associado
        if (!string.IsNullOrEmpty(dadosUsuario))
        {
            var usuario = JsonSerializer.Deserialize<Usuario>(dadosUsuario);
            var chaveEmail = $"usuario:email:{usuario?.Email}";
            db.KeyDelete(chaveEmail);
        }
    }

    public bool ExisteUsuario(int id)
    {
        var db = _redis.GetDatabase();
        var chaveUsuario = $"usuario:{id}";
        return db.KeyExists(chaveUsuario);
    }

    public Usuario? ObterPorEmail(string email)
    {
        var db = _redis.GetDatabase();

        // Primeiro, tenta obter o ID do usuário através do índice de e-mail
        var chaveEmail = $"usuario:email:{email}";
        var usuarioId = db.StringGet(chaveEmail);

        if (usuarioId.IsNullOrEmpty)
        {
            return null;  // Não encontrou o usuário
        }

        // Se o ID for encontrado, busca o usuário usando o ID
        var chaveUsuario = $"usuario:{usuarioId}";
        var dadosUsuario = db.StringGet(chaveUsuario);

        if (string.IsNullOrEmpty(dadosUsuario))
        {
            return null;  // Usuário não encontrado
        }

        // Retorna o usuário desserializado
        return JsonSerializer.Deserialize<Usuario>(dadosUsuario);
    }
}

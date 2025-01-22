using Cadastros.Models;
using Cadastros.Repositories;
using System.Text.Json;

namespace Cadastros.Services
{
    public class CadastroService
    {
        private readonly RedisRepository _repository;

        public CadastroService(RedisRepository repository)
        {
            _repository = repository;
        }

        public async Task CadastrarUsuarioAsync(Usuario usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            await _repository.SaveAsync(usuario.Email, json);
        }

        public async Task<Usuario?> ObterUsuarioAsync(string email)
        {
            var json = await _repository.GetAsync(email);
            return json == null ? null : JsonSerializer.Deserialize<Usuario>(json);
        }
    }
}

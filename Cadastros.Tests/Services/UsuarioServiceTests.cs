using Xunit;
using Moq;
using Cadastros.Models;
using Cadastros.Services;
using Cadastros.Repositories;
using System.Collections.Generic;

namespace Cadastros.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepository;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _mockRepository = new Mock<IUsuarioRepository>();
            _usuarioService = new UsuarioService(_mockRepository.Object);
        }

        //[Fact]
        //public void AdicionarUsuario_DeveChamarMetodoDoRepositorio()
        //{
        //    // Arrange
        //    var usuario = new Usuario
        //    {
        //        PrimeiroNome = "João",
        //        UltimoNome = "Silva",
        //        Email = "joao.silva@email.com",
        //        Documento = "12345678900",
        //        Telefones = new List<string> { "123456789" },
        //        NomeGerente = "Carlos",
        //        Senha = "senha123"
        //    };

        //    // Act
        //    _usuarioService.AdicionarUsuario(usuario, usuario);

        //    // Assert
        //    _mockRepository.Verify(r => r.Adicionar(usuario), Times.Once);
        //}

        [Fact]
        public void RemoverUsuario_DeveRemoverUsuarioExistente()
        {
            // Arrange
            var usuarioMock = new Usuario { Id = 1, PrimeiroNome = "João", UltimoNome = "Silva", Email = "joao@teste.com" };
            var repositoryMock = new Mock<IUsuarioRepository>();
            repositoryMock.Setup(r => r.ObterPorId(1)).Returns(usuarioMock);
            repositoryMock.Setup(r => r.Remover(1));

            var service = new UsuarioService(repositoryMock.Object);

            // Act
            service.RemoverUsuario(1);

            // Assert
            repositoryMock.Verify(r => r.Remover(1), Times.Once);
        }
    }
}

using Cadastros.Models;
using Cadastros.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Cadastros.Models.LoginRequest;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public IActionResult AdicionarUsuario([FromBody] AdicionarUsuarioRequest adicionarUsuario)
    {

        try
        {
            _usuarioService.AdicionarUsuario(adicionarUsuario);

            return Ok("Usuário criado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var usuario = _usuarioService.AutenticarUsuario(loginRequest.Email, loginRequest.Senha);
            if (usuario == null)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            return Ok(new
            {
                usuario.Id,
                usuario.PrimeiroNome,
                usuario.UltimoNome,
                usuario.Email,
                usuario.Permissao
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult ObterUsuarioPorId(int id)
    {
        try
        {
            var usuario = _usuarioService.ObterUsuarioPorId(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado.");
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    public IActionResult ObterTodosUsuarios()
    {
        try
        {
            var usuarios = _usuarioService.ObterTodosUsuarios();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public IActionResult AtualizarUsuario(int id, [FromBody] AdicionarUsuarioRequest adicionarUsuario)
    {
        try
        {
            adicionarUsuario.Usuario.Id = id; // A propriedade Id é usada aqui
            _usuarioService.AtualizarUsuario(adicionarUsuario);
            return Ok("Usuário atualizado com sucesso.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public IActionResult RemoverUsuario(int id)
    {
        try
        {
            _usuarioService.RemoverUsuario(id);
            return Ok("Usuário removido com sucesso.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

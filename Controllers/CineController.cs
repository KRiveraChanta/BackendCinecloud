using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using BackendCine.Services;
using BackendCine.Entities.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Company.TestProject1;
using Microsoft.AspNetCore.Authorization;
using BackendCine.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendCine.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _serviceUsuario;
        private readonly IConfiguration _configuration;
        private readonly ClienteService _serviceCliente;

        public AuthController(
            UsuarioService serviceUsuario,
            IConfiguration configuration,
            ClienteService serviceCliente
        )
        {
            _serviceUsuario = serviceUsuario;
            _configuration = configuration;
            _serviceCliente = serviceCliente;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginParam param)
        {
            if (string.IsNullOrEmpty(param.NomUsuario) || string.IsNullOrEmpty(param.Password))
            {
                return BadRequest("Usuario y contraseña no pueden estar vacios.");
            }

            var user = await _serviceUsuario.AuthenticateAsync(param);
            if (user == null)
            {
                return Unauthorized("Usuario inválido.");
            }

            // 1. Preparar Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NomUsuario),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("IdCliente", user.IdCliente.ToString()),
            };

            if (user.Rol != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Rol.NombreRol));
                claims.Add(new Claim("IdRol", user.IdRol.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                user = new
                {
                    user.Id,
                    user.NomUsuario,
                    user.IdCliente,
                    Rol = user.Rol?.IdRol
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioExistente = await _serviceUsuario.GetUsuarioByNomUsuarioAsync(dto.NomUsuario);

            if (usuarioExistente != null)
            {
                return BadRequest("El nombre de usuario ya está registrado.");
            }

            // 1. Crear Cliente
            var cliente = new Cliente
            {
                Documento = dto.Cliente.Documento,
                Nombre = dto.Cliente.Nombre,
                Apellido = dto.Cliente.Apellido,
                FechaNacimiento = dto.Cliente.FechaNacimiento,
                Genero = dto.Cliente.Genero,
                Estado = "Activo",
                swt = true,
                usuario_creacion = 1, // ID temporal
                fecha_creacion = DateTime.Now
            };

            var newCliente = await _serviceCliente.AddClienteAsync(cliente);

            // 2. Crear Usuario y asignarle el Id del Cliente
            var usuario = new Usuario
            {
                NomUsuario = dto.NomUsuario,
                Email = dto.Email,
                // Cifra la contraseña ANTES de guardar
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IdRol = dto.IdRol,
                IdCliente = newCliente.Id,
                swt = true,
                usuario_creacion = 1,
                fecha_creacion = DateTime.Now
            };

            var newUsuario = await _serviceUsuario.AddUsuarioAsync(usuario);


            return Ok(new { message = "Registro exitoso", newUsuario });
        }
    }

    [ApiController]
    [Route("cartelera")]
    public class CarteleraController : ControllerBase
    {
        private CarteleraService _serviceCartelera;
        private GeneroService _serviceGenero;
        public CarteleraController(CarteleraService serviceCartelera, GeneroService serviceGenero/* , SalaService serviceSala */)
        {
            _serviceCartelera = serviceCartelera;
            _serviceGenero = serviceGenero;
        }

        [HttpGet("generos")]
        public async Task<ActionResult> GetGeneros()
        {
            var generos = await _serviceGenero.GetGenerosAsync();
            if (generos == null || !generos.Any())
            {
                return NotFound($"No genders found.");
            }
            return Ok(generos);
        }

    }

    [ApiController]
    [Route("cine")]
    [Authorize]
    public class CineController : ControllerBase
    {
        [HttpGet("")]
        public IActionResult GetCine()
        {
            return Ok(new { Message = "Welcome to the Cine API!" });
        }
    }
}
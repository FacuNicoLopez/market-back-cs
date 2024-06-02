using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUsuarios.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Globalization;
using System.Security.Claims;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly UsuarioContext _context;
        private readonly ILogger<UsuarioController>? _logger;
        public UsuarioController(UsuarioContext context, ILogger<UsuarioController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpGet("{id}/email")]
        public async Task<ActionResult<UsuarioEmailDto>> GetUsuarioEmail(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if(usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            var usuarioEmailDto = new UsuarioEmailDto { Email = usuario.Email};
            return Ok(usuarioEmailDto);
        }

        [HttpGet("{id}/nameUser")]
        public async Task<ActionResult<NameUserDto>> GetUsuarioName(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if(usuario == null)
            {
                return NotFound("usuario no encontrado");
            }

            var usuarioNameDto = new NameUserDto { NameUser = usuario.NameUser};
            return Ok(usuarioNameDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        {

            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email == usuarioDto.Email)
                .FirstOrDefaultAsync();

            if(usuario == null || !BCrypt.Net.BCrypt.Verify(usuarioDto.Password, usuario.Password))
            {
                return Unauthorized("Las credenciales son incorrectas");
            }

            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(usuario.Id.ToString()));
            return Ok(new {Token = token, Email = usuario.Email, nameUser = usuario.NameUser, apellido = usuario.Apellido});
        }

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register([FromBody] UsuarioDto usuarioDto)
        {
            if (_context.Usuarios.Any(u => u.Email == usuarioDto.Email))
            {
                return BadRequest("El correo electrónico ya está registrado.");
            }

            var usuario = new Usuario
            {
                Email = usuarioDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password),
                NameUser = usuarioDto.NameUser,
                Apellido = usuarioDto.Apellido
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, new { usuario.Id, usuario.Email });
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
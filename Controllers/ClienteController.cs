using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUsuarios.Models;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public ClienteController(UsuarioContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginCliente([FromBody] ClienteLoginDto clienteDto)
        {
            var cliente = await _context.Cliente
                .Where(c => c.EmailCliente == clienteDto.EmailCliente)
                .FirstOrDefaultAsync();

            if(cliente == null || !BCrypt.Net.BCrypt.Verify(clienteDto.ClaveCliente, cliente.ClaveCliente))
            {
                return Unauthorized("Las credenciales son incorrectas");
            }


            try
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(clienteDto.ClaveCliente, cliente.ClaveCliente);
                if (!validPassword)
                {
                    return Unauthorized("Las credenciales son incorrectas");
                }
            }
            catch (Exception)
            {
                throw;
            }

            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cliente.IdCliente.ToString()));
            return Ok(new { ClienteId = cliente.IdCliente, Token = token, EmailCliente = cliente.EmailCliente, 
                NombreCliente = cliente.NombreCliente });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Cliente.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if(cliente == null)
            {
                return NotFound();
            }
            return cliente;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] ClienteDto clienteDto)
        {
            if (_context.Cliente.Any(c => c.EmailCliente == clienteDto.EmailCliente))
            {
                return BadRequest("El correo electrónico ya está registrado.");
            }

            var cliente = new Cliente
            {
                EmailCliente = clienteDto.EmailCliente,
                ClaveCliente = BCrypt.Net.BCrypt.HashPassword(clienteDto.ClaveCliente),
                NombreCliente = clienteDto.NombreCliente,
                ApellidoCliente = clienteDto.ApellidoCliente
            };
            var billetera = new Billetera
            {
                Cliente = cliente,
                Saldo = 0.00m
            };

            _context.Cliente.Add(cliente);
            _context.Billeteras.Add(billetera);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente
                .Include(c => c.Carritos)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if(cliente == null)
            {
                return NotFound("Cliente no encontrado");
            }

            if(cliente.Carritos.Any())
            {
                return BadRequest("No se puede eliminar el cliente porque tiene un carrito cargado");
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/edit")]
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if(id != cliente.IdCliente)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Cliente.Any(e => e.IdCliente == id))
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
    }
}

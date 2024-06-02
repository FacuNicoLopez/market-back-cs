
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ApiUsuarios.Models;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public TransaccionController(UsuarioContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RealizarTransaccion([FromBody] TransaccionDto transaccionDto)
        {
            var billetera = await _context.Billeteras.FirstOrDefaultAsync(b => b.ClienteId == transaccionDto.ClienteId);
            if (billetera == null)
            {
                return NotFound("Billetera no encontrada.");
            }

            decimal montoDecimal = (decimal)transaccionDto.Monto;

            if (transaccionDto.TipoTransaccion.ToLower() == "deposito")
            {
                billetera.Saldo += montoDecimal;
            }
            else if (transaccionDto.TipoTransaccion.ToLower() == "retiro")
            {
                if (billetera.Saldo < montoDecimal)
                {
                    return BadRequest("Saldo insuficiente.");
                }
                billetera.Saldo -= montoDecimal;
            }
            else
            {
                return BadRequest("Tipo de transacción inválido.");
            }

            var transaccion = new Transaccion
            {
                BilleteraId = billetera.Id,
                Monto = montoDecimal,
                TipoTransaccion = transaccionDto.TipoTransaccion,
                FechaTransaccion = DateTime.Now
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return Ok(new { SaldoActual = billetera.Saldo });
        }

        [HttpGet("{clienteId}")]
        public async Task<IActionResult> ObtenerTransacciones(int clienteId)
        {
            var billetera = await _context.Billeteras.FirstOrDefaultAsync(b => b.ClienteId == clienteId);
            if (billetera == null)
            {
                return NotFound("Billetera no encontrada.");
            }

            var transacciones = await _context.Transacciones
                .Where(t => t.BilleteraId == billetera.Id)
                .ToListAsync();

            return Ok(transacciones);
        }
    }
}

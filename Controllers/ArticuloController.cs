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
    public class ArticuloController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public ArticuloController(UsuarioContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Articulo>>> GetArticulos()
        {
            return await _context.Articulo.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Articulo>> GetArticulo(int id)
        {
            var articulo = await _context.Articulo.FindAsync(id);
            if(articulo == null)
            {
                return NotFound();
            }
            return articulo;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Articulo>> PostArticulo([FromBody] Articulo articulo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Articulo.Add(articulo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticulo), new { id = articulo.Id }, articulo);
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteArticulo(int id)
        {
            var articulo = await _context.Articulo
                .Include(a => a.Carritos)
                .FirstOrDefaultAsync(a => a.Id == id);
            if(articulo == null)
            {
                return NotFound("Articulo no encontrado");
            }

            if(articulo.Carritos.Any())
            {
                return BadRequest("El articulo no puede ser eliminado porque esta en el carrito de un cliente");
            }

            _context.Articulo.Remove(articulo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/edit")]
        public async Task<IActionResult> UpdateArticulo(int id, Articulo articulo)
        {
            if(id != articulo.Id)
            {
                return BadRequest();
            }

            _context.Entry(articulo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Articulo.Any(e => e.Id == id))
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

        [HttpDelete("{id}/detele/stock")]
        public async Task<IActionResult> DeleteStock(int id, [FromBody] int stockToReduce)
        {
            var articulo = await _context.Articulo.FindAsync(id);
            if(articulo == null)
            {
                return NotFound("Articulo no encontrado");
            }

            if(articulo.Stock < stockToReduce)
            {
                return BadRequest("No hay suficiente stock para reducir");
            }

            articulo.Stock -= stockToReduce;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Articulo.Any(e => e.Id == id))
                {
                    return NotFound("Articulo no encontrado despues de intento de actualizacion");
                }
                else
                {
                    return StatusCode(500, "Error al actualizar el stock del articulo");
                }
            }
        }
}
}

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
    public class CarritoController : ControllerBase
    {
        private readonly UsuarioContext _context;

        public CarritoController(UsuarioContext context)
        {
            _context = context;
        }
        
        [HttpPost("add")]
        public async Task<ActionResult<Carrito>> AddCarrito([FromBody] CarritoDto carritoDto)
        {
            var producto = await _context.Articulo.FindAsync(carritoDto.ArticuloCarrito);
            if(producto == null || producto.Stock < carritoDto.Cantidad)
            {
                return BadRequest("Producto no encontrado o stock insuficiente");
            }

            var existingItem = await _context.Carrito
                .FirstOrDefaultAsync(ci => ci.ClienteCarrito == carritoDto.ClienteCarrito && ci.ArticuloCarrito == carritoDto.ArticuloCarrito);

            if (existingItem != null)
            {
                existingItem.Cantidad += carritoDto.Cantidad;
            }
            else
            {
                existingItem = new Carrito
                {
                    ClienteCarrito = carritoDto.ClienteCarrito,
                    ArticuloCarrito = carritoDto.ArticuloCarrito,
                    Cantidad = carritoDto.Cantidad,
                    FechaAgregado = DateTime.Now
                };
                _context.Carrito.Add(existingItem);
            }

            producto.Stock -= carritoDto.Cantidad;

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCarritoById), new { carritoId = existingItem.Id }, existingItem);
                
        }

        [HttpGet("detalle/{carritoId}")]
        public async Task<ActionResult<CarritoDto>> GetCarritoById(int carritoId)
        {
            var carrito = await _context.Carrito
                .Include(c => c.Articulo)
                .FirstOrDefaultAsync(c => c.Id == carritoId);

            if (carrito == null)
            {
                return NotFound("Carrito no encontrado");
            }

            var carritoDto = new CarritoDto
            {
                ClienteCarrito = carrito.ClienteCarrito,
                ArticuloCarrito = carrito.ArticuloCarrito,
                Cantidad = carrito.Cantidad,
                ArticuloNombre = carrito.Articulo.NombreArticulo,
                Precio = carrito.Articulo.Precio,
                Stock = carrito.Articulo.Stock,
                ImagenUrl = carrito.Articulo.Imagen
            };
            return Ok(carritoDto);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveCarrito(int id)
        {
            var carrito = await _context.Carrito.FindAsync(id);
            if(carrito == null)
            {
                return NotFound("Articulo del carrito no encontrado");
            }

            var producto = await _context.Articulo.FindAsync(carrito.ArticuloCarrito);
            if(producto != null)
            {
                producto.Stock += carrito.Cantidad;
            }

            _context.Carrito.Remove(carrito);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<CarritoDto>>> GetCarrito(int clienteId)
        {
            var carritoItems = await _context.Carrito
                .Where(ci => ci.ClienteCarrito == clienteId)
                .Include(ci => ci.Articulo) 
                .Select(ci => new CarritoDto
                {
                    Id = ci.Id,
                    ClienteCarrito = ci.ClienteCarrito,
                    ArticuloCarrito = ci.ArticuloCarrito,
                    Cantidad = ci.Cantidad,
                    ArticuloNombre = ci.Articulo.NombreArticulo,
                    Precio = ci.Articulo.Precio,
                    Stock = ci.Articulo.Stock,
                    ImagenUrl = ci.Articulo.Imagen
                })
                .ToListAsync();
            foreach (var item in carritoItems) {
            }
            
            if (!carritoItems.Any())
            {
                return NotFound("No se encontraron artículos en el carrito para este cliente.");
            }

            return Ok(carritoItems);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCarrito([FromBody] UpdateCarritoDto updateCarritoDto)
        {

            var carritoItem = await _context.Carrito
                .FirstOrDefaultAsync(
                    ci => ci.ClienteCarrito == updateCarritoDto.ClienteCarrito &&
                    ci.ArticuloCarrito == updateCarritoDto.ArticuloCarrito);
            
            if(carritoItem == null)
            {
                return NotFound("Carrito no encontrado");
            }

            carritoItem.Cantidad = updateCarritoDto.Cantidad;
            _context.Carrito.Update(carritoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("clear/{clienteId}")]
        public async Task<IActionResult> ClearCarrito(int clienteId)
        {
            var carritos = await _context.Carrito.Where(c => c.ClienteCarrito == clienteId).ToListAsync();
            if(carritos == null || !carritos.Any())
            {
                return NotFound("No se encontraron carritos para este cliente");
            }
 
            _context.Carrito.RemoveRange(carritos);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    }


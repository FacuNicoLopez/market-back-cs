using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiUsuarios.Models;

[Route("api/[controller]")]
[ApiController]
public class BilleteraController : ControllerBase
{
    private readonly UsuarioContext _context;

    public BilleteraController(UsuarioContext context)
    {
        _context = context;
    }

    [HttpGet("{clienteId}/saldo")]
    public async Task<IActionResult> ObtenerSaldo(int clienteId)
    {
        var billetera = await _context.Billeteras.FirstOrDefaultAsync(b => b.ClienteId == clienteId);
        if(billetera == null)
        {
            return NotFound("Billetera no encontrada para el usuario con ID: {clienteId}");
        }
        return Ok(billetera.Saldo);
    }
}
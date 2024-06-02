using Microsoft.Identity.Client;

namespace ApiUsuarios.Models{

    public class CarritoDto
    {
        public int Id { get; set; }  
        public int ClienteCarrito { get; set; }
        public int ArticuloCarrito { get; set; }
        public int Cantidad { get; set; }
        public string ArticuloNombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
    }

    public class UpdateCarritoDto
    {
        public int ClienteCarrito { get; set; }
        public int ArticuloCarrito { get; set; }
        public int Cantidad { get; set; }
    }
}
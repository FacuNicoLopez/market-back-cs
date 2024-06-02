using Microsoft.Identity.Client;

namespace ApiUsuarios.Models{
    public class ArticuloDto
    {
        public string NombreArticulo { get; set; }
        public string Talle { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Imagen { get; set; }
    }
}
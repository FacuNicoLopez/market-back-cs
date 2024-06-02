using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUsuarios.Models{

    public class Articulo
    {
        [Key]
        public int Id { get; set; }
        public string NombreArticulo { get; set; }
        public string Talle { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; }
        public List<Carrito> Carritos { get; set; } = new List<Carrito>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUsuarios.Models{
    public class Carrito
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteCarrito { get; set; }
        public Cliente Cliente { get; set; }
        [ForeignKey("Articulo")]
        public int ArticuloCarrito { get; set; }
        public Articulo Articulo { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAgregado { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiUsuarios.Models{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }
        public string EmailCliente { get; set; }
        public string ClaveCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public List<Carrito> Carritos { get; set; } = new List<Carrito>();
        public virtual Billetera Billetera { get; set; }
    }
}
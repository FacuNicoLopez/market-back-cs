

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiUsuarios.Models
{
    public class Billetera
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }
        public List<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }
}
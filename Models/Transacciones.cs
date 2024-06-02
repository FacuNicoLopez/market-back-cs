
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace ApiUsuarios.Models
{
    public class Transaccion
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Billetera")]
        public int BilleteraId { get; set; }
        public Billetera Billetera { get; set; }
        public decimal Monto { get; set; }
        public string TipoTransaccion { get; set; }
        public DateTime FechaTransaccion { get; set; }
    }
}
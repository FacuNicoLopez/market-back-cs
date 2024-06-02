using Microsoft.Identity.Client;

namespace ApiUsuarios.Models{
    public class ClienteLoginDto
    {
        public string EmailCliente { get; set; }
        public string ClaveCliente { get; set; }
    }

    public class ClienteDto
    {
        public string EmailCliente { get; set; }
        public string ClaveCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
    }

    public class DepositoDto
    {
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
    }

    public class TransaccionDto
    {
        public int ClienteId { get; set; }
        public decimal Monto { get; set; }
        public string TipoTransaccion { get; set; }
    }
}